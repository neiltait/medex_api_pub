using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;

namespace MedicalExaminer.Common.Services.User
{
    /// <summary>
    /// User Session Retrieval By Okta Id Service.
    /// </summary>
    public class UserSessionRetrievalByOktaIdService : QueryHandler<UserSessionRetrievalByOktaIdQuery, Models.MeUserSession>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UserSessionRetrievalByOktaIdService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database access</param>
        /// <param name="connectionSettings">Connection settings</param>
        public UserSessionRetrievalByOktaIdService(IDatabaseAccess databaseAccess, IUserSessionConnectionSettings connectionSettings)
            : base(databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override async Task<Models.MeUserSession> Handle(UserSessionRetrievalByOktaIdQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var result = await GetItemAsync(x => x.OktaId == param.OktaId);

            return result;
        }
    }
}
