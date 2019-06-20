using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.User
{
    /// <summary>
    /// User Update Okta Service.
    /// </summary>
    public class UserUpdateOktaService : QueryHandler<UserUpdateOktaQuery, MeUser>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UserUpdateOktaService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">Connection Settings.</param>
        public UserUpdateOktaService(
            IDatabaseAccess databaseAccess,
            IUserConnectionSettings connectionSettings)
            : base(databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override async Task<MeUser> Handle(UserUpdateOktaQuery userUpdate)
        {
            if (userUpdate == null)
            {
                throw new ArgumentNullException(nameof(userUpdate));
            }

            var userToUpdate = GetItemAsync(meUser => meUser.UserId == userUpdate.UserId).Result;

            if (userToUpdate == null)
            {
                throw new ArgumentNullException(nameof(userToUpdate));
            }

            userToUpdate.OktaId = userUpdate.OktaId;

            var result = await UpdateItemAsync(userToUpdate);
            return result;
        }
    }
}
