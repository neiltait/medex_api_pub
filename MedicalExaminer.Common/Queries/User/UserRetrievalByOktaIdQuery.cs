using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    /// <summary>
    /// User Retrieval By Okta Id Query.
    /// </summary>
    /// <seealso cref="IQuery{MeUser}" />
    public class UserRetrievalByOktaIdQuery : IQuery<MeUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRetrievalByOktaIdQuery"/> class.
        /// </summary>
        /// <param name="oktaId">The okta identifier.</param>
        public UserRetrievalByOktaIdQuery(string oktaId)
        {
            OktaId = oktaId;
        }

        /// <summary>
        /// Gets the o kta identifier.
        /// </summary>
        public string OktaId { get; }
    }
}
