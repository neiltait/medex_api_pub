using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MedicalExaminer.API.Services;
using Microsoft.IdentityModel.Tokens;

namespace MedicalExaminer.API
{
    /// <summary>
    /// Okta JWT Security Token Handler
    /// </summary>
    /// <inheritdoc/>
    public class OktaJwtSecurityTokenHandler : ISecurityTokenValidator
    {
        /// <summary>
        /// Standard handler.
        /// </summary>
        private readonly JwtSecurityTokenHandler _tokenHandler;

        /// <summary>
        /// Token service.
        /// </summary>
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Initialise a new instance of the Okta JWT Security Token Handler
        /// </summary>
        /// <param name="tokenService">The Token Service.</param>
        public OktaJwtSecurityTokenHandler(ITokenService tokenService)
        {
            _tokenHandler = new JwtSecurityTokenHandler();

            _tokenService = tokenService;
        }

        /// <inheritdoc/>
        public bool CanValidateToken => true;

        /// <inheritdoc/>
        public int MaximumTokenSizeInBytes
        {
            get => TokenValidationParameters.DefaultMaximumTokenSizeInBytes;
            set => throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public bool CanReadToken(string securityToken)
        {
            return _tokenHandler.CanReadToken(securityToken);
        }

        /// <inheritdoc/>
        public ClaimsPrincipal ValidateToken(
            string securityToken,
            TokenValidationParameters validationParameters,
            out SecurityToken validatedToken)
        {
            var response = _tokenService.IntrospectToken(securityToken).Result;

            Console.WriteLine("Hello from token thing!?");

            if (!response.Active)
            {
                validatedToken = null;
                throw new SecurityTokenValidationException();
            }

            return _tokenHandler.ValidateToken(securityToken, validationParameters, out validatedToken);
        }
    }
}
