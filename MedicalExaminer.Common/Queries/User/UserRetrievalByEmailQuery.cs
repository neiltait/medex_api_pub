using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    /// <summary>
    /// User Retrieval By Email Query.
    /// </summary>
    /// <seealso cref="IQuery{MeUser}" />
    public class UserRetrievalByEmailQuery : IQuery<MeUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRetrievalByEmailQuery"/> class.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        public UserRetrievalByEmailQuery(string emailAddress)
        {
            EmailAddress = emailAddress;
        }

        /// <summary>
        /// Gets the email address.
        /// </summary>
        public string EmailAddress { get; }
    }
}
