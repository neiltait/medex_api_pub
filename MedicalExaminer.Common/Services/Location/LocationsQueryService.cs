using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries;
using MedicalExaminer.Common.Queries.Location;

namespace MedicalExaminer.Common.Services.Location
{
    /// <summary>
    /// Location Query Service.
    /// </summary>
    /// <inheritdoc/>
    public class LocationsQueryService : QueryHandler<LocationsRetrievalByQuery, IEnumerable<Models.Location>>
    {
        /// <summary>
        /// Initialise a new instance of the Location Id Service.
        /// </summary>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">Connection Settings.</param>
        public LocationsQueryService(IDatabaseAccess databaseAccess, ILocationConnectionSettings connectionSettings)
            : base(databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override async Task<IEnumerable<Models.Location>> Handle(LocationsRetrievalByQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var predicate = GetPredicate(param);

            if (param.PermissedLocations != null)
            {
                Expression<Func<Models.Location, bool>> idFilter = l =>
                    param.PermissedLocations.Contains(l.LocationId)
                    || param.PermissedLocations.Contains(l.NationalLocationId)
                    || param.PermissedLocations.Contains(l.RegionLocationId)
                    || param.PermissedLocations.Contains(l.TrustLocationId)
                    || param.PermissedLocations.Contains(l.SiteLocationId);

                predicate = predicate.And(idFilter);
            }

            IEnumerable<Models.Location> result;

            if (param.ForLookup)
            {
                result = await GetItemsAsync<Models.Location>(predicate, location => new
                {
                    name = location.Name,
                    id = location.LocationId
                });
            }
            else
            {
                result = await GetItemsAsync(predicate);
            }

            return result;
        }

        /// <summary>
        /// Get Predicate.
        /// </summary>
        /// <param name="param">Params.</param>
        /// <returns>The Predicate.</returns>
        private Expression<Func<Models.Location, bool>> GetPredicate(LocationsRetrievalByQuery param)
        {
            Expression<Func<Models.Location, bool>> predicate = x => true;

            if (param.Name != null)
            {
                predicate = predicate.And(x => x.Name == param.Name);
            }

            if (param.ParentId != null)
            {
                // An empty guid takes the place of null allowing us to have no filter, or filter by null.
                var parentId = param.ParentId == Guid.Empty.ToString()
                    ? null
                    : param.ParentId;
                predicate = predicate.And(x => x.ParentId == parentId);
            }

            if (param.OnlyMeOffices)
            {
                predicate = predicate.And(x => x.IsMeOffice);
            }

            return predicate;
        }
    }
}