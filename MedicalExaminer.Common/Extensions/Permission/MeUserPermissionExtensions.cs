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

        public static Task<IEnumerable<Location>> GetLocationName(this IEnumerable<MEUserPermission> permissions, IAsyncQueryHandler<LocationsRetrievalByQuery, IEnumerable<Location>> service)
        {
            var uniqueLocationIds = permissions.Select(x => x.LocationId).Distinct();
            return service.Handle(new LocationsRetrievalByQuery(null, null, false, uniqueLocationIds));
        }

        public static Task<IEnumerable<Location>> GetLocationName(this MedicalExaminer.Models.MeUser user, IAsyncQueryHandler<LocationsRetrievalByQuery, IEnumerable<Location>> service)
        {
            var uniqueLocationIds = user.Permissions.Select(x => x.LocationId).Distinct();
            return service.Handle(new LocationsRetrievalByQuery(null, null, false, uniqueLocationIds));
        }

        public static Task<IEnumerable<Location>> GetLocationName(this IEnumerable<MedicalExaminer.Models.MeUser> users, IAsyncQueryHandler<LocationsRetrievalByQuery, IEnumerable<Location>> service)
        {
            var usersPermissions = users.SelectMany(usr => usr.Permissions);
            var uniqueLocationIds = usersPermissions.Select(x => x.PermissionId).Distinct();
            return service.Handle(new LocationsRetrievalByQuery(null, null, false, uniqueLocationIds));
        }
    }
}
