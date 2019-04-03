using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;

namespace MedicalExaminer.Common.Services.User
{
    public class UserRetrievalByIdService : IAsyncQueryHandler<UserRetrievalByIdQuery, Models.MeUser>
    {
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IUserConnectionSettings _connectionSettings;

        public UserRetrievalByIdService(IDatabaseAccess databaseAccess, IUserConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }

        public Task<Models.MeUser> Handle(UserRetrievalByIdQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            try
            {
                var result = _databaseAccess.GetItemAsync<Models.MeUser>(_connectionSettings,
                    x => x.UserId == param.UserId);
                return result;
            }
            catch (Exception)
            {
                //_logger.Log("Failed to retrieve examination data", e);
                throw;
            }
        }
    }
}
