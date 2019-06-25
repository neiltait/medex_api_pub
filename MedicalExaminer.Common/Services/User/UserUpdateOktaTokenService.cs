using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;
using Microsoft.Azure.Documents.SystemFunctions;

namespace MedicalExaminer.Common.Services.User
{
    public class UserUpdateOktaTokenService : IAsyncQueryHandler<UsersUpdateOktaTokenQuery, Models.MeUser>
    {
        private readonly IConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;

        public UserUpdateOktaTokenService(
            IDatabaseAccess databaseAccess,
            IUserConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }

        public async Task<Models.MeUser> Handle(UsersUpdateOktaTokenQuery userUpdate)
        {
            if (userUpdate == null)
            {
                throw new ArgumentNullException(nameof(userUpdate));
            }

            var userToUpdate =
                _databaseAccess
                    .GetItemByIdAsync<Models.MeUser>(
                        _connectionSettings,
                        userUpdate.UserId).Result;

            if (userToUpdate == null)
            {
                throw new ArgumentNullException(nameof(userToUpdate));
            }

            userToUpdate.OktaToken = userUpdate.OktaToken;
            userToUpdate.OktaTokenExpiry = userUpdate.OktaTokenExpiry;

            var result = await _databaseAccess.UpdateItemAsync(_connectionSettings, userToUpdate);
            return result;
        }
    }
}
