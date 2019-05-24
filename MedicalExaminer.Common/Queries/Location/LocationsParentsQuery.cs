using System.Collections.Generic;

namespace MedicalExaminer.Common.Queries.Location
{
    /// <summary>
    /// Locations Parents Query.
    /// </summary>
    public class LocationsParentsQuery: IQuery<IDictionary<string, IEnumerable<Models.Location>>>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="LocationParentsQuery"/>.
        /// </summary>
        /// <param name="locationIds">Lis of location Ids</param>
        public LocationsParentsQuery(IEnumerable<string> locationIds)
        {
            LocationIds = locationIds;
        }

        /// <summary>
        /// Location Id.
        /// </summary>
        public IEnumerable<string> LocationIds { get; }
    }
}
