using System;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    /// <summary>
    /// Users Update Okta Token Query.
    /// </summary>
    public class UserSessionUpdateOktaTokenQuery : IQuery<MeUserSession>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UserSessionUpdateOktaTokenQuery"/>.
        /// </summary>
        /// <param name="user">The user object to get details from.</param>
        public UserSessionUpdateOktaTokenQuery(MeUserSession user)
        {
            UserId = user.UserId;
            OktaId = user.OktaId;
            OktaToken = user.OktaToken;
            OktaTokenExpiry = user.OktaTokenExpiry;
        }

        /// <summary>
        /// User Id.
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// Okta Id.
        /// </summary>
        public string OktaId { get; }

        /// <summary>
        /// Okta Token.
        /// </summary>
        public string OktaToken { get; }

        /// <summary>
        /// Okta Token Expiry.
        /// </summary>
        public DateTimeOffset OktaTokenExpiry { get; }
    }
}
