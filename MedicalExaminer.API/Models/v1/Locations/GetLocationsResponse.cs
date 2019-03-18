using System.Collections.Generic;
using MedicalExaminer.API.Models.v1;

namespace MedicalExaminer.Models.V1.Locations
{
    /// <inheritdoc />
    /// <summary>
    ///     Response object for a list of locations.
    /// </summary>
    public class GetLocationsResponse : ResponseBase
    {
        /// <summary>
        ///     List of Locations.
        /// </summary>
        public IEnumerable<LocationItem> Locations { get; set; }
    }
}