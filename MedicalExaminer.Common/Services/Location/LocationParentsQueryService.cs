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
    /// Location Parents Query Service.
    /// </summary>
    public class LocationParentsQueryService : QueryHandler<LocationParentsQuery, IEnumerable<Models.Location>>
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
        public LocationParentsQueryService(
            IDatabaseAccess databaseAccess,
            ILocationConnectionSettings connectionSettings)
            : base(databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override async Task<IEnumerable<Models.Location>> Handle(LocationParentsQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var locationId = param.LocationId;
            var maxLoops = MaxLoopIterations;
            var result = new List<Models.Location>();

            while (locationId != null && (maxLoops--) > 0)
            {
                var id = locationId;

                var item = await DatabaseAccess.GetItemAsync(
                    ConnectionSettings,
                    (Models.Location location) => location.LocationId == id);

                if (item == null)
                {
                    return Enumerable.Empty<Models.Location>();
                }

                result.Add(item);

                locationId = item.ParentId;
            }

            return result.AsEnumerable();
        }
    }
}
