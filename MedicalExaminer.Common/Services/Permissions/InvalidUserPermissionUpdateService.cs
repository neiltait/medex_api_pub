using System;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Permissions;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.Permissions
{
    /// <summary>
    /// Service update Invalid Permission IDs
    /// </summary>
    public class InvalidUserPermissionUpdateService : QueryHandler<InvalidUserPermissionQuery, bool>
    {
        public InvalidUserPermissionUpdateService(
            IDatabaseAccess databaseAccess,
            IUserConnectionSettings connectionSettings)
            : base(databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override async Task<bool> Handle(InvalidUserPermissionQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var thingsToUpdate = DatabaseAccess.GetItemsAsync<MeUser>(ConnectionSettings, meUser => true).Result;

            foreach (var user in thingsToUpdate)
            {
                if (user.Permissions == null)
                {
                    continue;
                }

                // Replace all the id's if there are duplicates
                if (user.Permissions.Count() != user.Permissions.Select(x => x.PermissionId).Distinct().Count())
                {
                    foreach (var permission in user.Permissions)
                    {
                        permission.PermissionId = Guid.NewGuid().ToString();
                    }

                    await DatabaseAccess.UpdateItemAsync(ConnectionSettings, user);
                }
                else
                {
                    // Update only the null or empty ones
                    foreach (var permission in user.Permissions)
                    {
                        if (string.IsNullOrEmpty(permission.PermissionId))
                        {
                            permission.PermissionId = Guid.NewGuid().ToString();
                        }
                    }

                    await DatabaseAccess.UpdateItemAsync(ConnectionSettings, user);
                }
            }

            return true;
        }
    }
}
