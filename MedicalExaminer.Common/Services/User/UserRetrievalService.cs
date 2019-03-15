using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.User;
using Microsoft.Azure.Documents;


namespace MedicalExaminer.Common.Services.User
{
    public class UserRetrievalService : IAsyncQueryHandler<UserRetrievalQuery, Models.MeUser>
    {
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IUserConnectionSettings _connectionSettings;

        public UserRetrievalService(IDatabaseAccess databaseAccess, IUserConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }

        public Task<Models.MeUser> Handle(UserRetrievalQuery param)
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
