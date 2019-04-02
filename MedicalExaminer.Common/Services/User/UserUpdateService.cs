using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;

namespace MedicalExaminer.Common.Services.User
{
    public class UserUpdateService : IAsyncQueryHandler<UserUpdateQuery, Models.MeUser>
    {
        private readonly IConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;

        public UserUpdateService(
            IDatabaseAccess databaseAccess,
            IUserConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }

        public async Task<Models.MeUser> Handle(UserUpdateQuery userUpdate)
        {
            if (userUpdate == null)
            {
                throw new ArgumentNullException(nameof(userUpdate));
            }
            
            var userToUpdate = await
                _databaseAccess
                    .GetItemAsync<Models.MeUser>(
                        _connectionSettings,
                        meUser => meUser.UserId == userUpdate.UserId);

            if (userUpdate == null)
            {
                throw new ArgumentNullException(nameof(userUpdate));
            }

            userToUpdate.Email = userUpdate.Email;
            userToUpdate.Permissions = userUpdate.Permissions;
            
            var result = await _databaseAccess.UpdateItemAsync(_connectionSettings, userToUpdate);
            return result;
        }
    }
}