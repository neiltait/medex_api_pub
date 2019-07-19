using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    /// <summary>
    /// User Session Retrieval By Okta Id Query.
    /// </summary>
    /// <seealso cref="IQuery{MeUserSession}" />
    public class UserSessionRetrievalByOktaIdQuery : IQuery<MeUserSession>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSessionRetrievalByOktaIdQuery"/> class.
        /// </summary>
        /// <param name="oktaId">The okta identifier.</param>
        public UserSessionRetrievalByOktaIdQuery(string oktaId)
        {
            OktaId = oktaId;
        }

        /// <summary>
        /// Gets the o kta identifier.
        /// </summary>
        public string OktaId { get; }
    }
}
