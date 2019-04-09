using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;

namespace MedicalExaminer.Common.Services.User
{
    public class UsersRetrievalByOktaTokenService : IAsyncQueryHandler<UserRetrievalByOktaTokenQuery, Models.MeUser>
    {
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IUserConnectionSettings _connectionSettings;

        public UsersRetrievalByOktaTokenService(IDatabaseAccess databaseAccess, IUserConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }


        public  Task<Models.MeUser> Handle(UserRetrievalByOktaTokenQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            try
            {
                var result = _databaseAccess.GetItemAsync<Models.MeUser>(_connectionSettings,
                    x => x.OktaToken == param.OktaToken);

                if (result.Result != null)
                    return result;
                else
                    return null;
            }
            catch (Exception e)
            {
                //_logger.Log("Failed to retrieve examination data", e);
                throw;
            }
        }
    }
}
