using System.Collections.Generic;
using System.Linq;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Queries.User
{
    public class UserUpdateQuery : IQuery<MeUser>
    {
        public UserUpdateQuery(MeUser user)
        {
            UserId = user.UserId;
            Email = user.Email;
            Permissions = user.Permissions != null
                ? user.Permissions.Select(up => new MEUserPermission()
                {
                    PermissionId = up.PermissionId,
                    LocationId = up.LocationId,
                    UserRole = up.UserRole,
                })
                : Enumerable.Empty<MEUserPermission>();
        }

        public string UserId { get; }
        public string Email { get; }
        public IEnumerable<MEUserPermission> Permissions { get; }
    }
}