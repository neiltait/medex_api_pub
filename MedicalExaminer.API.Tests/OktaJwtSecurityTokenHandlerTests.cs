using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
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
        private readonly Mock<IOktaClient> _mockOktaClient;

        private readonly OktaJwtSecurityTokenHandler sut;

        public OktaJwtSecurityTokenHandlerTests()
        {
            _mockTokenService = new Mock<ITokenService>(MockBehavior.Strict);
            _mockTokenValidator = new Mock<ISecurityTokenValidator>(MockBehavior.Strict);
            _mockUserSessionUpdateOktaTokenService =new Mock<IAsyncQueryHandler<UserSessionUpdateOktaTokenQuery, MeUserSession>>(MockBehavior.Strict);
            _mockUserSessionRetrievalByOktaIdService =new Mock<IAsyncQueryHandler<UserSessionRetrievalByOktaIdQuery, MeUserSession>>(MockBehavior.Strict);
            _mockUserRetrievalById = new Mock<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>>(MockBehavior.Strict);
            _mockUserRetrievalByOktaId = new Mock<IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser>>(MockBehavior.Strict);
            _mockUserRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>(MockBehavior.Strict);
            _mockUserUpdateService = new Mock<IAsyncQueryHandler<UserUpdateQuery, MeUser>>(MockBehavior.Strict);
            _mockUserCreationService = new Mock<IAsyncQueryHandler<CreateUserQuery, MeUser>>(MockBehavior.Strict);
            _mockOktaClient = new Mock<IOktaClient>();

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
                _mockOktaClient.Object,
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
        public void ValidateToken_ShouldThrowException_WhenClaimsPrincipalNull()
        {
            var expectedUserId = "userId";
            var expectedOktaId = "oktaId";
            var expectedToken = "expectedToken";
            var expectedValidatedToken = new Mock<SecurityToken>().Object;
            var expectedTokenValidationParameters = new TokenValidationParameters();
            var expectedIntrospectResponse = new IntrospectResponse
            {
                Active = false
            };

            _mockTokenService
                .Setup(ts => ts.IntrospectToken(expectedToken, It.IsAny<HttpClient>()))
                .Returns(Task.FromResult(expectedIntrospectResponse));

            _mockTokenValidator.Setup(stv =>
                    stv.ValidateToken(
                        expectedToken,
                        expectedTokenValidationParameters,
                        out expectedValidatedToken))
                .Returns((ClaimsPrincipal)null);

            Action action = () => sut
                .ValidateToken(expectedToken, expectedTokenValidationParameters, out expectedValidatedToken);

            action.Should().Throw<SecurityTokenValidationException>();

            expectedValidatedToken.Should().BeNull();
        }

        [Fact]
        public void ValidateToken_ShouldThrowException_WhenClaimsPrincipalMissingOktaId()
        {
            var expectedUserId = "userId";
            var expectedOktaId = "oktaId";
            var expectedToken = "expectedToken";
            var expectedValidatedToken = new Mock<SecurityToken>().Object;
            var expectedTokenValidationParameters = new TokenValidationParameters();
            var expectedIntrospectResponse = new IntrospectResponse
            {
                Active = false
            };
            var mockExpectedClaims = new Mock<ClaimsPrincipal>();
            mockExpectedClaims
                .Setup(cp => cp.Claims)
                .Returns(Enumerable.Empty<Claim>());
            mockExpectedClaims
                .Setup(cp => cp.Identities)
                .Returns(Enumerable.Empty<ClaimsIdentity>());

            _mockTokenService
                .Setup(ts => ts.IntrospectToken(expectedToken, It.IsAny<HttpClient>()))
                .Returns(Task.FromResult(expectedIntrospectResponse));

            _mockTokenValidator.Setup(stv =>
                    stv.ValidateToken(
                        expectedToken,
                        expectedTokenValidationParameters,
                        out expectedValidatedToken))
                .Returns(mockExpectedClaims.Object);

            Action action = () => sut
                .ValidateToken(expectedToken, expectedTokenValidationParameters, out expectedValidatedToken);

            action.Should().Throw<SecurityTokenValidationException>();

            expectedValidatedToken.Should().BeNull();
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
            var identities = new List<ClaimsIdentity>();
            var identity = new ClaimsIdentity();
            identities.Add(identity);
            mockExpectedClaims.Setup(cp => cp.Identities).Returns(identities);

            _mockTokenService
                .Setup(ts => ts.IntrospectToken(expectedToken, It.IsAny<HttpClient>()))
                .Returns(Task.FromResult(expectedIntrospectResponse));

            var expectedSession = new MeUserSession
            {
                UserId = expectedUserId,
                OktaId = expectedOktaId,
                OktaToken = expectedToken,
                OktaTokenExpiry = DateTimeOffset.Now.AddDays(-1),
            };

            _mockUserSessionRetrievalByOktaIdService
                .Setup(service => service.Handle(It.IsAny<UserSessionRetrievalByOktaIdQuery>()))
                .Returns(Task.FromResult(expectedSession))
                .Verifiable();

            _mockTokenValidator.Setup(stv =>
                    stv.ValidateToken(
                        expectedToken,
                        expectedTokenValidationParameters,
                        out expectedValidatedToken))
                .Returns(mockExpectedClaims.Object);

            _mockUserSessionUpdateOktaTokenService
                .Setup(service => service.Handle(It.IsAny<UserSessionUpdateOktaTokenQuery>()))
                .Returns((UserSessionUpdateOktaTokenQuery param) => Task.FromResult(new MeUserSession()
                {
                    UserId = param.UserId,
                    OktaId = expectedSession.OktaId,
                    OktaToken = param.OktaToken,
                    OktaTokenExpiry = param.OktaTokenExpiry,
                }))
                .Verifiable();

            sut.ValidateToken(expectedToken, expectedTokenValidationParameters, out expectedValidatedToken)
                .Should().Be(mockExpectedClaims.Object);

            _mockTokenService
                .Verify(ts => ts.IntrospectToken(expectedToken, It.IsAny<HttpClient>()));

            _mockUserSessionUpdateOktaTokenService
                .Verify(service => service.Handle(It.IsAny<UserSessionUpdateOktaTokenQuery>()));
        }

        [Fact]
        public void ValidateToken_ShouldThrowException_WhenIntrospectResponseIsNotActive()
        {
            var expectedUserId = "userId";
            var expectedOktaId = "oktaId";
            var expectedToken = "expectedToken";
            var expectedValidatedToken = new Mock<SecurityToken>().Object;
            var expectedTokenValidationParameters = new TokenValidationParameters();
            var expectedIntrospectResponse = new IntrospectResponse
            {
                Active = false
            };
            var mockExpectedClaims = new Mock<ClaimsPrincipal>();
            var claims = new List<Claim>();
            var claim = new Claim(MEClaimTypes.OktaUserId, expectedOktaId);
            claims.Add(claim);
            mockExpectedClaims.Setup(cp => cp.Claims).Returns(claims);
            var identities = new List<ClaimsIdentity>();
            var identity = new ClaimsIdentity();
            identities.Add(identity);
            mockExpectedClaims.Setup(cp => cp.Identities).Returns(identities);

            _mockUserSessionRetrievalByOktaIdService
                .Setup(service => service.Handle(It.IsAny<UserSessionRetrievalByOktaIdQuery>()))
                .Returns(Task.FromResult((MeUserSession)null))
                .Verifiable();

            _mockTokenService
                .Setup(ts => ts.IntrospectToken(expectedToken, It.IsAny<HttpClient>()))
                .Returns(Task.FromResult(expectedIntrospectResponse));

            _mockTokenValidator.Setup(stv =>
                    stv.ValidateToken(
                        expectedToken,
                        expectedTokenValidationParameters,
                        out expectedValidatedToken))
                .Returns(mockExpectedClaims.Object);

            Action action = () => sut
                .ValidateToken(expectedToken, expectedTokenValidationParameters, out expectedValidatedToken)
                .Should().Be(mockExpectedClaims.Object);

            action.Should().Throw<SecurityTokenValidationException>();

            expectedValidatedToken.Should().BeNull();
        }

        [Fact]
        public void ValidateToken_ShouldReturnClaimsPrincipalAndUpdateUserInfo_WhenValidTokenChanged()
        {
            // Arrange
            var expectedUserId = "userId";
            var expectedOktaId = "oktaId";
            var expectedToken = "expectedToken";
            var expectedOriginalToken = "expectedOriginalToken";
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
            var identities = new List<ClaimsIdentity>();
            var identity = new ClaimsIdentity();
            identities.Add(identity);
            mockExpectedClaims.Setup(cp => cp.Identities).Returns(identities);

            _mockTokenService
                .Setup(ts => ts.IntrospectToken(expectedToken, It.IsAny<HttpClient>()))
                .Returns(Task.FromResult(expectedIntrospectResponse));

            var expectedSession = new MeUserSession
            {
                UserId = expectedUserId,
                OktaId = expectedOktaId,
                OktaToken = expectedOriginalToken,
                OktaTokenExpiry = DateTimeOffset.Now.AddDays(-1),
            };

            var expectedUser = new MeUser
            {
                UserId = expectedUserId,
                FirstName = "currentFirstName",
                LastName = "currentLastName",
                Email = "currentEmail",
            };

            var expectedOktaUser = new Mock<IUser>(MockBehavior.Strict);
            expectedOktaUser.Setup(o => o.Profile.FirstName).Returns("oktaFirstName");
            expectedOktaUser.Setup(o => o.Profile.LastName).Returns("oktaLastName");
            expectedOktaUser.Setup(o => o.Profile.Email).Returns("oktaEmail");

            var mockUsersClient = new Mock<IUsersClient>(MockBehavior.Strict);

            mockUsersClient
                .Setup(service => service.GetUserAsync(It.IsAny<string>(), default(CancellationToken)))
                .Returns(Task.FromResult(expectedOktaUser.Object));

            _mockOktaClient
                .Setup(service => service.Users)
                .Returns(mockUsersClient.Object);

            _mockUserSessionRetrievalByOktaIdService
                .Setup(service => service.Handle(It.IsAny<UserSessionRetrievalByOktaIdQuery>()))
                .Returns(Task.FromResult(expectedSession))
                .Verifiable();

            _mockTokenValidator.Setup(stv =>
                    stv.ValidateToken(
                        expectedToken,
                        expectedTokenValidationParameters,
                        out expectedValidatedToken))
                .Returns(mockExpectedClaims.Object);

            _mockUserSessionUpdateOktaTokenService
                .Setup(service => service.Handle(It.IsAny<UserSessionUpdateOktaTokenQuery>()))
                .Returns((UserSessionUpdateOktaTokenQuery param) => Task.FromResult(new MeUserSession()
                {
                    UserId = param.UserId,
                    OktaId = expectedSession.OktaId,
                    OktaToken = param.OktaToken,
                    OktaTokenExpiry = param.OktaTokenExpiry,
                }))
                .Verifiable();

            _mockUserRetrievalById
                .Setup(service => service.Handle(It.IsAny<UserRetrievalByIdQuery>()))
                .Returns(Task.FromResult(expectedUser));

            _mockUserUpdateService
                .Setup(service => service.Handle(It.IsAny<UserUpdateQuery>()))
                .Returns((UserUpdateQuery query) => Task.FromResult(new MeUser
                {
                    UserId = query.UserUpdate.UserId,
                }))
                .Verifiable();

            // Act
            sut.ValidateToken(expectedToken, expectedTokenValidationParameters, out expectedValidatedToken)
                .Should().Be(mockExpectedClaims.Object);

            // Assert
            _mockTokenService
                .Verify(ts => ts.IntrospectToken(expectedToken, It.IsAny<HttpClient>()));

            _mockUserSessionUpdateOktaTokenService
                .Verify(service => service.Handle(It.IsAny<UserSessionUpdateOktaTokenQuery>()));

            _mockUserUpdateService
                .Verify(service => service.Handle(It.IsAny<UserUpdateQuery>()));
        }

        [Fact]
        public void ValidateToken_ShouldThrowException_WhenValidTokenChangedButUserDoesntExist()
        {
            // Arrange
            var expectedUserId = "userId";
            var expectedOktaId = "oktaId";
            var expectedToken = "expectedToken";
            var expectedOriginalToken = "expectedOriginalToken";
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
            var identities = new List<ClaimsIdentity>();
            var identity = new ClaimsIdentity();
            identities.Add(identity);
            mockExpectedClaims.Setup(cp => cp.Identities).Returns(identities);

            _mockTokenService
                .Setup(ts => ts.IntrospectToken(expectedToken, It.IsAny<HttpClient>()))
                .Returns(Task.FromResult(expectedIntrospectResponse));

            var expectedSession = new MeUserSession
            {
                UserId = expectedUserId,
                OktaId = expectedOktaId,
                OktaToken = expectedOriginalToken,
                OktaTokenExpiry = DateTimeOffset.Now.AddDays(-1),
            };

            var expectedUser = new MeUser
            {
                UserId = expectedUserId,
                FirstName = "oktaFirstName",
                LastName = "oktaLastName",
                Email = "oktaEmail",
            };

            var expectedOktaUser = new Mock<IUser>(MockBehavior.Strict);
            expectedOktaUser.Setup(o => o.Profile.FirstName).Returns("oktaFirstName");
            expectedOktaUser.Setup(o => o.Profile.LastName).Returns("oktaLastName");
            expectedOktaUser.Setup(o => o.Profile.Email).Returns("oktaEmail");

            var mockUsersClient = new Mock<IUsersClient>(MockBehavior.Strict);

            mockUsersClient
                .Setup(service => service.GetUserAsync(It.IsAny<string>(), default(CancellationToken)))
                .Returns(Task.FromResult(expectedOktaUser.Object));

            _mockOktaClient
                .Setup(service => service.Users)
                .Returns(mockUsersClient.Object);

            _mockUserSessionRetrievalByOktaIdService
                .Setup(service => service.Handle(It.IsAny<UserSessionRetrievalByOktaIdQuery>()))
                .Returns(Task.FromResult(expectedSession))
                .Verifiable();

            _mockTokenValidator.Setup(stv =>
                    stv.ValidateToken(
                        expectedToken,
                        expectedTokenValidationParameters,
                        out expectedValidatedToken))
                .Returns(mockExpectedClaims.Object);

            _mockUserSessionUpdateOktaTokenService
                .Setup(service => service.Handle(It.IsAny<UserSessionUpdateOktaTokenQuery>()))
                .Returns((UserSessionUpdateOktaTokenQuery param) => Task.FromResult(new MeUserSession()
                {
                    UserId = param.UserId,
                    OktaId = expectedSession.OktaId,
                    OktaToken = param.OktaToken,
                    OktaTokenExpiry = param.OktaTokenExpiry,
                }))
                .Verifiable();

            _mockUserRetrievalById
                .Setup(service => service.Handle(It.IsAny<UserRetrievalByIdQuery>()))
                .Returns(Task.FromResult((MeUser)null));

            _mockUserUpdateService
                .Setup(service => service.Handle(It.IsAny<UserUpdateQuery>()))
                .Verifiable();

            // Act
            Action action = () => sut
                .ValidateToken(expectedToken, expectedTokenValidationParameters, out expectedValidatedToken)
                .Should().Be(mockExpectedClaims.Object);

            // Assert
            action.Should().Throw<Exception>();
        }

        [Fact]
        public void ValidateToken_ShouldReturnClaimsPrincipalAndNotUpdateUserInfo_WhenValidTokenChangedButUserInfoAlreadyUpToDate()
        {
            // Arrange
            var expectedUserId = "userId";
            var expectedOktaId = "oktaId";
            var expectedToken = "expectedToken";
            var expectedOriginalToken = "expectedOriginalToken";
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
            var identities = new List<ClaimsIdentity>();
            var identity = new ClaimsIdentity();
            identities.Add(identity);
            mockExpectedClaims.Setup(cp => cp.Identities).Returns(identities);

            _mockTokenService
                .Setup(ts => ts.IntrospectToken(expectedToken, It.IsAny<HttpClient>()))
                .Returns(Task.FromResult(expectedIntrospectResponse));

            var expectedSession = new MeUserSession
            {
                UserId = expectedUserId,
                OktaId = expectedOktaId,
                OktaToken = expectedOriginalToken,
                OktaTokenExpiry = DateTimeOffset.Now.AddDays(-1),
            };

            var expectedUser = new MeUser
            {
                UserId = expectedUserId,
                FirstName = "oktaFirstName",
                LastName = "oktaLastName",
                Email = "oktaEmail",
            };

            var expectedOktaUser = new Mock<IUser>(MockBehavior.Strict);
            expectedOktaUser.Setup(o => o.Profile.FirstName).Returns("oktaFirstName");
            expectedOktaUser.Setup(o => o.Profile.LastName).Returns("oktaLastName");
            expectedOktaUser.Setup(o => o.Profile.Email).Returns("oktaEmail");

            var mockUsersClient = new Mock<IUsersClient>(MockBehavior.Strict);

            mockUsersClient
                .Setup(service => service.GetUserAsync(It.IsAny<string>(), default(CancellationToken)))
                .Returns(Task.FromResult(expectedOktaUser.Object));

            _mockOktaClient
                .Setup(service => service.Users)
                .Returns(mockUsersClient.Object);

            _mockUserSessionRetrievalByOktaIdService
                .Setup(service => service.Handle(It.IsAny<UserSessionRetrievalByOktaIdQuery>()))
                .Returns(Task.FromResult(expectedSession))
                .Verifiable();

            _mockTokenValidator.Setup(stv =>
                    stv.ValidateToken(
                        expectedToken,
                        expectedTokenValidationParameters,
                        out expectedValidatedToken))
                .Returns(mockExpectedClaims.Object);

            _mockUserSessionUpdateOktaTokenService
                .Setup(service => service.Handle(It.IsAny<UserSessionUpdateOktaTokenQuery>()))
                .Returns((UserSessionUpdateOktaTokenQuery param) => Task.FromResult(new MeUserSession()
                {
                    UserId = param.UserId,
                    OktaId = expectedSession.OktaId,
                    OktaToken = param.OktaToken,
                    OktaTokenExpiry = param.OktaTokenExpiry,
                }))
                .Verifiable();

            _mockUserRetrievalById
                .Setup(service => service.Handle(It.IsAny<UserRetrievalByIdQuery>()))
                .Returns(Task.FromResult(expectedUser));

            _mockUserUpdateService
                .Setup(service => service.Handle(It.IsAny<UserUpdateQuery>()))
                .Verifiable();

            // Act
            sut.ValidateToken(expectedToken, expectedTokenValidationParameters, out expectedValidatedToken)
                .Should().Be(mockExpectedClaims.Object);

            // Assert
            _mockTokenService
                .Verify(ts => ts.IntrospectToken(expectedToken, It.IsAny<HttpClient>()));

            _mockUserSessionUpdateOktaTokenService
                .Verify(service => service.Handle(It.IsAny<UserSessionUpdateOktaTokenQuery>()));

            _mockUserUpdateService
                .Verify(service => service.Handle(It.IsAny<UserUpdateQuery>()), Times.Never);
        }

        [Fact]
        public void ValidateToken_ShouldReturnClaimsPrincipalAndCreateSession_WhenValidTokenAndNoSessionExists()
        {
            // Arrange
            var expectedUserId = "userId";
            var expectedOktaId = "oktaId";
            var expectedToken = "expectedToken";
            var expectedOriginalToken = "expectedOriginalToken";
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
            var identities = new List<ClaimsIdentity>();
            var identity = new ClaimsIdentity();
            identities.Add(identity);
            mockExpectedClaims.Setup(cp => cp.Identities).Returns(identities);

            _mockTokenService
                .Setup(ts => ts.IntrospectToken(expectedToken, It.IsAny<HttpClient>()))
                .Returns(Task.FromResult(expectedIntrospectResponse));

           /* var expectedSession = new MeUserSession
            {
                UserId = expectedUserId,
                OktaId = expectedOktaId,
                OktaToken = expectedOriginalToken,
                OktaTokenExpiry = DateTimeOffset.Now.AddDays(-1),
            };*/

            var expectedUser = new MeUser
            {
                UserId = expectedUserId,
                FirstName = "oktaFirstName",
                LastName = "oktaLastName",
                Email = "oktaEmail",
            };

            var expectedOktaUser = new Mock<IUser>(MockBehavior.Strict);
            expectedOktaUser.Setup(o => o.Profile.FirstName).Returns("oktaFirstName");
            expectedOktaUser.Setup(o => o.Profile.LastName).Returns("oktaLastName");
            expectedOktaUser.Setup(o => o.Profile.Email).Returns("oktaEmail");

            var mockUsersClient = new Mock<IUsersClient>(MockBehavior.Strict);

            mockUsersClient
                .Setup(service => service.GetUserAsync(It.IsAny<string>(), default(CancellationToken)))
                .Returns(Task.FromResult(expectedOktaUser.Object));

            _mockOktaClient
                .Setup(service => service.Users)
                .Returns(mockUsersClient.Object);

            _mockUserSessionRetrievalByOktaIdService
                .Setup(service => service.Handle(It.IsAny<UserSessionRetrievalByOktaIdQuery>()))
                .Returns(Task.FromResult((MeUserSession)null))
                .Verifiable();

            _mockUserRetrievalByOktaId
                .Setup(service => service.Handle(It.IsAny<UserRetrievalByOktaIdQuery>()))
                .Returns(Task.FromResult(expectedUser));

            _mockTokenValidator.Setup(stv =>
                    stv.ValidateToken(
                        expectedToken,
                        expectedTokenValidationParameters,
                        out expectedValidatedToken))
                .Returns(mockExpectedClaims.Object);

            _mockUserSessionUpdateOktaTokenService
                .Setup(service => service.Handle(It.IsAny<UserSessionUpdateOktaTokenQuery>()))
                .Returns((UserSessionUpdateOktaTokenQuery param) => Task.FromResult(new MeUserSession()
                {
                    UserId = param.UserId,
                    OktaId = expectedOktaId,
                    OktaToken = param.OktaToken,
                    OktaTokenExpiry = param.OktaTokenExpiry,
                }))
                .Verifiable();

            _mockUserRetrievalById
                .Setup(service => service.Handle(It.IsAny<UserRetrievalByIdQuery>()))
                .Returns(Task.FromResult(expectedUser));

            _mockUserUpdateService
                .Setup(service => service.Handle(It.IsAny<UserUpdateQuery>()))
                .Verifiable();

            // Act
            sut.ValidateToken(expectedToken, expectedTokenValidationParameters, out expectedValidatedToken)
                .Should().Be(mockExpectedClaims.Object);

            // Assert
            _mockTokenService
                .Verify(ts => ts.IntrospectToken(expectedToken, It.IsAny<HttpClient>()));

            _mockUserSessionUpdateOktaTokenService
                .Verify(service => service.Handle(It.IsAny<UserSessionUpdateOktaTokenQuery>()));

            _mockUserUpdateService
                .Verify(service => service.Handle(It.IsAny<UserUpdateQuery>()), Times.Never);
        }

        [Fact]
        public void ValidateToken_ShouldReturnClaimsPrincipalAndCreateUserAndSession_WhenValidTokenAndNoUserOrSessionExists_AndNoUserExistsWithSameEmail()
        {
            // Arrange
            var expectedUserId = "userId";
            var expectedOktaId = "oktaId";
            var expectedToken = "expectedToken";
            var expectedOriginalToken = "expectedOriginalToken";
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
            var identities = new List<ClaimsIdentity>();
            var identity = new ClaimsIdentity();
            identities.Add(identity);
            mockExpectedClaims.Setup(cp => cp.Identities).Returns(identities);

            _mockTokenService
                .Setup(ts => ts.IntrospectToken(expectedToken, It.IsAny<HttpClient>()))
                .Returns(Task.FromResult(expectedIntrospectResponse));

          /*  var expectedUser = new MeUser
            {
                UserId = expectedUserId,
                FirstName = "oktaFirstName",
                LastName = "oktaLastName",
                Email = "oktaEmail",
            };*/

            var expectedOktaUser = new Mock<IUser>(MockBehavior.Strict);
            expectedOktaUser.Setup(o => o.Id).Returns(expectedOktaId);
            expectedOktaUser.Setup(o => o.Profile.FirstName).Returns("oktaFirstName");
            expectedOktaUser.Setup(o => o.Profile.LastName).Returns("oktaLastName");
            expectedOktaUser.Setup(o => o.Profile.Email).Returns("oktaEmail");

            var mockUsersClient = new Mock<IUsersClient>(MockBehavior.Strict);

            mockUsersClient
                .Setup(service => service.GetUserAsync(It.IsAny<string>(), default(CancellationToken)))
                .Returns(Task.FromResult(expectedOktaUser.Object));

            _mockOktaClient
                .Setup(service => service.Users)
                .Returns(mockUsersClient.Object);

            _mockUserSessionRetrievalByOktaIdService
                .Setup(service => service.Handle(It.IsAny<UserSessionRetrievalByOktaIdQuery>()))
                .Returns(Task.FromResult((MeUserSession)null))
                .Verifiable();

            _mockUserRetrievalByOktaId
                .Setup(service => service.Handle(It.IsAny<UserRetrievalByOktaIdQuery>()))
                .Returns(Task.FromResult((MeUser)null));

            _mockUserRetrievalByEmailService
                .Setup(service => service.Handle(It.IsAny<UserRetrievalByEmailQuery>()))
                .Returns(Task.FromResult((MeUser)null));

            var expectedCreateUserResponse = new MeUser
            {
                UserId = "createUserId"
            };

            _mockUserCreationService
                .Setup(service => service.Handle(It.IsAny<CreateUserQuery>()) )
                .Returns((CreateUserQuery query) => Task.FromResult(expectedCreateUserResponse))
                .Verifiable();

            _mockTokenValidator.Setup(stv =>
                    stv.ValidateToken(
                        expectedToken,
                        expectedTokenValidationParameters,
                        out expectedValidatedToken))
                .Returns(mockExpectedClaims.Object);

            _mockUserSessionUpdateOktaTokenService
                .Setup(service => service.Handle(It.IsAny<UserSessionUpdateOktaTokenQuery>()))
                .Returns((UserSessionUpdateOktaTokenQuery param) => Task.FromResult(new MeUserSession()
                {
                    UserId = param.UserId,
                    OktaId = expectedOktaId,
                    OktaToken = param.OktaToken,
                    OktaTokenExpiry = param.OktaTokenExpiry,
                }))
                .Verifiable();
/*
            _mockUserRetrievalById
                .Setup(service => service.Handle(It.IsAny<UserRetrievalByIdQuery>()))
                .Returns(Task.FromResult(expectedUser));
*/
            _mockUserUpdateService
                .Setup(service => service.Handle(It.IsAny<UserUpdateQuery>()))
                .Verifiable();

            // Act
            sut.ValidateToken(expectedToken, expectedTokenValidationParameters, out expectedValidatedToken)
                .Should().Be(mockExpectedClaims.Object);

            // Assert
            _mockTokenService
                .Verify(ts => ts.IntrospectToken(expectedToken, It.IsAny<HttpClient>()));

            _mockUserSessionUpdateOktaTokenService
                .Verify(service => service.Handle(It.IsAny<UserSessionUpdateOktaTokenQuery>()));

            _mockUserUpdateService
                .Verify(service => service.Handle(It.IsAny<UserUpdateQuery>()), Times.Never);
        }
    }
}