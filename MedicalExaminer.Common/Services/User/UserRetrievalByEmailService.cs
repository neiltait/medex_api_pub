using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.User
{
    /// <summary>
    /// User Retrieval By Email Service.
    /// </summary>
    public class UserRetrievalByEmailService : QueryHandler<UserRetrievalByEmailQuery, MeUser>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UserRetrievalByEmailService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">Connection Settings.</param>
        public UserRetrievalByEmailService(IDatabaseAccess databaseAccess, IUserConnectionSettings connectionSettings)
            : base(databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override async Task<MeUser> Handle(UserRetrievalByEmailQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var result = await GetItemAsync(x => x.Email.ToLower() == param.EmailAddress.ToLower());
            return result;
        }
    }
}