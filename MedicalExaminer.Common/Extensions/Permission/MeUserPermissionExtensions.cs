using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Extensions.Permission
{
    public static class MeUserPermissionExtensions
    {
        public static bool IsEquivalent(this MEUserPermission permissionA, MEUserPermission permissionB)
        {
            return permissionA.LocationId == permissionB.LocationId && permissionA.UserRole == permissionB.UserRole;
        }

        public static Task<Location> GetLocationName(this MEUserPermission permission, IAsyncQueryHandler<LocationRetrievalByIdQuery, Location> service)
        {
            return service.Handle(new LocationRetrievalByIdQuery(permission.LocationId));
        }

        public static Task<IEnumerable<Location>> GetUniqueLocationNames(this IEnumerable<MEUserPermission> permissions, IAsyncQueryHandler<LocationsRetrievalByQuery, IEnumerable<Location>> service)
        {
            var uniqueLocationIds = permissions.Select(x => x.LocationId).Distinct();
            return service.Handle(new LocationsRetrievalByQuery(null, null, false, uniqueLocationIds));
        }

        
    }
}
