using MedicalExaminer.Models;

namespace MedicalExaminer.API.Models
{
    public class PermissionLocation
    {
        public readonly MEUserPermission Permission;
        public readonly Location Location;
        public readonly string UserId;

        public PermissionLocation(MEUserPermission permission, Location location, string userId)
        {
            Location = location;
            Permission = permission;
            UserId = userId;
        }
    }
}
