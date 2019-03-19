using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.UserQueries;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.UserService
{
    public class UserRetrievalService : IAsyncQueryHandler<UserRetrievalQuery, MeUser>
    {
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IUserConnectionSettings _connectionSettings;

        public UserRetrievalService(IDatabaseAccess databaseAccess, IUserConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }
        public Task<MeUser> Handle(UserRetrievalQuery param)
        {
            return _databaseAccess.GetItemAsync<MeUser>(_connectionSettings,
                user => user.Id == param.Id);
        }
    }
}
