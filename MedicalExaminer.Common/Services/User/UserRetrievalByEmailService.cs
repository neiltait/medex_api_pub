using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;


namespace MedicalExaminer.Common.Services.User
{
    public class UserRetrievalByEmailService : IAsyncQueryHandler<UserRetrievalByEmailQuery, Models.MeUser>
    {
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IUserConnectionSettings _connectionSettings;

        public UserRetrievalByEmailService(IDatabaseAccess databaseAccess, IUserConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }

        public Task<Models.MeUser> Handle(UserRetrievalByEmailQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            try
            {
                var result = _databaseAccess.GetItemAsync<Models.MeUser>(_connectionSettings,
                    x => x.Email == param.UserEmail);
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
