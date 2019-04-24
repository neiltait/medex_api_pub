using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Permission;

namespace MedicalExaminer.Common.Services.Permission
{
    /// <summary>
    /// Permissions Retrieval By Locations And User Service.
    /// </summary>
    public class PermissionsRetrievalByLocationsAndUserService : QueryHandler<PermissionsRetrievalByLocationsAndUserServiceQuery, IEnumerable<Models.Permission>>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="PermissionsRetrievalByLocationsAndUserService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">Connection Settings.</param>
        public PermissionsRetrievalByLocationsAndUserService(
            IDatabaseAccess databaseAccess,
            IConnectionSettings connectionSettings)
            : base(databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override Task<IEnumerable<Models.Permission>> Handle(PermissionsRetrievalByLocationsAndUserServiceQuery param)
        {
            Expression<Func<Models.Permission, bool>> predicate = p =>
                param.Locations.Contains(p.LocationId) && p.UserId == param.MeUserId;

            return GetItemsAsync(predicate);
        }
    }
}
