using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.API.Authorization;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Okta.Sdk;
using Okta.Sdk.Configuration;
using Xunit;

namespace MedicalExaminer.API.Tests
{
    /// <summary>
    ///     Okta Jwt Security Token Handler Tests
    /// </summary>
    public class OktaJwtSecurityTokenHandlerTests
    {
        private const int TokenExpiryTime = 30;

        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<ISecurityTokenValidator> _mockTokenValidator;
        private readonly Mock<IAsyncQueryHandler<UserSessionUpdateOktaTokenQuery, MeUserSession>> _mockUserSessionUpdateOktaTokenService;
        private readonly Mock<IAsyncQueryHandler<UserSessionRetrievalByOktaIdQuery, MeUserSession>> _mockUserSessionRetrievalByOktaIdService;
        private readonly Mock<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>> _mockUserRetrievalById;
        private readonly Mock<IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser>> _mockUserRetrievalByOktaId;
        private readonly Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>> _mockUserRetrievalByEmailService;
        private readonly Mock<IAsyncQueryHandler<UserUpdateQuery, MeUser>> _mockUserUpdateService;
        private readonly Mock<IAsyncQueryHandler<CreateUserQuery, MeUser>> _mockUserCreationService;
        private readonly OktaClient _oktaClient;

        private readonly OktaJwtSecurityTokenHandler sut;

        public OktaJwtSecurityTokenHandlerTests()
        {
            _mockTokenService = new Mock<ITokenService>();
            _mockTokenValidator = new Mock<ISecurityTokenValidator>();
            _mockUserSessionUpdateOktaTokenService =new Mock<IAsyncQueryHandler<UserSessionUpdateOktaTokenQuery, MeUserSession>>();
            _mockUserSessionRetrievalByOktaIdService =new Mock<IAsyncQueryHandler<UserSessionRetrievalByOktaIdQuery, MeUserSession>>();
            _mockUserRetrievalById = new Mock<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>>();
            _mockUserRetrievalByOktaId = new Mock<IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser>>();
            _mockUserRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            _mockUserUpdateService = new Mock<IAsyncQueryHandler<UserUpdateQuery, MeUser>>();
            _mockUserCreationService = new Mock<IAsyncQueryHandler<CreateUserQuery, MeUser>>();
            var oktaClientConfiguration = new OktaClientConfiguration
            {
                OktaDomain = "https://xxx-123456.test.com",
                Token = "Token1",
            };
            _oktaClient = new OktaClient(oktaClientConfiguration);

            sut = new OktaJwtSecurityTokenHandler(
                _mockTokenService.Object,
                _mockTokenValidator.Object,
                _mockUserSessionUpdateOktaTokenService.Object,
                _mockUserSessionRetrievalByOktaIdService.Object,
                _mockUserRetrievalById.Object,
                _mockUserRetrievalByOktaId.Object,
                _mockUserRetrievalByEmailService.Object,
                _mockUserUpdateService.Object,
                _mockUserCreationService.Object,
                _oktaClient,
                TokenExpiryTime);
        }

        [Fact]
        public void CanReadToken_ShouldCallCanReadOnHandler()
        {
            _mockTokenValidator
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
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void ValidateToken_ShouldReturnClaimsPrincipal_WhenValidTokenPassedAndIntrospectResponseIsActive()
        {
            var expectedUserId = "userId";
            var expectedOktaId = "oktaId";
            var expectedToken = "expectedToken";
            var expectedValidatedToken = new Mock<SecurityToken>().Object;
            var expectedTokenValidationParameters = new TokenValidationParameters();
            var expectedIntrospectResponse = new IntrospectResponse
            {
                Active = true
            };
            var mockExpectedClaims = new Mock<ClaimsPrincipal>();
            var claims = new List<Claim>();
            var claim = new Claim(MEClaimTypes.OktaUserId, expectedOktaId);
            claims.Add(claim);
            mockExpectedClaims.Setup(cp => cp.Claims).Returns(claims);

            _mockTokenService
                .Setup(ts => ts.IntrospectToken(expectedToken, It.IsAny<HttpClient>()))
                .Returns(Task.FromResult(expectedIntrospectResponse));

            var expectedSession = new MeUserSession
            {
                UserId = expectedUserId,
                OktaId = expectedOktaId,
                OktaToken = expectedToken,
                OktaTokenExpiry = DateTimeOffset.Now.AddDays(1),
            };

            _mockUserSessionRetrievalByOktaIdService
                .Setup(service => service.Handle(It.IsAny<UserSessionRetrievalByOktaIdQuery>()))
                .Returns(Task.FromResult(expectedSession));

            _mockTokenValidator.Setup(stv =>
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

            _mockTokenValidator.Setup(stv =>
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
        /*
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
                UserId = "mockUserId",
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
            var claim = new Claim(MEClaimTypes.OktaUserId, "oktaId");
            claims.Add(claim);
            var identities = new List<ClaimsIdentity>();
            var identity = new ClaimsIdentity();
            identities.Add(identity);
            claimsPrincipal.Setup(cp => cp.Claims).Returns(claims);
            claimsPrincipal.Setup(cp => cp.Identities).Returns(identities);
            _mockUserRetrieveOktaTokenService.Setup(ts => ts.Handle(It.IsAny<UserRetrievalByOktaTokenQuery>()))
                .Returns(Task.FromResult(meUserExisting));
            _mockTokenService.Setup(ts => ts.IntrospectToken(securityToken, It.IsAny<HttpClient>())).Returns(Task.FromResult(introspectResponse));
            _mockUserUpdateOktaTokenService.Setup(ts => ts.Handle(It.IsAny<UsersUpdateOktaTokenQuery>()));
            _mockSecurityTokenValidator.Setup(tv => tv.ValidateToken(securityToken, expectedTokenValidationParameters, out expectedValidatedToken)).Returns(claimsPrincipal.Object);
            _mockUsersRetrievalByOktaIdService.Setup(es => es.Handle(It.IsAny<UserRetrievalByOktaIdQuery>()))
                .Returns(Task.FromResult(meUserExisting));

            //Act
            sut.ValidateToken(securityToken, expectedTokenValidationParameters, out expectedValidatedToken);

            //Assert
            _mockUserUpdateOktaTokenService.Verify(ts => ts.Handle(It.IsAny<UsersUpdateOktaTokenQuery>()), Times.AtLeastOnce);
        }*/
    }
}