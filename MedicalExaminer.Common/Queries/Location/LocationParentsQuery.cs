using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.Common.Queries.Location
{
    /// <summary>
    /// Location Prents Query.
    /// </summary>
    public class LocationParentsQuery: IQuery<IEnumerable<Models.Location>>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="LocationParentsQuery"/>.
        /// </summary>
        /// <param name="locationId">Location Id.</param>
        public LocationParentsQuery(string locationId)
        {
            LocationId = locationId;
        }

        /// <summary>
        /// Location Id.
        /// </summary>
        public string LocationId { get; }
    }
}
