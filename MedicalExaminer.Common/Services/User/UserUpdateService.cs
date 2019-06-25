using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;

namespace MedicalExaminer.Common.Services.User
{
    /// <summary>
    /// User Update Service.
    /// </summary>
    public class UserUpdateService : QueryHandler<UserUpdateQuery, Models.MeUser>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UserUpdateService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">User Connection Settings.</param>
        public UserUpdateService(IDatabaseAccess databaseAccess, IUserConnectionSettings connectionSettings)
            : base(databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override async Task<Models.MeUser> Handle(UserUpdateQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var userToUpdate = await GetItemByIdAsync(param.UserId);

            if (userToUpdate == null)
            {
                throw new InvalidOperationException($"User with id `{param.UserId}` not found.");
            }

            userToUpdate.Email = param.Email;
            userToUpdate.Permissions = param.Permissions;
            userToUpdate.LastModifiedBy = param.CurrentUser.UserId;
            userToUpdate.ModifiedAt = DateTime.Now;

            var result = await UpdateItemAsync(userToUpdate);
            return result;
        }
    }
}