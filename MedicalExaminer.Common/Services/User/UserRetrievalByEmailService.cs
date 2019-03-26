using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.User
{
    public class UserRetrievalByEmailService : IAsyncQueryHandler<UserRetrievalByEmailQuery, Models.MeUser>
    {
        private readonly IUserConnectionSettings connectionSettings;
        private readonly IDatabaseAccess databaseAccess;

        public UserRetrievalByEmailService(IDatabaseAccess databaseAccess, IUserConnectionSettings connectionSettings)
        {
            this.databaseAccess = databaseAccess;
            this.connectionSettings = connectionSettings;
        }

        public Task<Models.MeUser> Handle(UserRetrievalByEmailQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var result = databaseAccess.GetItemAsync<MeUser>(
                connectionSettings,
                x => x.Email == param.UserEmail);
            return result;
        }
    }
}