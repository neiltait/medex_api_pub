using MedicalExaminer.Models;
using System.Collections.Generic;

namespace MedicalExaminer.API.Models
{
    public class PermissionLocation
    {
        public readonly MEUserPermission Permission;
        public readonly List<Location> Locations = new List<Location>();
        public readonly string UserId;

        public PermissionLocation(MEUserPermission permission, Location location, string userId)
        {
            Locations.Add(location);
            Permission = permission;
            UserId = userId;
        }

        public PermissionLocation(MEUserPermission permission, IEnumerable<Location> locations, string userId)
        {
            Locations.AddRange(locations);
            Permission = permission;
            UserId = userId;
        }
    }
}
