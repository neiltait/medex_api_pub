using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using MedicalExaminer.API.Authorization;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.Azure.Documents;
using Microsoft.IdentityModel.Tokens;
using Okta.Sdk;

namespace MedicalExaminer.API
{
    /// <summary>
    ///     Okta JWT Security Token Handler
    /// </summary>
    /// <inheritdoc />
    public class OktaJwtSecurityTokenHandler : ISecurityTokenValidator
    {
        /// <summary>
        ///     Standard handler.
        /// </summary>
        private readonly ISecurityTokenValidator _tokenHandler;

        /// <summary>
        ///     Token service.
        /// </summary>
        private readonly ITokenService _tokenService;

        /// <summary>
        /// User Session Retrieval By Okta Id Service
        /// </summary>
        private readonly IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser> _userRetrievalById;

        /// <summary>
        /// User Session Retrieval By Okta Id Service
        /// </summary>
        private readonly IAsyncQueryHandler<UserSessionRetrievalByOktaIdQuery, MeUserSession>
            _userSessionRetrievalByOktaIdService;

        /// <summary>
        /// Token Update Service
        /// </summary>
        private readonly IAsyncQueryHandler<UserSessionUpdateOktaTokenQuery, MeUserSession>
            _userSessionUpdateOktaTokenService;

        /// <summary>
        /// Token Retrieval Service
        /// </summary>
        private readonly IAsyncQueryHandler<UserSessionRetrievalByOktaTokenQuery, MeUserSession>
            _userSessionRetrievalByOktaTokenService;

        /// <summary>
        /// User Update Service.
        /// </summary>
        private readonly IAsyncQueryHandler<UserUpdateQuery, MeUser> _userUpdateService;

        private readonly IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> _userRetrievalByEmailService;

        private readonly IAsyncQueryHandler<UserUpdateOktaQuery, MeUser> _userUpdateOktaService;

        private readonly IAsyncQueryHandler<CreateUserQuery, MeUser> _userCreationService;

        private readonly IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser> _userRetrievalByOktaId;

        /// <summary>
        ///     Okta Client.
        /// </summary>
        private readonly OktaClient _oktaClient;

        /// <summary>
        /// Time in minutes before Okta token expires and has to be resubmitted to Okta
        /// </summary>
        private readonly int _oktaTokenExpiryTime;

        /// <summary>
        ///     Initialise a new instance of the Okta JWT Security Token Handler
        /// </summary>
        /// <param name="tokenService">The Token Service.</param>
        /// <param name="tokenValidator">Token validator.</param>
        /// <param name="userSessionUpdateOktaTokenService">User update okta token service.</param>
        /// <param name="userSessionRetrievalByOktaTokenService">User retrieval okta token service.</param>
        /// <param name="userSessionRetrievalByOktaIdService">Users retrieval by okta id service.</param>
        /// <param name="userRetrievalById">User retrieval by id service.</param>
        /// <param name="userUpdateService">Update user service.</param>
        /// <param name="userRetrievalByEmailService"></param>
        /// <param name="userUpdateOktaService"></param>
        /// <param name="userCreationService"></param>
        /// <param name="userRetrievalByOktaId"></param>
        /// <param name="oktaClient">Okta client.</param>
        /// <param name="oktaTokenExpiryTime">Okta token expiry time.</param>
        public OktaJwtSecurityTokenHandler(
            ITokenService tokenService,
            ISecurityTokenValidator tokenValidator,
            IAsyncQueryHandler<UserSessionUpdateOktaTokenQuery, MeUserSession> userSessionUpdateOktaTokenService,
            IAsyncQueryHandler<UserSessionRetrievalByOktaTokenQuery, MeUserSession> userSessionRetrievalByOktaTokenService,
            IAsyncQueryHandler<UserSessionRetrievalByOktaIdQuery, MeUserSession> userSessionRetrievalByOktaIdService,
            IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser> userRetrievalById,
            IAsyncQueryHandler<UserUpdateQuery, MeUser> userUpdateService,
            IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> userRetrievalByEmailService,
            IAsyncQueryHandler<UserUpdateOktaQuery, MeUser> userUpdateOktaService,
            IAsyncQueryHandler<CreateUserQuery, MeUser> userCreationService,
            IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser> userRetrievalByOktaId,
            OktaClient oktaClient,
            int oktaTokenExpiryTime)
        {
            _tokenHandler = tokenValidator;
            _tokenService = tokenService;
            _userSessionUpdateOktaTokenService = userSessionUpdateOktaTokenService;
            _userSessionRetrievalByOktaTokenService = userSessionRetrievalByOktaTokenService;
            _userSessionRetrievalByOktaIdService = userSessionRetrievalByOktaIdService;
            _oktaTokenExpiryTime = oktaTokenExpiryTime;
            _userRetrievalByEmailService = userRetrievalByEmailService;
            _userUpdateOktaService = userUpdateOktaService;
            _userCreationService = userCreationService;
            _userRetrievalByOktaId = userRetrievalByOktaId;
            _userRetrievalById = userRetrievalById;
            _userUpdateService = userUpdateService;
            _oktaClient = oktaClient;
        }

        /// <inheritdoc />
        public bool CanValidateToken => true;

        /// <inheritdoc />
        public int MaximumTokenSizeInBytes
        {
            get => TokenValidationParameters.DefaultMaximumTokenSizeInBytes;
            set => throw new InvalidOperationException();
        }

        /// <inheritdoc />
        public bool CanReadToken(string securityToken)
        {
            return _tokenHandler.CanReadToken(securityToken);
        }

        /// <inheritdoc />
        public ClaimsPrincipal ValidateToken(
            string securityToken,
            TokenValidationParameters validationParameters,
            out SecurityToken validatedToken)
        {
            // Attempt to retrieve user session based on token. If it can be retrieved and token not expired then can bypass call to Okta
            var userSessionTask =
                _userSessionRetrievalByOktaTokenService.Handle(new UserSessionRetrievalByOktaTokenQuery(securityToken));

            var userSession = userSessionTask?.Result;

            // Cannot find user by token or token has expired so go to Okta
            if (userSession == null || userSession.OktaTokenExpiry < DateTimeOffset.Now)
            {
                var introspectTokenResponse = _tokenService.IntrospectToken(securityToken, new HttpClient()).Result;

                if (!introspectTokenResponse.Active)
                {
                    validatedToken = null;
                    throw new SecurityTokenValidationException();
                }
            }

            // Use _tokenHandler to deserialize Okta onto claimsPrincipal. Note that the token carries an expiry internally
            // If it has expired then we would expect SecurityTokenExpiredException to be thrown and authentication fails
            var claimsPrincipal = _tokenHandler.ValidateToken(securityToken, validationParameters, out validatedToken);

            // Update user record with okta settings if not found originally or if token had expired
            if ((userSession == null || userSession.OktaTokenExpiry < DateTimeOffset.Now) &&
                claimsPrincipal != null)
            {
                userSession = UpdateOktaTokenForUser(securityToken, claimsPrincipal, userSession);
            }

            if (userSession != null)
            {
                claimsPrincipal?.Identities.First().AddClaim(
                    new Claim(
                        MEClaimTypes.UserId,
                        userSession.UserId));
            }

            return claimsPrincipal;
        }

        /// <summary>
        /// Update user's Okta token.
        /// </summary>
        /// <param name="oktaToken">Okta token</param>
        /// <param name="claimsPrincipal">Claims principal.</param>
        /// <remarks>Attempts to find user DB record based on email address. If user is found then update oktea token</remarks>
        /// <returns>User if found by email.</returns>
        private MeUserSession UpdateOktaTokenForUser(string oktaToken, ClaimsPrincipal claimsPrincipal, MeUserSession meUserSession)
        {
            var oktaId = claimsPrincipal
                .Claims
                .Where(c => c.Type == MEClaimTypes.OktaUserId).Select(c => c.Value)
                .First();

            if (oktaId == null)
            {
                return null;
            }

            // Assume by default it did change.
            var tokenDidChange = true;

            if (meUserSession == null)
            {
                // no session so check user record
                var userTask = _userRetrievalByOktaId.Handle(new UserRetrievalByOktaIdQuery(oktaId));

                var user = userTask?.Result;

                // User doesn't exist in the system yet linked to an okta id.
                if (user == null)
                {
                    // We create them from okta anyway so don't do it again at the end
                    tokenDidChange = false;

                    var newUserTask = CreateNewUser(oktaId, oktaToken);

                    var newUser = newUserTask?.Result;

                    user = newUser ?? throw new Exception("Unable to create user");
                }

                meUserSession = new MeUserSession
                {
                    UserId = user.UserId,
                    OktaId = oktaId,
                };
            }
            else
            {
                tokenDidChange = meUserSession.OktaToken.Equals(oktaToken);
            }

            meUserSession.OktaToken = oktaToken;
            meUserSession.OktaTokenExpiry = DateTimeOffset.Now.AddMinutes(_oktaTokenExpiryTime);

            // Awaitable, but don't wait.
            _userSessionUpdateOktaTokenService.Handle(new UserSessionUpdateOktaTokenQuery(meUserSession));

            // If token did change; assume login happened.
            if (tokenDidChange)
            {
                UpdateUserFromOktaClient(meUserSession.UserId, meUserSession.OktaId);
            }

            return meUserSession;
        }

        /// <summary>
        /// Create new user with details from Okta if does not already exist
        /// </summary>
        /// <param name="oktaId">Okta ID of user from token.</param>
        /// <param name="oktaToken">Okta token</param>
        /// <returns>UserToCreate</returns>
        /// <remarks>virtual so that it can be unit tested via proxy class</remarks>
        private async Task<MeUser> CreateNewUser(string oktaId, string oktaToken)
        {
            // Get everything that Okta knows about this user
            var oktaUser = await _oktaClient.Users.GetUserAsync(oktaId);

            // See if they exist using their email
            var existingUser = await _userRetrievalByEmailService.Handle(new UserRetrievalByEmailQuery(oktaUser.Profile.Email));

            // User exists already
            if (existingUser != null)
            {
                // Just update them and set their okta ID.
                var updatedUser = await _userUpdateOktaService.Handle(new UserUpdateOktaQuery(existingUser.UserId, oktaId));

                return updatedUser;
            }
            else
            {
                var expiryTime = DateTime.Now.AddMinutes(_oktaTokenExpiryTime);

                var createdMeUser = await CreateUser(new MeUser
                {
                    FirstName = oktaUser.Profile.FirstName,
                    LastName = oktaUser.Profile.LastName,
                    Email = oktaUser.Profile.Email,
                    LastModifiedBy = null,
                    ModifiedAt = DateTimeOffset.Now,
                    CreatedAt = DateTimeOffset.Now,
                    OktaId = oktaUser.Id,
                });

                var meUserSession = new MeUserSession()
                {
                    UserId = createdMeUser.UserId,
                    OktaId = oktaId,
                    OktaToken = oktaToken,
                    OktaTokenExpiry = expiryTime
                };

                await _userSessionUpdateOktaTokenService.Handle(new UserSessionUpdateOktaTokenQuery(meUserSession));

                var meUser = createdMeUser;

                return meUser;
            }
        }

        private async Task<MeUser> CreateUser(MeUser toCreate)
        {
            try
            {
                // The user is being created so we don't know what their ID is yet. Pass an empty one for now.
                var currentUser = new MeUser()
                {
                    UserId = Guid.Empty.ToString(),
                };
                var createdUser = await _userCreationService.Handle(new CreateUserQuery(toCreate, currentUser));

                return createdUser;
            }
            catch (DocumentClientException)
            {
                return null;
            }
        }

        private async void UpdateUserFromOktaClient(string userId, string oktaId)
        {
            var meUser = await _userRetrievalById.Handle(new UserRetrievalByIdQuery(userId));

            if (meUser != null)
            {
                var oktaUser = await _oktaClient.Users.GetUserAsync(oktaId);

                var different = false;

                different |= oktaUser.Profile.FirstName != meUser.FirstName;
                different |= oktaUser.Profile.LastName != meUser.LastName;
                different |= oktaUser.Profile.Email != meUser.Email;

                if (different)
                {
                    var updatedUser = new UserUpdateDetails()
                    {
                        UserId = meUser.UserId,
                        FirstName = oktaUser.Profile.FirstName,
                        LastName = oktaUser.Profile.LastName,
                        Email = oktaUser.Profile.Email,
                    };

                    await _userUpdateService.Handle(new UserUpdateQuery(updatedUser, new MeUser{ UserId = "SystemService" }));
                }
            }
        }
    }
}
