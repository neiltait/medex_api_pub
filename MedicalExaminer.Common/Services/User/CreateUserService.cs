using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.User
{
    public class CreateUserService : IAsyncQueryHandler<CreateUserQuery, MeUser>
    {
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IConnectionSettings _connectionSettings;

        public CreateUserService(IDatabaseAccess databaseAccess, IUserConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }

        public async Task<MeUser> Handle(CreateUserQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }
            try
            {
                param.MeUser.Id = Guid.NewGuid().ToString();
                param.MeUser.UserId = Guid.NewGuid().ToString();
                var result = await _databaseAccess.CreateItemAsync(_connectionSettings, param.MeUser, false);
                return result;
            }
            catch (Exception e)
            {
                //_logger.Log("Failed to retrieve examination data", e);
                throw;
            }
        }
    }
}
