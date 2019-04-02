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
        /// <summary>
        /// Time in seconds after which a cached token is deemed to have expired and is discarded
        /// </summary>
        /// <remarks>todo: make this configurable</remarks>
        private const int expiryLimit = 600; 

        protected Dictionary<string, ClaimsPrincipal> _claimsPrincipals;

        protected struct TokenExpiry
        {
            public DateTime timestamp;
            public string securityToken;
        }

        protected List<TokenExpiry> _tokenExpirys;

        /// <summary>
        /// Constructor
        /// </summary>
        public OktaTokenCache()
        {
            _claimsPrincipals = new Dictionary<string, ClaimsPrincipal>();
            _tokenExpirys = new List<TokenExpiry>();
        }
        /// <inheritdoc />
        public ClaimsPrincipal RetrieveClaimsPrincipal(string securityToken)
        {
            PurgeExpiredTokens();
            return null;
        }


        /// <inheritdoc />
        public void CacheClaimsPrincipal(string securityToken, ClaimsPrincipal claimsPrincipal, DateTime dateTime)
        {
            
        }

        /// <summary>
        /// Remove all tokens from dictionary that have expired
        /// </summary>
        private void PurgeExpiredTokens()
        {
            var timeNow = DateTime.Now;
            var expiryCutOff = timeNow.AddSeconds(-expiryLimit);
            var listExpired = _tokenExpirys.FindAll(t => t.timestamp < expiryCutOff).ToList();
            _tokenExpirys.RemoveAll(t => t.timestamp < expiryCutOff);
        }
    }
}
