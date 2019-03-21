using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.User
{
    public class CreateUserService : IAsyncQueryHandler<CreateUserQuery, MeUser>
    {
        private readonly IConnectionSettings connectionSettings;
        private readonly IDatabaseAccess databaseAccess;

        public CreateUserService(IDatabaseAccess databaseAccess, IUserConnectionSettings connectionSettings)
        {
            this.databaseAccess = databaseAccess;
            this.connectionSettings = connectionSettings;
        }

        public async Task<MeUser> Handle(CreateUserQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            param.MeUser.UserId = Guid.NewGuid().ToString();
            var result = await databaseAccess.CreateItemAsync(connectionSettings, param.MeUser, false);
            return result;
        }
    }
}