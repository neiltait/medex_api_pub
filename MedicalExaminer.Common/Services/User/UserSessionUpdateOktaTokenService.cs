using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.User
{
    /// <summary>
    /// User Session Update Okta Token Service.
    /// </summary>
    public class UserSessionUpdateOktaTokenService : QueryHandler<UserSessionUpdateOktaTokenQuery, MeUserSession>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UserSessionUpdateOktaTokenService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database access.</param>
        /// <param name="connectionSettings">Connection settings.</param>
        public UserSessionUpdateOktaTokenService(
            IDatabaseAccess databaseAccess,
            IUserSessionConnectionSettings connectionSettings)
            : base(databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override async Task<MeUserSession> Handle(UserSessionUpdateOktaTokenQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var userSessionToUpdate = await GetItemByIdAsync(query.UserId);

            // If no session exists, create one.
            if (userSessionToUpdate == null)
            {
                userSessionToUpdate = new MeUserSession
                {
                    UserId = query.UserId,
                    OktaId = query.OktaId
                };
            }

            userSessionToUpdate.OktaToken = query.OktaToken;
            userSessionToUpdate.OktaTokenExpiry = query.OktaTokenExpiry;

            var result = await UpdateItemAsync(userSessionToUpdate);
            return result;
        }
    }
}
