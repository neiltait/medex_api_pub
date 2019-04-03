using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using MedicalExaminer.API.Models;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Common.Services.User;
using MedicalExaminer.Models;
using Microsoft.Extensions.Options;
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
        /// Time in miutes before Okta token expires and has to be resubmitted to Okta
        /// </summary>
        private readonly int _oktaTokenExpiryTime;

        /// <summary>
        ///     Initialise a new instance of the Okta JWT Security Token Handler
        /// </summary>
        /// <param name="tokenService">The Token Service.</param>
        /// <param name="tokenValidator">Token validator.</param>
        public OktaJwtSecurityTokenHandler(ITokenService tokenService, ISecurityTokenValidator tokenValidator, IAsyncQueryHandler<UsersUpdateOktaTokenQuery, MeUser> userUpdateOktaTokenService, IAsyncQueryHandler<UserRetrievalByOktaTokenQuery, MeUser> userRetrieveOktaTokenService, int oktaTokenExpiryTime)
        {
            _tokenHandler = tokenValidator;
            _tokenService = tokenService;
            _userUpdateOktaTokenService = userUpdateOktaTokenService;
            _userRetrieveOktaTokenService = userRetrieveOktaTokenService;
            _oktaTokenExpiryTime = oktaTokenExpiryTime;
        }

        /// <inheritdoc />
        public bool CanValidateToken => true;

        /// <inheritdoc />
        public int MaximumTokenSizeInBytes
        {
            get => TokenValidationParameters.DefaultMaximumTokenSizeInBytes;
            set => throw new NotImplementedException();
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

            //Attempt to retrieve user based on token. If it can be retrieved then can bypass call to Okta
            var result = _userRetrieveOktaTokenService.Handle(new UserRetrievalByOktaTokenQuery(meUserWithToken));

            MeUser meUserReturned = null;

            if (result != null && result.Result != null)
                meUserReturned = result.Result;

            //Cannot find user by token or token has expired so go to Okta
            if (meUserReturned == null || meUserReturned.OktaTokenExpiry < DateTimeOffset.Now)
            {
                var response = _tokenService.IntrospectToken(securityToken, new HttpClient()).Result;

                if (!response.Active)
                {
                    validatedToken = null;
                    throw new SecurityTokenValidationException();
                }

                //If user was there but token expired then reset expiry
                if (meUserReturned != null)
                {
                    var expiryTime = DateTime.Now.AddMinutes(_oktaTokenExpiryTime);
                    meUserReturned.OktaToken = securityToken;
                    meUserReturned.OktaTokenExpiry = expiryTime;
                    _userUpdateOktaTokenService.Handle(new UsersUpdateOktaTokenQuery(meUserReturned));
                }
            }

            

            return _tokenHandler.ValidateToken(securityToken, validationParameters, out validatedToken);
        }
    }
}