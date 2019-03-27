using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.User
{
    public class UserRetrievalByIdService : IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>
    {
        private readonly IUserConnectionSettings connectionSettings;
        private readonly IDatabaseAccess databaseAccess;

        public UserRetrievalByIdService(IDatabaseAccess databaseAccess, IUserConnectionSettings connectionSettings)
        {
            this.databaseAccess = databaseAccess;
            this.connectionSettings = connectionSettings;
        }

        public Task<MeUser> Handle(UserRetrievalByIdQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var result = databaseAccess.GetItemAsync<MeUser>(
                connectionSettings,
                x => x.UserId == param.UserId);

            if (result == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            return result;
        }
    }
}