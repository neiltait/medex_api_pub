using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using MedicalExaminer.API.Authorization;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.IdentityModel.Tokens;

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
        /// Token Update Service
        /// </summary>
        private readonly IAsyncQueryHandler<UsersUpdateOktaTokenQuery, MeUser> _userUpdateOktaTokenService;

        /// <summary>
        /// Token Retrieval Service
        /// </summary>
        private readonly IAsyncQueryHandler<UserRetrievalByOktaTokenQuery, MeUser> _userRetrieveOktaTokenService;

        /// <summary>
        /// User Retrieval By Okta Id Service
        /// </summary>
        private readonly IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser> _usersRetrievalByOktaIdService;

        /// <summary>
        /// Time in minutes before Okta token expires and has to be resubmitted to Okta
        /// </summary>
        private readonly int _oktaTokenExpiryTime;

        /// <summary>
        ///     Initialise a new instance of the Okta JWT Security Token Handler
        /// </summary>
        /// <param name="tokenService">The Token Service.</param>
        /// <param name="tokenValidator">Token validator.</param>
        /// <param name="userUpdateOktaTokenService">User update okta token service.</param>
        /// <param name="userRetrieveOktaTokenService">User retrieval okta token service.</param>
        /// <param name="usersRetrievalByOktaIdService">Users retrieval by okta id service.</param>
        /// <param name="oktaTokenExpiryTime">Okta token expiry time.</param>
        public OktaJwtSecurityTokenHandler(
            ITokenService tokenService,
            ISecurityTokenValidator tokenValidator,
            IAsyncQueryHandler<UsersUpdateOktaTokenQuery, MeUser> userUpdateOktaTokenService,
            IAsyncQueryHandler<UserRetrievalByOktaTokenQuery, MeUser> userRetrieveOktaTokenService,
            IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser> usersRetrievalByOktaIdService,
            int oktaTokenExpiryTime)
        {
            _tokenHandler = tokenValidator;
            _tokenService = tokenService;
            _userUpdateOktaTokenService = userUpdateOktaTokenService;
            _userRetrieveOktaTokenService = userRetrieveOktaTokenService;
            _usersRetrievalByOktaIdService = usersRetrievalByOktaIdService;
            _oktaTokenExpiryTime = oktaTokenExpiryTime;
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
            var meUserWithToken = new MeUser
            {
                OktaToken = securityToken
            };

            // Attempt to retrieve user based on token. If it can be retrieved and token not expired then can bypass call to Okta
            var result = _userRetrieveOktaTokenService.Handle(new UserRetrievalByOktaTokenQuery(meUserWithToken));

            MeUser meUserReturned = null;

            if (result?.Result != null)
            {
                meUserReturned = result.Result;
            }

            // Cannot find user by token or token has expired so go to Okta
            if (meUserReturned == null || meUserReturned.OktaTokenExpiry < DateTimeOffset.Now)
            {
                var response = _tokenService.IntrospectToken(securityToken, new HttpClient()).Result;

                if (!response.Active)
                {
                    validatedToken = null;
                    throw new SecurityTokenValidationException();
                }
            }

            // Use _tokenHandler to deserialize Okta onto claimsPrincipal. Note that the token carries an expiry internally
            // If it has expired then we would expect SecurityTokenExpiredException to be thrown and authentication fails
            var claimsPrincipal = _tokenHandler.ValidateToken(securityToken, validationParameters, out validatedToken);

            // Update user record with okta settings if not found originally or if token had expired
            //if ((meUserReturned == null || meUserReturned.OktaTokenExpiry < DateTimeOffset.Now) &&
            //    claimsPrincipal != null)
            //{
            //    meUserReturned = UpdateOktaTokenForUser(securityToken, claimsPrincipal);
            //}

            if (meUserReturned != null)
            {
                claimsPrincipal?.Identities.First().AddClaim(
                    new Claim(
                        Authorization.MEClaimTypes.UserId,
                        meUserReturned.UserId));
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
        private MeUser UpdateOktaTokenForUser(string oktaToken, ClaimsPrincipal claimsPrincipal)
        {
            var oktaId = claimsPrincipal.Claims.Where(c => c.Type == MEClaimTypes.OktaUserId).Select(c => c.Value).First();

            if (oktaId != null)
            {
                var meUserReturned = _usersRetrievalByOktaIdService.Handle(new UserRetrievalByOktaIdQuery(oktaId));

                if (meUserReturned?.Result != null)
                {
                    var meUser = meUserReturned.Result;
                    meUser.OktaToken = oktaToken;
                    meUser.OktaTokenExpiry = DateTimeOffset.Now.AddMinutes(_oktaTokenExpiryTime);
                    _userUpdateOktaTokenService.Handle(new UsersUpdateOktaTokenQuery(meUser));

                    return meUser;
                }
            }

            return null;
        }
    }
}