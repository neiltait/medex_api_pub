using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.API.Services;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests
{
    /// <summary>
    /// Okta Jwt Security Token Handler Tests
    /// </summary>
    public class OktaJwtSecurityTokenHandlerTests
    {
        private Mock<ITokenService> _mockTokenService;

        private Mock<ISecurityTokenValidator> _mockSecurityTokenValidator;

        private OktaJwtSecurityTokenHandler sut;

        public OktaJwtSecurityTokenHandlerTests()
        {
            _mockTokenService = new Mock<ITokenService>();

            _mockSecurityTokenValidator = new Mock<ISecurityTokenValidator>();

            sut = new OktaJwtSecurityTokenHandler(_mockTokenService.Object, _mockSecurityTokenValidator.Object);
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
        public void CanReadToken_ShouldCallCanReadOnHandler()
        {
            _mockSecurityTokenValidator
                .Setup(stv => stv.CanReadToken(It.IsAny<string>()))
                .Returns(true);

            sut.CanReadToken(It.IsAny<string>()).Should().BeTrue();
        }

        [Fact]
        public void ValidateToken_ShouldReturnClaimsPrincipal_WhenValidTokenPassedAndIntrospectResponseIsActive()
        {
            var expectedToken = "expectedToken";
            var expectedValidatedToken = new Mock<SecurityToken>().Object;
            var expectedTokenValidationParameters = new TokenValidationParameters();
            var expectedIntrospectResponse = new IntrospectResponse()
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
            var expectedIntrospectResponse = new IntrospectResponse()
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

            Action action = () => sut.ValidateToken(expectedToken, expectedTokenValidationParameters, out expectedValidatedToken)
                .Should().Be(expectedClaims);

            action.Should().Throw<SecurityTokenValidationException>();

            expectedValidatedToken.Should().BeNull();
        }
    }
}
