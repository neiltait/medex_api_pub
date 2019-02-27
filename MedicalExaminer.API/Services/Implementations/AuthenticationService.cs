using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MedicalExaminer.API.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MedicalExaminer.API.Services.Implementations
{
    /// <summary>
    /// Authentication Service.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        /// <summary>
        /// The authentication settings.
        /// </summary>
        private readonly AuthenticationSettings _authenticationSettings;

        /// <summary>
        /// Initialise a new instance of the Authentication Service.
        /// </summary>
        /// <param name="authenticationSettings">Authentication Settings.</param>
        public AuthenticationService(IOptions<AuthenticationSettings> authenticationSettings)
        {
            _authenticationSettings = authenticationSettings.Value;
        }

        /// <inheritdoc/>
        public string Authenticate()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authenticationSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Role, "example"),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
