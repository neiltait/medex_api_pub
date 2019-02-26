using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medical_Examiner_API.Models.V1.Locations
{
    public class LocationItem
    {
        /// <summary>
        /// The Location Identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Location Code.
        /// </summary>
        public string Code { get; set; }
    }
}
