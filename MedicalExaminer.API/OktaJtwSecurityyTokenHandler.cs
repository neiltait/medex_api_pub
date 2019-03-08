using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MedicalExaminer.API.Services;
using Microsoft.IdentityModel.Tokens;

namespace MedicalExaminer.API
{
    public class OktaJtwSecurityyTokenHandler : ISecurityTokenValidator
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;

        private readonly ITokenService _tokenService;

        public bool CanReadToken(string securityToken)
        {
            return _tokenHandler.CanReadToken(securityToken);
        }

        public OktaJtwSecurityyTokenHandler(ITokenService tokenService)
        {
            _tokenHandler = new JwtSecurityTokenHandler();

            _tokenService = tokenService;
        }

        public ClaimsPrincipal ValidateToken(
            string securityToken,
            TokenValidationParameters validationParameters,
            out SecurityToken validatedToken)
        {
            var response = _tokenService.IntrospectToken(securityToken).Result;

            if (response.Active )
            {
                return _tokenHandler.ValidateToken(securityToken, validationParameters, out validatedToken);

            }

            // TODO: find out what we do to indicate failure
            return _tokenHandler.ValidateToken(string.Empty, validationParameters, out validatedToken);
        }

        public bool CanValidateToken => true;

        public int MaximumTokenSizeInBytes
        {
            get => TokenValidationParameters.DefaultMaximumTokenSizeInBytes;
            set => throw new System.NotImplementedException();
        }
    }
}
