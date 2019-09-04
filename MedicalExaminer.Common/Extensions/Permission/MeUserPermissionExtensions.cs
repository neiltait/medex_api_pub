using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Extensions.Permission
{
    /// <summary>
    /// Medical Examiner User Permission Extensions.
    /// </summary>
    public static class MeUserPermissionExtensions
    {
        /// <summary>
        /// Is Equivalent.
        /// Compares two permissions using their location and userRole properties to determine if they're equivalent.
        /// </summary>
        /// <param name="permissionA">First permission to compare.</param>
        /// <param name="permissionB">Second permission to compare.</param>
        /// <returns>Whether they're Equivalent.</returns>
        public static bool IsEquivalent(this MEUserPermission permissionA, MEUserPermission permissionB)
        {
            return permissionA.LocationId == permissionB.LocationId && permissionA.UserRole == permissionB.UserRole;
        }

        /// <summary>
        /// Get Location Name.
        /// </summary>
        /// <param name="permission">Permission to get location name for.</param>
        /// <param name="service">The locations retrieval by id service.</param>
        /// <returns>Single location</returns>
        public static async Task<Location> GetLocationName(
            this MEUserPermission permission,
            IAsyncQueryHandler<LocationsRetrievalByIdQuery, IEnumerable<Location>> service)
        {
            var locationIds = new List<string>
            {
                permission.LocationId
            };

            var locations = await service.Handle(new LocationsRetrievalByIdQuery(true, locationIds));

            return locations.FirstOrDefault();
        }
    }
}
