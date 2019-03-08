using MedicalExaminer.API.Models.v1;
using System.Collections.Generic;

namespace MedicalExaminer.Models.V1.Locations
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
