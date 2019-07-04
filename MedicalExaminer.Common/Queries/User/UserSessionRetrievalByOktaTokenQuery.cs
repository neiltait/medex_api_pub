using System;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    /// <summary>
    /// User Session Retrieval By Okta Token Query.
    /// </summary>
    public class UserSessionRetrievalByOktaTokenQuery : IQuery<MeUserSession>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UserSessionRetrievalByOktaTokenQuery"/>.
        /// </summary>
        /// <param name="oktaToken">Okta Token.</param>
        public UserSessionRetrievalByOktaTokenQuery(string oktaToken)
        {
            OktaToken = oktaToken;
        }

        /// <summary>
        /// Okta Token to query on.
        /// </summary>
        public string OktaToken { get; }
    }
}
