using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.User
{
    /// <summary>
    /// User Retrieval By Okta Id Service.
    /// </summary>
    public class UserRetrievalByOktaIdService : QueryHandler<UserRetrievalByOktaIdQuery, MeUser>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UserRetrievalByOktaIdService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">Connection Settings.</param>
        public UserRetrievalByOktaIdService(IDatabaseAccess databaseAccess, IUserConnectionSettings connectionSettings)
            : base(databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override Task<MeUser> Handle(UserRetrievalByOktaIdQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var result = GetItemAsync(x => x.OktaId == param.OktaId);
            return result;
        }
    }
}