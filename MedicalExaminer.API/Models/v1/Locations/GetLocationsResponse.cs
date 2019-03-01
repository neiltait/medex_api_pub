using MedicalExaminer.API.Models.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
