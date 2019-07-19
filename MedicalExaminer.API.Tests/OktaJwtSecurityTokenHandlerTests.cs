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

        private readonly OktaJwtSecurityTokenHandler _sut;

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

            _sut = new OktaJwtSecurityTokenHandler(
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

            _sut.CanReadToken(It.IsAny<string>()).Should().BeTrue();
        }

        [Fact]
        public void CanValidateToken_ShouldReturnTrue()
        {
            _sut.CanValidateToken.Should().BeTrue();
        }

        [Fact]
        public void MaximumTokenSizeInBytesGet_ShouldReturnDefault()
        {
            _sut.MaximumTokenSizeInBytes.Should().Be(TokenValidationParameters.DefaultMaximumTokenSizeInBytes);
        }

        [Fact]
        public void MaximumTokenSizeInBytesSet_ShouldThrowException()
        {
            Action act = () => _sut.MaximumTokenSizeInBytes = 23;
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void ValidateToken_ShouldThrowException_WhenClaimsPrincipalNull()
        {
            var token = "expectedToken";
            var validatedToken = new Mock<SecurityToken>().Object;
            var tokenValidationParameters = new TokenValidationParameters();
            var introspectResponse = new IntrospectResponse
            {
                Active = false
            };

            _mockTokenService
                .Setup(ts => ts.IntrospectToken(token, It.IsAny<HttpClient>()))
                .Returns(Task.FromResult(introspectResponse));

            _mockTokenValidator.Setup(stv =>
                    stv.ValidateToken(
                        token,
                        tokenValidationParameters,
                        out validatedToken))
                .Returns((ClaimsPrincipal)null);

            Action action = () => _sut
                .ValidateToken(token, tokenValidationParameters, out validatedToken);

            action.Should().Throw<SecurityTokenValidationException>();

            validatedToken.Should().BeNull();
        }

        [Fact]
        public void ValidateToken_ShouldThrowException_WhenClaimsPrincipalMissingOktaId()
        {
            var token = "expectedToken";
            var validatedToken = new Mock<SecurityToken>().Object;
            var tokenValidationParameters = new TokenValidationParameters();
            var introspectResponse = new IntrospectResponse
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
                .Setup(ts => ts.IntrospectToken(token, It.IsAny<HttpClient>()))
                .Returns(Task.FromResult(introspectResponse));

            _mockTokenValidator.Setup(stv =>
                    stv.ValidateToken(
                        token,
                        tokenValidationParameters,
                        out validatedToken))
                .Returns(mockExpectedClaims.Object);

            Action action = () => _sut
                .ValidateToken(token, tokenValidationParameters, out validatedToken);

            action.Should().Throw<SecurityTokenValidationException>();

            validatedToken.Should().BeNull();
        }

        [Fact]
        public void ValidateToken_ShouldReturnClaimsPrincipal_WhenValidTokenPassedAndIntrospectResponseIsActive()
        {
            var userId = "userId";
            var oktaId = "oktaId";
            var token = "expectedToken";
            var validatedToken = new Mock<SecurityToken>().Object;
            var tokenValidationParameters = new TokenValidationParameters();
            var introspectResponse = new IntrospectResponse
            {
                Active = true
            };
            var mockExpectedClaims = new Mock<ClaimsPrincipal>();
            var claims = new List<Claim>();
            var claim = new Claim(MEClaimTypes.OktaUserId, oktaId);
            claims.Add(claim);
            mockExpectedClaims.Setup(cp => cp.Claims).Returns(claims);
            var identities = new List<ClaimsIdentity>();
            var identity = new ClaimsIdentity();
            identities.Add(identity);
            mockExpectedClaims.Setup(cp => cp.Identities).Returns(identities);

            _mockTokenService
                .Setup(ts => ts.IntrospectToken(token, It.IsAny<HttpClient>()))
                .Returns(Task.FromResult(introspectResponse));

            var session = new MeUserSession
            {
                UserId = userId,
                OktaId = oktaId,
                OktaToken = token,
                OktaTokenExpiry = DateTimeOffset.Now.AddDays(-1),
            };

            _mockUserSessionRetrievalByOktaIdService
                .Setup(service => service.Handle(It.IsAny<UserSessionRetrievalByOktaIdQuery>()))
                .Returns(Task.FromResult(session))
                .Verifiable();

            _mockTokenValidator.Setup(stv =>
                    stv.ValidateToken(
                        token,
                        tokenValidationParameters,
                        out validatedToken))
                .Returns(mockExpectedClaims.Object);

            _mockUserSessionUpdateOktaTokenService
                .Setup(service => service.Handle(It.IsAny<UserSessionUpdateOktaTokenQuery>()))
                .Returns((UserSessionUpdateOktaTokenQuery param) => Task.FromResult(new MeUserSession()
                {
                    UserId = param.UserId,
                    OktaId = session.OktaId,
                    OktaToken = param.OktaToken,
                    OktaTokenExpiry = param.OktaTokenExpiry,
                }))
                .Verifiable();

            _sut.ValidateToken(token, tokenValidationParameters, out validatedToken)
                .Should().Be(mockExpectedClaims.Object);

            _mockTokenService
                .Verify(ts => ts.IntrospectToken(token, It.IsAny<HttpClient>()));

            _mockUserSessionUpdateOktaTokenService
                .Verify(service => service.Handle(It.IsAny<UserSessionUpdateOktaTokenQuery>()));
        }

        [Fact]
        public void ValidateToken_ShouldThrowException_WhenIntrospectResponseIsNotActive()
        {
            var oktaId = "oktaId";
            var token = "expectedToken";
            var validatedToken = new Mock<SecurityToken>().Object;
            var tokenValidationParameters = new TokenValidationParameters();
            var introspectResponse = new IntrospectResponse
            {
                Active = false
            };
            var mockExpectedClaims = new Mock<ClaimsPrincipal>();
            var claims = new List<Claim>();
            var claim = new Claim(MEClaimTypes.OktaUserId, oktaId);
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
                .Setup(ts => ts.IntrospectToken(token, It.IsAny<HttpClient>()))
                .Returns(Task.FromResult(introspectResponse));

            _mockTokenValidator.Setup(stv =>
                    stv.ValidateToken(
                        token,
                        tokenValidationParameters,
                        out validatedToken))
                .Returns(mockExpectedClaims.Object);

            Action action = () => _sut
                .ValidateToken(token, tokenValidationParameters, out validatedToken)
                .Should().Be(mockExpectedClaims.Object);

            action.Should().Throw<SecurityTokenValidationException>();

            validatedToken.Should().BeNull();
        }

        [Fact]
        public void ValidateToken_ShouldReturnClaimsPrincipalAndUpdateUserInfo_WhenValidTokenChanged()
        {
            // Arrange
            var userId = "userId";
            var oktaId = "oktaId";
            var token = "expectedToken";
            var originalToken = "expectedOriginalToken";
            var validatedToken = new Mock<SecurityToken>().Object;
            var tokenValidationParameters = new TokenValidationParameters();
            var introspectResponse = new IntrospectResponse
            {
                Active = true
            };
            var mockExpectedClaims = new Mock<ClaimsPrincipal>();
            var claims = new List<Claim>();
            var claim = new Claim(MEClaimTypes.OktaUserId, oktaId);
            claims.Add(claim);
            mockExpectedClaims.Setup(cp => cp.Claims).Returns(claims);
            var identities = new List<ClaimsIdentity>();
            var identity = new ClaimsIdentity();
            identities.Add(identity);
            mockExpectedClaims.Setup(cp => cp.Identities).Returns(identities);

            _mockTokenService
                .Setup(ts => ts.IntrospectToken(token, It.IsAny<HttpClient>()))
                .Returns(Task.FromResult(introspectResponse));

            var session = new MeUserSession
            {
                UserId = userId,
                OktaId = oktaId,
                OktaToken = originalToken,
                OktaTokenExpiry = DateTimeOffset.Now.AddDays(-1),
            };

            var user = new MeUser
            {
                UserId = userId,
                FirstName = "currentFirstName",
                LastName = "currentLastName",
                Email = "currentEmail",
            };

            var oktaUser = new Mock<IUser>(MockBehavior.Strict);
            oktaUser.Setup(o => o.Profile.FirstName).Returns("oktaFirstName");
            oktaUser.Setup(o => o.Profile.LastName).Returns("oktaLastName");
            oktaUser.Setup(o => o.Profile.Email).Returns("oktaEmail");

            var mockUsersClient = new Mock<IUsersClient>(MockBehavior.Strict);

            mockUsersClient
                .Setup(service => service.GetUserAsync(It.IsAny<string>(), default(CancellationToken)))
                .Returns(Task.FromResult(oktaUser.Object));

            _mockOktaClient
                .Setup(service => service.Users)
                .Returns(mockUsersClient.Object);

            _mockUserSessionRetrievalByOktaIdService
                .Setup(service => service.Handle(It.IsAny<UserSessionRetrievalByOktaIdQuery>()))
                .Returns(Task.FromResult(session))
                .Verifiable();

            _mockTokenValidator.Setup(stv =>
                    stv.ValidateToken(
                        token,
                        tokenValidationParameters,
                        out validatedToken))
                .Returns(mockExpectedClaims.Object);

            _mockUserSessionUpdateOktaTokenService
                .Setup(service => service.Handle(It.IsAny<UserSessionUpdateOktaTokenQuery>()))
                .Returns((UserSessionUpdateOktaTokenQuery param) => Task.FromResult(new MeUserSession()
                {
                    UserId = param.UserId,
                    OktaId = session.OktaId,
                    OktaToken = param.OktaToken,
                    OktaTokenExpiry = param.OktaTokenExpiry,
                }))
                .Verifiable();

            _mockUserRetrievalById
                .Setup(service => service.Handle(It.IsAny<UserRetrievalByIdQuery>()))
                .Returns(Task.FromResult(user));

            _mockUserUpdateService
                .Setup(service => service.Handle(It.IsAny<UserUpdateQuery>()))
                .Returns((UserUpdateQuery query) => Task.FromResult(new MeUser
                {
                    UserId = query.UserUpdate.UserId,
                }))
                .Verifiable();

            // Act
            _sut.ValidateToken(token, tokenValidationParameters, out validatedToken)
                .Should().Be(mockExpectedClaims.Object);

            // Assert
            _mockTokenService
                .Verify(ts => ts.IntrospectToken(token, It.IsAny<HttpClient>()));

            _mockUserSessionUpdateOktaTokenService
                .Verify(service => service.Handle(It.IsAny<UserSessionUpdateOktaTokenQuery>()));

            _mockUserUpdateService
                .Verify(service => service.Handle(It.IsAny<UserUpdateQuery>()));
        }

        [Fact]
        public void ValidateToken_ShouldThrowException_WhenValidTokenChangedButUserDoesNotExist()
        {
            // Arrange
            var userId = "userId";
            var oktaId = "oktaId";
            var token = "expectedToken";
            var originalToken = "expectedOriginalToken";
            var validatedToken = new Mock<SecurityToken>().Object;
            var tokenValidationParameters = new TokenValidationParameters();
            var introspectResponse = new IntrospectResponse
            {
                Active = true
            };
            var mockExpectedClaims = new Mock<ClaimsPrincipal>();
            var claims = new List<Claim>();
            var claim = new Claim(MEClaimTypes.OktaUserId, oktaId);
            claims.Add(claim);
            mockExpectedClaims.Setup(cp => cp.Claims).Returns(claims);
            var identities = new List<ClaimsIdentity>();
            var identity = new ClaimsIdentity();
            identities.Add(identity);
            mockExpectedClaims.Setup(cp => cp.Identities).Returns(identities);

            _mockTokenService
                .Setup(ts => ts.IntrospectToken(token, It.IsAny<HttpClient>()))
                .Returns(Task.FromResult(introspectResponse));

            var session = new MeUserSession
            {
                UserId = userId,
                OktaId = oktaId,
                OktaToken = originalToken,
                OktaTokenExpiry = DateTimeOffset.Now.AddDays(-1),
            };

            var oktaUser = new Mock<IUser>(MockBehavior.Strict);
            oktaUser.Setup(o => o.Profile.FirstName).Returns("oktaFirstName");
            oktaUser.Setup(o => o.Profile.LastName).Returns("oktaLastName");
            oktaUser.Setup(o => o.Profile.Email).Returns("oktaEmail");

            var mockUsersClient = new Mock<IUsersClient>(MockBehavior.Strict);

            mockUsersClient
                .Setup(service => service.GetUserAsync(It.IsAny<string>(), default(CancellationToken)))
                .Returns(Task.FromResult(oktaUser.Object));

            _mockOktaClient
                .Setup(service => service.Users)
                .Returns(mockUsersClient.Object);

            _mockUserSessionRetrievalByOktaIdService
                .Setup(service => service.Handle(It.IsAny<UserSessionRetrievalByOktaIdQuery>()))
                .Returns(Task.FromResult(session))
                .Verifiable();

            _mockTokenValidator.Setup(stv =>
                    stv.ValidateToken(
                        token,
                        tokenValidationParameters,
                        out validatedToken))
                .Returns(mockExpectedClaims.Object);

            _mockUserSessionUpdateOktaTokenService
                .Setup(service => service.Handle(It.IsAny<UserSessionUpdateOktaTokenQuery>()))
                .Returns((UserSessionUpdateOktaTokenQuery param) => Task.FromResult(new MeUserSession()
                {
                    UserId = param.UserId,
                    OktaId = session.OktaId,
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
            Action action = () => _sut
                .ValidateToken(token, tokenValidationParameters, out validatedToken)
                .Should().Be(mockExpectedClaims.Object);

            // Assert
            action.Should().Throw<Exception>();
        }

        [Fact]
        public void ValidateToken_ShouldReturnClaimsPrincipalAndNotUpdateUserInfo_WhenValidTokenChangedButUserInfoAlreadyUpToDate()
        {
            // Arrange
            var userId = "userId";
            var oktaId = "oktaId";
            var token = "expectedToken";
            var originalToken = "expectedOriginalToken";
            var validatedToken = new Mock<SecurityToken>().Object;
            var tokenValidationParameters = new TokenValidationParameters();
            var introspectResponse = new IntrospectResponse
            {
                Active = true
            };
            var mockExpectedClaims = new Mock<ClaimsPrincipal>();
            var claims = new List<Claim>();
            var claim = new Claim(MEClaimTypes.OktaUserId, oktaId);
            claims.Add(claim);
            mockExpectedClaims.Setup(cp => cp.Claims).Returns(claims);
            var identities = new List<ClaimsIdentity>();
            var identity = new ClaimsIdentity();
            identities.Add(identity);
            mockExpectedClaims.Setup(cp => cp.Identities).Returns(identities);

            _mockTokenService
                .Setup(ts => ts.IntrospectToken(token, It.IsAny<HttpClient>()))
                .Returns(Task.FromResult(introspectResponse));

            var session = new MeUserSession
            {
                UserId = userId,
                OktaId = oktaId,
                OktaToken = originalToken,
                OktaTokenExpiry = DateTimeOffset.Now.AddDays(-1),
            };

            var user = new MeUser
            {
                UserId = userId,
                FirstName = "oktaFirstName",
                LastName = "oktaLastName",
                Email = "oktaEmail",
            };

            var oktaUser = new Mock<IUser>(MockBehavior.Strict);
            oktaUser.Setup(o => o.Profile.FirstName).Returns("oktaFirstName");
            oktaUser.Setup(o => o.Profile.LastName).Returns("oktaLastName");
            oktaUser.Setup(o => o.Profile.Email).Returns("oktaEmail");

            var mockUsersClient = new Mock<IUsersClient>(MockBehavior.Strict);

            mockUsersClient
                .Setup(service => service.GetUserAsync(It.IsAny<string>(), default(CancellationToken)))
                .Returns(Task.FromResult(oktaUser.Object));

            _mockOktaClient
                .Setup(service => service.Users)
                .Returns(mockUsersClient.Object);

            _mockUserSessionRetrievalByOktaIdService
                .Setup(service => service.Handle(It.IsAny<UserSessionRetrievalByOktaIdQuery>()))
                .Returns(Task.FromResult(session))
                .Verifiable();

            _mockTokenValidator.Setup(stv =>
                    stv.ValidateToken(
                        token,
                        tokenValidationParameters,
                        out validatedToken))
                .Returns(mockExpectedClaims.Object);

            _mockUserSessionUpdateOktaTokenService
                .Setup(service => service.Handle(It.IsAny<UserSessionUpdateOktaTokenQuery>()))
                .Returns((UserSessionUpdateOktaTokenQuery param) => Task.FromResult(new MeUserSession()
                {
                    UserId = param.UserId,
                    OktaId = session.OktaId,
                    OktaToken = param.OktaToken,
                    OktaTokenExpiry = param.OktaTokenExpiry,
                }))
                .Verifiable();

            _mockUserRetrievalById
                .Setup(service => service.Handle(It.IsAny<UserRetrievalByIdQuery>()))
                .Returns(Task.FromResult(user));

            _mockUserUpdateService
                .Setup(service => service.Handle(It.IsAny<UserUpdateQuery>()))
                .Verifiable();

            // Act
            _sut.ValidateToken(token, tokenValidationParameters, out validatedToken)
                .Should().Be(mockExpectedClaims.Object);

            // Assert
            _mockTokenService
                .Verify(ts => ts.IntrospectToken(token, It.IsAny<HttpClient>()));

            _mockUserSessionUpdateOktaTokenService
                .Verify(service => service.Handle(It.IsAny<UserSessionUpdateOktaTokenQuery>()));

            _mockUserUpdateService
                .Verify(service => service.Handle(It.IsAny<UserUpdateQuery>()), Times.Never);
        }

        [Fact]
        public void ValidateToken_ShouldReturnClaimsPrincipalAndCreateSession_WhenValidTokenAndNoSessionExists()
        {
            // Arrange
            var userId = "userId";
            var oktaId = "oktaId";
            var token = "expectedToken";
            var validatedToken = new Mock<SecurityToken>().Object;
            var tokenValidationParameters = new TokenValidationParameters();
            var introspectResponse = new IntrospectResponse
            {
                Active = true
            };
            var mockExpectedClaims = new Mock<ClaimsPrincipal>();
            var claims = new List<Claim>();
            var claim = new Claim(MEClaimTypes.OktaUserId, oktaId);
            claims.Add(claim);
            mockExpectedClaims.Setup(cp => cp.Claims).Returns(claims);
            var identities = new List<ClaimsIdentity>();
            var identity = new ClaimsIdentity();
            identities.Add(identity);
            mockExpectedClaims.Setup(cp => cp.Identities).Returns(identities);

            _mockTokenService
                .Setup(ts => ts.IntrospectToken(token, It.IsAny<HttpClient>()))
                .Returns(Task.FromResult(introspectResponse));

            var user = new MeUser
            {
                UserId = userId,
                FirstName = "oktaFirstName",
                LastName = "oktaLastName",
                Email = "oktaEmail",
            };

            var oktaUser = new Mock<IUser>(MockBehavior.Strict);
            oktaUser.Setup(o => o.Profile.FirstName).Returns("oktaFirstName");
            oktaUser.Setup(o => o.Profile.LastName).Returns("oktaLastName");
            oktaUser.Setup(o => o.Profile.Email).Returns("oktaEmail");

            var mockUsersClient = new Mock<IUsersClient>(MockBehavior.Strict);

            mockUsersClient
                .Setup(service => service.GetUserAsync(It.IsAny<string>(), default(CancellationToken)))
                .Returns(Task.FromResult(oktaUser.Object));

            _mockOktaClient
                .Setup(service => service.Users)
                .Returns(mockUsersClient.Object);

            _mockUserSessionRetrievalByOktaIdService
                .Setup(service => service.Handle(It.IsAny<UserSessionRetrievalByOktaIdQuery>()))
                .Returns(Task.FromResult((MeUserSession)null))
                .Verifiable();

            _mockUserRetrievalByOktaId
                .Setup(service => service.Handle(It.IsAny<UserRetrievalByOktaIdQuery>()))
                .Returns(Task.FromResult(user));

            _mockTokenValidator.Setup(stv =>
                    stv.ValidateToken(
                        token,
                        tokenValidationParameters,
                        out validatedToken))
                .Returns(mockExpectedClaims.Object);

            _mockUserSessionUpdateOktaTokenService
                .Setup(service => service.Handle(It.IsAny<UserSessionUpdateOktaTokenQuery>()))
                .Returns((UserSessionUpdateOktaTokenQuery param) => Task.FromResult(new MeUserSession()
                {
                    UserId = param.UserId,
                    OktaId = oktaId,
                    OktaToken = param.OktaToken,
                    OktaTokenExpiry = param.OktaTokenExpiry,
                }))
                .Verifiable();

            _mockUserRetrievalById
                .Setup(service => service.Handle(It.IsAny<UserRetrievalByIdQuery>()))
                .Returns(Task.FromResult(user));

            _mockUserUpdateService
                .Setup(service => service.Handle(It.IsAny<UserUpdateQuery>()))
                .Verifiable();

            // Act
            _sut.ValidateToken(token, tokenValidationParameters, out validatedToken)
                .Should().Be(mockExpectedClaims.Object);

            // Assert
            _mockTokenService
                .Verify(ts => ts.IntrospectToken(token, It.IsAny<HttpClient>()));

            _mockUserSessionUpdateOktaTokenService
                .Verify(service => service.Handle(It.IsAny<UserSessionUpdateOktaTokenQuery>()));

            _mockUserUpdateService
                .Verify(service => service.Handle(It.IsAny<UserUpdateQuery>()), Times.Never);
        }

        [Fact]
        public void ValidateToken_ShouldReturnClaimsPrincipalAndCreateUserAndSession_WhenValidTokenAndNoUserOrSessionExists_AndNoUserExistsWithSameEmail()
        {
            // Arrange
            var oktaId = "oktaId";
            var token = "expectedToken";
            var validatedToken = new Mock<SecurityToken>().Object;
            var tokenValidationParameters = new TokenValidationParameters();
            var introspectResponse = new IntrospectResponse
            {
                Active = true
            };
            var mockExpectedClaims = new Mock<ClaimsPrincipal>();
            var claims = new List<Claim>();
            var claim = new Claim(MEClaimTypes.OktaUserId, oktaId);
            claims.Add(claim);
            mockExpectedClaims.Setup(cp => cp.Claims).Returns(claims);
            var identities = new List<ClaimsIdentity>();
            var identity = new ClaimsIdentity();
            identities.Add(identity);
            mockExpectedClaims.Setup(cp => cp.Identities).Returns(identities);

            _mockTokenService
                .Setup(ts => ts.IntrospectToken(token, It.IsAny<HttpClient>()))
                .Returns(Task.FromResult(introspectResponse));

            var oktaUser = new Mock<IUser>(MockBehavior.Strict);
            oktaUser.Setup(o => o.Id).Returns(oktaId);
            oktaUser.Setup(o => o.Profile.FirstName).Returns("oktaFirstName");
            oktaUser.Setup(o => o.Profile.LastName).Returns("oktaLastName");
            oktaUser.Setup(o => o.Profile.Email).Returns("oktaEmail");

            var mockUsersClient = new Mock<IUsersClient>(MockBehavior.Strict);

            mockUsersClient
                .Setup(service => service.GetUserAsync(It.IsAny<string>(), default(CancellationToken)))
                .Returns(Task.FromResult(oktaUser.Object));

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

            var createUserResponse = new MeUser
            {
                UserId = "createUserId"
            };

            _mockUserCreationService
                .Setup(service => service.Handle(It.IsAny<CreateUserQuery>()) )
                .Returns((CreateUserQuery query) => Task.FromResult(createUserResponse))
                .Verifiable();

            _mockTokenValidator.Setup(stv =>
                    stv.ValidateToken(
                        token,
                        tokenValidationParameters,
                        out validatedToken))
                .Returns(mockExpectedClaims.Object);

            _mockUserSessionUpdateOktaTokenService
                .Setup(service => service.Handle(It.IsAny<UserSessionUpdateOktaTokenQuery>()))
                .Returns((UserSessionUpdateOktaTokenQuery param) => Task.FromResult(new MeUserSession()
                {
                    UserId = param.UserId,
                    OktaId = oktaId,
                    OktaToken = param.OktaToken,
                    OktaTokenExpiry = param.OktaTokenExpiry,
                }))
                .Verifiable();

            _mockUserUpdateService
                .Setup(service => service.Handle(It.IsAny<UserUpdateQuery>()))
                .Verifiable();

            // Act
            _sut.ValidateToken(token, tokenValidationParameters, out validatedToken)
                .Should().Be(mockExpectedClaims.Object);

            // Assert
            _mockTokenService
                .Verify(ts => ts.IntrospectToken(token, It.IsAny<HttpClient>()));

            _mockUserSessionUpdateOktaTokenService
                .Verify(service => service.Handle(It.IsAny<UserSessionUpdateOktaTokenQuery>()));

            _mockUserUpdateService
                .Verify(service => service.Handle(It.IsAny<UserUpdateQuery>()), Times.Never);
        }

        [Fact]
        public void ValidateToken_ShouldReturnClaimsPrincipalAndBindUserAndSession_WhenValidTokenAndNoUserOrSessionExists_AndUserExistsWithSameEmail()
        {
            // Arrange
            var userId = "userId";
            var oktaId = "oktaId";
            var token = "expectedToken";
            var validatedToken = new Mock<SecurityToken>().Object;
            var tokenValidationParameters = new TokenValidationParameters();
            var introspectResponse = new IntrospectResponse
            {
                Active = true
            };
            var mockExpectedClaims = new Mock<ClaimsPrincipal>();
            var claims = new List<Claim>();
            var claim = new Claim(MEClaimTypes.OktaUserId, oktaId);
            claims.Add(claim);
            mockExpectedClaims.Setup(cp => cp.Claims).Returns(claims);
            var identities = new List<ClaimsIdentity>();
            var identity = new ClaimsIdentity();
            identities.Add(identity);
            mockExpectedClaims.Setup(cp => cp.Identities).Returns(identities);

            _mockTokenService
                .Setup(ts => ts.IntrospectToken(token, It.IsAny<HttpClient>()))
                .Returns(Task.FromResult(introspectResponse));

            var user = new MeUser
            {
                UserId = userId,
                Email = "oktaEmail",
            };

            var oktaUser = new Mock<IUser>(MockBehavior.Strict);
            oktaUser.Setup(o => o.Id).Returns(oktaId);
            oktaUser.Setup(o => o.Profile.FirstName).Returns("oktaFirstName");
            oktaUser.Setup(o => o.Profile.LastName).Returns("oktaLastName");
            oktaUser.Setup(o => o.Profile.Email).Returns("oktaEmail");

            var mockUsersClient = new Mock<IUsersClient>(MockBehavior.Strict);

            mockUsersClient
                .Setup(service => service.GetUserAsync(It.IsAny<string>(), default(CancellationToken)))
                .Returns(Task.FromResult(oktaUser.Object));

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
                .Returns(Task.FromResult(user));

            var createUserResponse = new MeUser
            {
                UserId = "createUserId"
            };

            _mockUserCreationService
                .Setup(service => service.Handle(It.IsAny<CreateUserQuery>()))
                .Returns((CreateUserQuery query) => Task.FromResult(createUserResponse))
                .Verifiable();

            _mockTokenValidator.Setup(stv =>
                    stv.ValidateToken(
                        token,
                        tokenValidationParameters,
                        out validatedToken))
                .Returns(mockExpectedClaims.Object);

            _mockUserSessionUpdateOktaTokenService
                .Setup(service => service.Handle(It.IsAny<UserSessionUpdateOktaTokenQuery>()))
                .Returns((UserSessionUpdateOktaTokenQuery param) => Task.FromResult(new MeUserSession()
                {
                    UserId = param.UserId,
                    OktaId = oktaId,
                    OktaToken = param.OktaToken,
                    OktaTokenExpiry = param.OktaTokenExpiry,
                }))
                .Verifiable();

            _mockUserUpdateService
                .Setup(service => service.Handle(It.IsAny<UserUpdateQuery>()))
                .Returns((UserUpdateQuery query) =>
                {
                    var update = query.UserUpdate as UserUpdateFull;
                    Assert.NotNull(update);
                    return Task.FromResult(new MeUser
                    {
                        UserId = query.UserUpdate.UserId,
                        OktaId = update.OktaId,
                        Email = update.Email,
                        FirstName = update.FirstName,
                        LastName = update.LastName,
                    });
                })
                .Verifiable();

            // Act
            _sut.ValidateToken(token, tokenValidationParameters, out validatedToken)
                .Should().Be(mockExpectedClaims.Object);

            // Assert
            _mockTokenService
                .Verify(ts => ts.IntrospectToken(token, It.IsAny<HttpClient>()));

            _mockUserSessionUpdateOktaTokenService
                .Verify(service => service.Handle(It.IsAny<UserSessionUpdateOktaTokenQuery>()));

            _mockUserUpdateService
                .Verify(service => service.Handle(It.IsAny<UserUpdateQuery>()), Times.Once);
        }

        [Fact]
        public void ValidateToken_ShouldThrowException_WhenValidTokenAndNoUserOrSessionExists_AndUserExistsWithSameEmail_ButEmailIsAlreadyBound()
        {
            // Arrange
            var userId = "userId";
            var oktaId = "oktaId";
            var token = "expectedToken";
            var validatedToken = new Mock<SecurityToken>().Object;
            var tokenValidationParameters = new TokenValidationParameters();
            var introspectResponse = new IntrospectResponse
            {
                Active = true
            };
            var mockExpectedClaims = new Mock<ClaimsPrincipal>();
            var claims = new List<Claim>();
            var claim = new Claim(MEClaimTypes.OktaUserId, oktaId);
            claims.Add(claim);
            mockExpectedClaims.Setup(cp => cp.Claims).Returns(claims);
            var identities = new List<ClaimsIdentity>();
            var identity = new ClaimsIdentity();
            identities.Add(identity);
            mockExpectedClaims.Setup(cp => cp.Identities).Returns(identities);

            _mockTokenService
                .Setup(ts => ts.IntrospectToken(token, It.IsAny<HttpClient>()))
                .Returns(Task.FromResult(introspectResponse));

            var user = new MeUser
            {
                UserId = userId,
                OktaId = oktaId,
                Email = "oktaEmail",
            };

            var oktaUser = new Mock<IUser>(MockBehavior.Strict);
            oktaUser.Setup(o => o.Id).Returns(oktaId);
            oktaUser.Setup(o => o.Profile.FirstName).Returns("oktaFirstName");
            oktaUser.Setup(o => o.Profile.LastName).Returns("oktaLastName");
            oktaUser.Setup(o => o.Profile.Email).Returns("oktaEmail");

            var mockUsersClient = new Mock<IUsersClient>(MockBehavior.Strict);

            mockUsersClient
                .Setup(service => service.GetUserAsync(It.IsAny<string>(), default(CancellationToken)))
                .Returns(Task.FromResult(oktaUser.Object));

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
                .Returns(Task.FromResult(user));

            var createUserResponse = new MeUser
            {
                UserId = "createUserId"
            };

            _mockUserCreationService
                .Setup(service => service.Handle(It.IsAny<CreateUserQuery>()))
                .Returns((CreateUserQuery query) => Task.FromResult(createUserResponse))
                .Verifiable();

            _mockTokenValidator.Setup(stv =>
                    stv.ValidateToken(
                        token,
                        tokenValidationParameters,
                        out validatedToken))
                .Returns(mockExpectedClaims.Object);

            _mockUserSessionUpdateOktaTokenService
                .Setup(service => service.Handle(It.IsAny<UserSessionUpdateOktaTokenQuery>()))
                .Returns((UserSessionUpdateOktaTokenQuery param) => Task.FromResult(new MeUserSession()
                {
                    UserId = param.UserId,
                    OktaId = oktaId,
                    OktaToken = param.OktaToken,
                    OktaTokenExpiry = param.OktaTokenExpiry,
                }))
                .Verifiable();

            // Act
            Action action = () => _sut
                .ValidateToken(token, tokenValidationParameters, out validatedToken)
                .Should().Be(mockExpectedClaims.Object);

            // Assert
            action.Should().Throw<Exception>();
        }
    }
}