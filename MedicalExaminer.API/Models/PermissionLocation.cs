using System.Collections.Generic;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Models
{
    public class PermissionLocation
    {
        public readonly List<MEUserPermission> _permission = new List<MEUserPermission>();
        public readonly List<Location> _location = new List<Location>();
        public readonly string UserId;

        public PermissionLocation(MEUserPermission permission, Location location, string userId)
        {
            _location.Add(location);
            _permission.Add(permission);
            UserId = userId;
        }

        public PermissionLocation(IEnumerable<MEUserPermission> permissions, IEnumerable<Location> locations, string userId)
        {
            _location.AddRange(locations);
            _permission.AddRange(permissions);
            UserId = userId;
        }
    }
}
