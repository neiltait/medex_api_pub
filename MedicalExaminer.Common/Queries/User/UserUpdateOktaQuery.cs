using System;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    /// <summary>
    /// User Update Okta Query.
    /// </summary>
    public class UserUpdateOktaQuery : IQuery<MeUser>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UserUpdateOktaQuery"/>.
        /// </summary>
        /// <param name="userId">The id to query,</param>
        /// <param name="oktaId">The okta id to set against this user.</param>
        public UserUpdateOktaQuery(string userId, string oktaId)
        {
            UserId = userId;
            OktaId = oktaId;
        }

        /// <summary>
        /// User Id.
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// Okta Token.
        /// </summary>
        public string OktaId { get; }
    }
}
