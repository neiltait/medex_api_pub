using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MedicalExaminer.API.Services.Implementations
{
    /// <summary>
    /// Maintains list of Okta tokens and associated ClaimPrincipals
    /// </summary>
    public interface IOktaTokenCache
    {
        /// <summary>
        /// Retrieves ClaimsPrincipal associated with token
        /// </summary>
        /// <param name="securityToken">Okta token to associate with ClaimsPrincipal</param>
        /// <returns>ClaimsPrincipal associated with token</returns>
        /// <remarks>returns null if token has not been cached before or if it has expired</remarks>
        ClaimsPrincipal RetrieveClaimsPrincipal(string securityToken);

        /// <summary>
        /// Caches ClaimsPrincipal and associated Okta security token
        /// </summary>
        /// <param name="securityToken">token from Okta</param>
        /// <param name="claimsPrincipal">ClaimsPrincipal retrieved from Okta</param>
        /// <param name="dateTime"></param>
        void CacheClaimsPrincipal(string securityToken, ClaimsPrincipal claimsPrincipal, DateTime dateTime);
    }
}
