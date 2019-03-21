using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.User
{
    public class UserRetrievalService : IAsyncQueryHandler<UserRetrievalQuery, MeUser>
    {
        private readonly IUserConnectionSettings connectionSettings;
        private readonly IDatabaseAccess databaseAccess;

        public UserRetrievalService(IDatabaseAccess databaseAccess, IUserConnectionSettings connectionSettings)
        {
            this.databaseAccess = databaseAccess;
            this.connectionSettings = connectionSettings;
        }

        public Task<MeUser> Handle(UserRetrievalQuery param)
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