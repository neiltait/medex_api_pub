using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Okta.Sdk;
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

            sut.ValidateToken(expectedToken, expectedTokenValidationParameters, out expectedValidatedToken)
                .Should().Be(expectedClaims);
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
    }
}