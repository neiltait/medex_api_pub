using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Models.V1.Locations
{
    public class LocationItem
    {
        /// <summary>
        /// The Location Identifier.
        /// </summary>
        public string LocationId { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Location Code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The LocationId of the location's parent location
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// Indicates if location is site, trust, region or national
        /// </summary>
        public LocationType Type { get; set; }

        /// <summary>
        /// Indicates if location is available for use
        /// </summary>
        public bool IsActive { get; set; }
    }
}
