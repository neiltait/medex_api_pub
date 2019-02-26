using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medical_Examiner_API.Models.V1.Locations
{
    /// <inheritdoc />
    /// <summary>
    /// Response object for a list of locations.
    /// </summary>
    public class GetLocationsResponse : ResponseBase
    {

        /// <summary>
        /// List of Locations.
        /// </summary>
        public IEnumerable<LocationItem> Locations { get; set; }

    }
}
