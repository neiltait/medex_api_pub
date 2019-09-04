using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Location;

namespace MedicalExaminer.Common.Services.Location
{
    /// <summary>
    /// Locations Parents Query Service.
    /// </summary>
    public class LocationsParentsQueryService : QueryHandler<LocationsParentsQuery, IDictionary<string,IEnumerable<Models.Location>>>
    {
        /// <summary>
        /// Maximum number of "recursive" loops before exiting.
        /// </summary>
        /// <remarks>Shouldn't be possible if locations are automatically configured, but better not left to chance.</remarks>
        private const int MaxLoopIterations = 10;

        /// <summary>
        /// Initialise a new instance of <see cref="LocationParentsQueryService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">Connection Settings</param>
        public LocationsParentsQueryService(
            IDatabaseAccess databaseAccess,
            ILocationConnectionSettings connectionSettings)
            : base(databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override async Task<IDictionary<string,IEnumerable<Models.Location>>> Handle(LocationsParentsQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            // Initialise with the key and value the same
            var locationIds = param.LocationIds.ToList();

            var maxLoops = MaxLoopIterations;
            var result = param.LocationIds.Distinct().ToDictionary(
                k => k,
                v => new List<Models.Location>());

            while (locationIds.Any() && (maxLoops--) > 0)
            {
                var ids = locationIds.ToList();

                var items = (await DatabaseAccess.GetItemsAsync(
                    ConnectionSettings,
                    (Models.Location location) => ids.Contains(location.LocationId))).ToList();

                if (!items.Any())
                {
                    break;
                }

                foreach (var item in items)
                {
                    var keys = result
                        .Where(kvp =>
                            (!kvp.Value.Any() && kvp.Key == item.LocationId)
                            || (kvp.Value.Any() && kvp.Value.Last().ParentId == item.LocationId))
                        .Select(kvp => kvp.Key);

                    foreach (var key in keys)
                    {
                        result[key].Add(item);
                    }
                }

                locationIds = result
                    .Where(r => r.Value.Count > 0)
                    .Select(r => r.Value.Last().ParentId).ToList();
            }

            // Stop the value being a list
            return result.ToDictionary(
                k => k.Key,
                v => v.Value.AsEnumerable());
        }
    }
}
