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
        private readonly MeUser _systemUser = new MeUser { UserId = "SystemService" };

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
        /// Token Update Service
        /// </summary>
        private readonly IAsyncQueryHandler<UserSessionUpdateOktaTokenQuery, MeUserSession>
            _userSessionUpdateOktaTokenService;

        /// <summary>
        /// Token Retrieval Service
        /// </summary>
        private readonly IAsyncQueryHandler<UserSessionRetrievalByOktaIdQuery, MeUserSession>
            _userSessionRetrievalByOktaIdService;

        /// <summary>
        /// User Update Service.
        /// </summary>
        private readonly IAsyncQueryHandler<UserUpdateQuery, MeUser> _userUpdateService;

        private readonly IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> _userRetrievalByEmailService;

        private readonly IAsyncQueryHandler<CreateUserQuery, MeUser> _userCreationService;

        private readonly IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser> _userRetrievalByOktaId;

        /// <summary>
        ///     Okta Client.
        /// </summary>
        private readonly IOktaClient _oktaClient;

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
        /// <param name="userSessionRetrievalByOktaIdService">User retrieval by okta id service.</param>
        /// <param name="userRetrievalById">User retrieval by id service.</param>
        /// <param name="userRetrievalByEmailService">User retrieval by email service.</param>
        /// <param name="userRetrievalByOktaId">User retrieval by okta id.</param>
        /// <param name="userUpdateService">Update user service.</param>
        /// <param name="userCreationService">User creation service.</param>
        /// <param name="oktaClient">Okta client.</param>
        /// <param name="oktaTokenExpiryTime">Okta token expiry time.</param>
        public OktaJwtSecurityTokenHandler(
            ITokenService tokenService,
            ISecurityTokenValidator tokenValidator,
            IAsyncQueryHandler<UserSessionUpdateOktaTokenQuery, MeUserSession> userSessionUpdateOktaTokenService,
            IAsyncQueryHandler<UserSessionRetrievalByOktaIdQuery, MeUserSession> userSessionRetrievalByOktaIdService,
            IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser> userRetrievalById,
            IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser> userRetrievalByOktaId,
            IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> userRetrievalByEmailService,
            IAsyncQueryHandler<UserUpdateQuery, MeUser> userUpdateService,
            IAsyncQueryHandler<CreateUserQuery, MeUser> userCreationService,
            IOktaClient oktaClient,
            int oktaTokenExpiryTime)
        {
            _tokenHandler = tokenValidator;
            _tokenService = tokenService;
            _userSessionUpdateOktaTokenService = userSessionUpdateOktaTokenService;
            _oktaTokenExpiryTime = oktaTokenExpiryTime;
            _userSessionRetrievalByOktaIdService = userSessionRetrievalByOktaIdService;
            _userRetrievalByEmailService = userRetrievalByEmailService;
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
            // Use _tokenHandler to deserialize Okta onto claimsPrincipal. Note that the token carries an expiry internally
            // If it has expired then we would expect SecurityTokenExpiredException to be thrown and authentication fails
            var claimsPrincipal = _tokenHandler.ValidateToken(securityToken, validationParameters, out validatedToken);

            // If above didn't already throw exception, catch null being returned.
            if (claimsPrincipal == null)
            {
                validatedToken = null;
                throw new SecurityTokenValidationException();
            }

            var oktaId = claimsPrincipal
                .Claims
                .Where(c => c.Type == MEClaimTypes.OktaUserId)
                .Select(c => c.Value)
                .FirstOrDefault();

            // No point continuing if we don't have any idea who it is.
            if (oktaId == null)
            {
                validatedToken = null;
                throw new SecurityTokenValidationException();
            }

            // Attempt to retrieve user session based on okta id in the token.
            var userSessionTask = _userSessionRetrievalByOktaIdService.Handle(new UserSessionRetrievalByOktaIdQuery(oktaId));
            var userSession = userSessionTask?.Result;

            var shouldCheckToken = userSession == null
                                   || userSession.OktaTokenExpiry < DateTimeOffset.Now
                                   || userSession.OktaToken != securityToken;

            if (shouldCheckToken)
            {
                var introspectTokenResponse = _tokenService.IntrospectToken(securityToken, new HttpClient()).Result;

                // Token is invalid or has been recalled; can't continue.
                if (!introspectTokenResponse.Active)
                {
                    validatedToken = null;
                    throw new SecurityTokenValidationException();
                }
            }

            try
            {
                userSession = CreateOrUpdateSession(userSession, oktaId, securityToken);
            }
            catch
            {
                validatedToken = null;
                throw new SecurityTokenValidationException();
            }

            if (userSession.UserId != null)
            {
                claimsPrincipal.Identities.First().AddClaim(
                    new Claim(
                        MEClaimTypes.UserId,
                        userSession.UserId));
            }

            return claimsPrincipal;
        }

        /// <summary>
        /// Create or update session.
        /// </summary>
        /// <param name="userSession">Existing session or null if not created yet.</param>
        /// <param name="oktaId">Okta id of the user from the token.</param>
        /// <param name="oktaToken">The current token being used.</param>
        /// <returns>The active session for this user.</returns>
        private MeUserSession CreateOrUpdateSession(MeUserSession userSession, string oktaId, string oktaToken)
        {
            var didChangeSession = false;

            if (userSession == null)
            {
                // User has never had a session so go see if the user exists.
                var userTask = _userRetrievalByOktaId.Handle(new UserRetrievalByOktaIdQuery(oktaId));
                var user = userTask?.Result;

                // A user doesn't exist in the system yet who's linked to an okta id.
                if (user == null)
                {
                    var newUserTask = CreateOrBindUser(oktaId);
                    var newUser = newUserTask.Result;

                    user = newUser;
                }

                didChangeSession = true;
                userSession = new MeUserSession
                {
                    UserId = user.UserId,
                    OktaId = oktaId,
                    OktaToken = oktaToken
                };
            }

            // User has previously had a session but token doesn't match
            else if (userSession.OktaToken != oktaToken)
            {
                didChangeSession = true;
                userSession.OktaToken = oktaToken;

                // If the token changed; assume a "login" is happening so pull okta details again
                UpdateUserFromOktaClient(userSession.UserId, userSession.OktaId);
            }

            // Only update the expiry after its expired.
            if (userSession.OktaTokenExpiry < DateTimeOffset.Now)
            {
                didChangeSession = true;
                userSession.OktaTokenExpiry = DateTimeOffset.Now.AddMinutes(_oktaTokenExpiryTime);
            }

            // Finally update the session with whatever we've changed.
            if (didChangeSession)
            {
                _userSessionUpdateOktaTokenService.Handle(new UserSessionUpdateOktaTokenQuery(userSession));
            }

            return userSession;
        }

        /// <summary>
        /// Create or bind existing user to okta Id.
        /// </summary>
        /// <param name="oktaId">Okta ID of user from token.</param>
        /// <returns>A new user or the existing user after being updated.</returns>
        private async Task<MeUser> CreateOrBindUser(string oktaId)
        {
            var oktaUser = await _oktaClient.Users.GetUserAsync(oktaId);

            var existingUserWithMatchingEmail =
                await _userRetrievalByEmailService.Handle(new UserRetrievalByEmailQuery(oktaUser.Profile.Email));

            if (existingUserWithMatchingEmail == null)
            {
                var toCreate = new MeUser
                {
                    OktaId = oktaUser.Id,
                    FirstName = oktaUser.Profile.FirstName,
                    LastName = oktaUser.Profile.LastName,
                    Email = oktaUser.Profile.Email,
                };

                var createdUser = await _userCreationService.Handle(new CreateUserQuery(toCreate, _systemUser));

                return createdUser;
            }

            return await BindUser(existingUserWithMatchingEmail, oktaUser);
        }

        private async Task<MeUser> BindUser(MeUser existingUser, IUser oktaUser)
        {
            if (existingUser.OktaId != null)
            {
                throw new Exception("User already bound to an existing okta id.");
            }

            var updateUser = new UserUpdateFull()
            {
                UserId = existingUser.UserId,
                OktaId = oktaUser.Id,
                FirstName = oktaUser.Profile.FirstName,
                LastName = oktaUser.Profile.LastName,
                Email = oktaUser.Profile.Email,
            };

            var updatedUser =
                await _userUpdateService.Handle(new UserUpdateQuery(
                    updateUser,
                    _systemUser));

            return updatedUser;
        }

        private void UpdateUserFromOktaClient(string userId, string oktaId)
        {
            var meUserTask = _userRetrievalById.Handle(new UserRetrievalByIdQuery(userId));
            var meUser = meUserTask?.Result;

            if (meUser == null)
            {
                throw new Exception("User record missing");
            }

            var oktaUserTask = _oktaClient.Users.GetUserAsync(oktaId);
            var oktaUser = oktaUserTask?.Result;

            if (oktaUser != null)
            {
                var different = false;

                // If any of the fields differ; we update the whole record again.
                different |= oktaUser.Profile.FirstName != meUser.FirstName;
                different |= oktaUser.Profile.LastName != meUser.LastName;
                different |= oktaUser.Profile.Email != meUser.Email;

                if (!different)
                {
                    return;
                }

                var updatedUser = new UserUpdateDetails()
                {
                    UserId = meUser.UserId,
                    FirstName = oktaUser.Profile.FirstName,
                    LastName = oktaUser.Profile.LastName,
                    Email = oktaUser.Profile.Email,
                };

                var updateUserTask = _userUpdateService.Handle(new UserUpdateQuery(updatedUser, _systemUser));
                updateUserTask.Wait();
            }
        }
    }
}
