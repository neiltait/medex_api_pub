using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;

namespace MedicalExaminer.Common.Services.User
{
    /// <summary>
    /// User Session Retrieval By Okta Token Service.
    /// </summary>
    public class UserSessionRetrievalByOktaTokenService : QueryHandler<UserSessionRetrievalByOktaTokenQuery, Models.MeUserSession>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UserSessionRetrievalByOktaTokenService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database access</param>
        /// <param name="connectionSettings">Connection settings</param>
        public UserSessionRetrievalByOktaTokenService(IDatabaseAccess databaseAccess, IUserConnectionSettings connectionSettings)
            : base(databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override async Task<Models.MeUserSession> Handle(UserSessionRetrievalByOktaTokenQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var result = await GetItemAsync(x => x.OktaToken == param.OktaToken);

            return result;
        }
    }
}
