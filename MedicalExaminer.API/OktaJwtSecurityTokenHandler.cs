using System;
using System.Net.Http;
using System.Security.Claims;
using MedicalExaminer.API.Services;
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
        ///     Initialise a new instance of the Okta JWT Security Token Handler
        /// </summary>
        /// <param name="tokenService">The Token Service.</param>
        /// <param name="tokenValidator">Token validator.</param>
        public OktaJwtSecurityTokenHandler(ITokenService tokenService, ISecurityTokenValidator tokenValidator)
        {
            _tokenHandler = tokenValidator;
            _tokenService = tokenService;
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
            var response = _tokenService.IntrospectToken(securityToken, new HttpClient()).Result;

            if (!response.Active)
            {
                validatedToken = null;
                throw new SecurityTokenValidationException();
            }

            var djp = _tokenHandler.ValidateToken(securityToken, validationParameters, out validatedToken);

            return _tokenHandler.ValidateToken(securityToken, validationParameters, out validatedToken);
        }
    }
}