using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Medical_Examiner_API.Services
{
    /// <summary>
    /// Example Authentication Service that just always gives out tokens
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Example Authentication that requires no input and generates a token
        /// </summary>
        /// <returns>A token.</returns>
        string Authenticate();
    }
}
