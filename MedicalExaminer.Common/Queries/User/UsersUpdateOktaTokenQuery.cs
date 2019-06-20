using System;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    /// <summary>
    /// Users Update Okta Token Query.
    /// </summary>
    public class UsersUpdateOktaTokenQuery : IQuery<MeUser>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UsersUpdateOktaTokenQuery"/>.
        /// </summary>
        /// <param name="user">The user object to get details from.</param>
        public UsersUpdateOktaTokenQuery(MeUser user)
        {
            UserId = user.UserId;
            OktaToken = user.OktaToken;
            OktaTokenExpiry = user.OktaTokenExpiry;
        }

        /// <summary>
        /// Okta Token.
        /// </summary>
        public string OktaToken { get; }

        /// <summary>
        /// Okta Token Expiry.
        /// </summary>
        public DateTimeOffset OktaTokenExpiry { get; }

        /// <summary>
        /// User Id.
        /// </summary>
        public string UserId { get; }
    }
}
