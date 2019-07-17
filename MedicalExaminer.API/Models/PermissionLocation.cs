using System.Collections.Generic;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Models
{
    public class PermissionLocation
    {
        public readonly List<MEUserPermission> Permissions = new List<MEUserPermission>();
        public readonly List<Location> Locations = new List<Location>();
        public readonly string UserId;

        public PermissionLocation(MEUserPermission permission, Location location, string userId)
        {
            Locations.Add(location);
            Permissions.Add(permission);
            UserId = userId;
        }

        public PermissionLocation(IEnumerable<MEUserPermission> permissions, IEnumerable<Location> locations, string userId)
        {
            Locations.AddRange(locations);
            Permissions.AddRange(permissions);
            UserId = userId;
        }
    }
}
