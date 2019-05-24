using System.Collections.Generic;
using System.Linq;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    public class UserUpdateQuery : IQuery<MeUser>
    {
        public UserUpdateQuery(MeUser userToUpdate, MeUser currentUser)
        {
            CurrentUser = currentUser;
            UserId = userToUpdate.UserId;
            Email = userToUpdate.Email;
            Permissions = userToUpdate.Permissions != null
                ? userToUpdate.Permissions.Select(up => new MEUserPermission()
                {
                    PermissionId = up.PermissionId,
                    LocationId = up.LocationId,
                    UserRole = up.UserRole,
                })
                : Enumerable.Empty<MEUserPermission>();
        }

        public MeUser CurrentUser { get; }

        public string UserId { get; }

        public string Email { get; }

        public IEnumerable<MEUserPermission> Permissions { get; }
    }
}