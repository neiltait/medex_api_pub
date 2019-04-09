using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Common;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Common.Services.User;
using MedicalExaminer.Models;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests
{
    /// <summary>
    ///     Okta Jwt Security Token Handler Tests
    /// </summary>
    public class OktaJwtSecurityTokenHandlerTests
    {
        public OktaJwtSecurityTokenHandlerTests()
        {
            _mockTokenService = new Mock<ITokenService>();

            _mockSecurityTokenValidator = new Mock<ISecurityTokenValidator>();

            _mockUserUpdateOktaTokenService = new Mock<IAsyncQueryHandler<UsersUpdateOktaTokenQuery, MeUser>>();

            _mockUserRetrieveOktaTokenService = new Mock<IAsyncQueryHandler<UserRetrievalByOktaTokenQuery, MeUser>>();

            _mockUsersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();

            var tokenExpiryTime = 30;

            sut = new OktaJwtSecurityTokenHandler(_mockTokenService.Object, _mockSecurityTokenValidator.Object, _mockUserUpdateOktaTokenService.Object, _mockUserRetrieveOktaTokenService.Object, _mockUsersRetrievalByEmailService.Object, tokenExpiryTime);
        }

        private readonly Mock<ITokenService> _mockTokenService;

        private readonly Mock<ISecurityTokenValidator> _mockSecurityTokenValidator;

        private readonly Mock<IAsyncQueryHandler<UsersUpdateOktaTokenQuery, MeUser>> _mockUserUpdateOktaTokenService;

        private readonly Mock<IAsyncQueryHandler<UserRetrievalByOktaTokenQuery, MeUser>>
            _mockUserRetrieveOktaTokenService;

        private readonly Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>> _mockUsersRetrievalByEmailService;

        private readonly OktaJwtSecurityTokenHandler sut;

        [Fact]
        public void CanReadToken_ShouldCallCanReadOnHandler()
        {
            _mockSecurityTokenValidator
                .Setup(stv => stv.CanReadToken(It.IsAny<string>()))
                .Returns(true);

            sut.CanReadToken(It.IsAny<string>()).Should().BeTrue();
        }

        [Fact]
        public void CanValidateToken_ShouldReturnTrue()
        {
            sut.CanValidateToken.Should().BeTrue();
        }

        [Fact]
        public void MaximumTokenSizeInBytesGet_ShouldReturnDefault()
        {
            sut.MaximumTokenSizeInBytes.Should().Be(TokenValidationParameters.DefaultMaximumTokenSizeInBytes);
        }

        [Fact]
        public void MaximumTokenSizeInBytesSet_ShouldThrowException()
        {
            Action act = () => sut.MaximumTokenSizeInBytes = 23;
            act.Should().Throw<NotImplementedException>();
        }

        [Fact]
        public void ValidateToken_ShouldReturnClaimsPrincipal_WhenValidTokenPassedAndIntrospectResponseIsActive()
        {
            var expectedToken = "expectedToken";
            var expectedValidatedToken = new Mock<SecurityToken>().Object;
            var expectedTokenValidationParameters = new TokenValidationParameters();
            var expectedIntrospectResponse = new IntrospectResponse
            {
                Active = true
            };
            var mockExpectedClaims = new Mock<ClaimsPrincipal>();
            var claims = new List<Claim>();
            var claim = new Claim(ClaimTypes.Email, "joe.doe@nhs.co.uk");
            claims.Add(claim);
            mockExpectedClaims.Setup(cp => cp.Claims).Returns(claims);

            _mockTokenService
                .Setup(ts => ts.IntrospectToken(expectedToken, It.IsAny<HttpClient>()))
                .Returns(Task.FromResult(expectedIntrospectResponse));

            _mockSecurityTokenValidator.Setup(stv =>
                    stv.ValidateToken(
                        expectedToken,
                        expectedTokenValidationParameters,
                        out expectedValidatedToken))
                .Returns(mockExpectedClaims.Object);

            sut.ValidateToken(expectedToken, expectedTokenValidationParameters, out expectedValidatedToken)
                .Should().Be(mockExpectedClaims.Object);
        }

        [Fact]
        public void ValidateToken_ShouldThrowException_WhenIntrospectResponseIsNotActive()
        {
            var expectedToken = "expectedToken";
            var expectedValidatedToken = new Mock<SecurityToken>().Object;
            var expectedTokenValidationParameters = new TokenValidationParameters();
            var expectedIntrospectResponse = new IntrospectResponse
            {
                Active = false
            };
            var expectedClaims = new ClaimsPrincipal();

            _mockTokenService
                .Setup(ts => ts.IntrospectToken(expectedToken, It.IsAny<HttpClient>()))
                .Returns(Task.FromResult(expectedIntrospectResponse));

            _mockSecurityTokenValidator.Setup(stv =>
                    stv.ValidateToken(
                        expectedToken,
                        expectedTokenValidationParameters,
                        out expectedValidatedToken))
                .Returns(expectedClaims);

            Action action = () => sut
                .ValidateToken(expectedToken, expectedTokenValidationParameters, out expectedValidatedToken)
                .Should().Be(expectedClaims);

            action.Should().Throw<SecurityTokenValidationException>();

            expectedValidatedToken.Should().BeNull();
        }

        [Fact]
        public void ValidateToken_BypassOktaIfUserFoundAndTokenNotExpired()
        {
            //Arrange
            var securityToken = "Token1";
            var tokenExpiryTime = DateTime.Now.AddMinutes(30);
            var expectedTokenValidationParameters = new TokenValidationParameters();
            var expectedValidatedToken = new Mock<SecurityToken>().Object;
            var meUserExisting = new MeUser
            {
                OktaToken = securityToken,
                OktaTokenExpiry = tokenExpiryTime
            };

            _mockUserRetrieveOktaTokenService.Setup(ts => ts.Handle(It.IsAny<UserRetrievalByOktaTokenQuery>()))
                .Returns(Task.FromResult(meUserExisting));
            _mockTokenService.Setup(ts => ts.IntrospectToken(securityToken, It.IsAny<HttpClient>()));

            //Act
            sut.ValidateToken(securityToken, expectedTokenValidationParameters, out expectedValidatedToken);

            //Assert
            _mockTokenService.Verify(ts => ts.IntrospectToken(securityToken, It.IsAny<HttpClient>()), Times.Never());
        }

        [Fact]
        public void ValidateToken_CallOktaIfUserFoundByTokenButExpired()
        {
            //Arrange
            var securityToken = "Token1";
            var tokenExpiryTime = DateTime.Now.AddMinutes(-30); //expired!
            var expectedTokenValidationParameters = new TokenValidationParameters();
            var meUserExisting = new MeUser
            {
                OktaToken = securityToken,
                OktaTokenExpiry = tokenExpiryTime
            };
            var expectedValidatedToken = new Mock<SecurityToken>().Object;
            var introspectResponse = new IntrospectResponse
            {
                Active = true
            };

            _mockUserRetrieveOktaTokenService.Setup(ts => ts.Handle(It.IsAny<UserRetrievalByOktaTokenQuery>()))
                .Returns(Task.FromResult(meUserExisting));
            _mockTokenService.Setup(ts => ts.IntrospectToken(securityToken, It.IsAny<HttpClient>())).Returns(Task.FromResult(introspectResponse));

            //Act
            sut.ValidateToken(securityToken, expectedTokenValidationParameters, out expectedValidatedToken);

            //Assert
            _mockTokenService.Verify(ts => ts.IntrospectToken(securityToken, It.IsAny<HttpClient>()), Times.AtLeastOnce);
        }

        [Fact]
        public void ValidateToken_ExpiredTokenReset()
        {
            //Arrange
            var securityToken = "Token1";
            var tokenExpiryTime = DateTime.Now.AddMinutes(-30); //expired!
            var expectedTokenValidationParameters = new TokenValidationParameters();
            var meUserExisting = new MeUser
            {
                OktaToken = securityToken,
                OktaTokenExpiry = tokenExpiryTime
            };
            var expectedValidatedToken = new Mock<SecurityToken>().Object;
            var introspectResponse = new IntrospectResponse
            {
                Active = true
            };
            var claimsPrincipal = new Mock<ClaimsPrincipal>();
            var claims = new List<Claim>();
            var claim = new Claim(ClaimTypes.Email, "joe.doe@nhs.co.uk");
            claims.Add(claim);
            claimsPrincipal.Setup(cp => cp.Claims).Returns(claims);

            _mockUserRetrieveOktaTokenService.Setup(ts => ts.Handle(It.IsAny<UserRetrievalByOktaTokenQuery>()))
                .Returns(Task.FromResult(meUserExisting));
            _mockTokenService.Setup(ts => ts.IntrospectToken(securityToken, It.IsAny<HttpClient>())).Returns(Task.FromResult(introspectResponse));
            _mockUserUpdateOktaTokenService.Setup(ts => ts.Handle(It.IsAny<UsersUpdateOktaTokenQuery>()));
            _mockSecurityTokenValidator.Setup(tv => tv.ValidateToken(securityToken, expectedTokenValidationParameters, out expectedValidatedToken)).Returns(claimsPrincipal.Object);
            _mockUsersRetrievalByEmailService.Setup(es => es.Handle(It.IsAny<UserRetrievalByEmailQuery>()))
                .Returns(Task.FromResult(meUserExisting));

            //Act
            sut.ValidateToken(securityToken, expectedTokenValidationParameters, out expectedValidatedToken);

            //Assert
            _mockUserUpdateOktaTokenService.Verify(ts => ts.Handle(It.IsAny<UsersUpdateOktaTokenQuery>()), Times.AtLeastOnce);
        }
    }
}