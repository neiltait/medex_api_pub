using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Microsoft.ApplicationInsights;

namespace MedicalExaminer.Common.Extensions.Models
{
    /// <summary>
    /// Location Based Document Extensions.
    /// </summary>
    public static class LocationPathExtensions
    {
        /// <summary>
        /// Convert a list of locations to a locationPath directly.
        /// </summary>
        /// <remarks>Object should ideally inherit from <see cref="ILocationPath"/>, but if it doesn't or can't. Then use this when you need location path.</remarks>
        /// <param name="locations">List of locations.</param>
        /// <returns>Location based locationPath.</returns>
        public static LocationPath ToLocationPath(this IEnumerable<Location> locations)
        {
            var document = new LocationPath();
            document.UpdateLocationPath(locations);
            return document;
        }

        /// <summary>
        /// Update Location Path from a list of Locations.
        /// </summary>
        /// <param name="locationPath">The Location Path.</param>
        /// <param name="locations">Locations.</param>
        public static void UpdateLocationPath(this ILocationPath locationPath, IEnumerable<Location> locations)
        {
            foreach (var location in locations)
            {
                switch (location.Type)
                {
                    case LocationType.National:
                        locationPath.NationalLocationId = location.LocationId;
                        break;
                    case LocationType.Region:
                        locationPath.RegionLocationId = location.LocationId;
                        break;
                    case LocationType.Trust:
                        locationPath.TrustLocationId = location.LocationId;
                        break;
                    case LocationType.Site:
                        locationPath.SiteLocationId = location.LocationId;
                        break;
                }
            }
        }
    }
}
