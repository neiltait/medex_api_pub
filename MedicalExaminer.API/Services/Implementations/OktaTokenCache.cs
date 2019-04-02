using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MedicalExaminer.API.Services.Implementations
{
    /// <inheritdoc />
    public class OktaTokenCache : IOktaTokenCache
    {
        /// <inheritdoc />
        public ClaimsPrincipal RetrieveClaimsPrincipal(string securityToken)
        {
            return null;
        }


        /// <inheritdoc />
        public void CacheClaimsPrincipal(string securityToken, ClaimsPrincipal claimsPrincipal, DateTime dateTime)
        {

        }
    }
}
