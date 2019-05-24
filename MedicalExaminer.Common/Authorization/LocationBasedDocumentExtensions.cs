using System.Collections.Generic;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Authorization
{
    /// <summary>
    /// Location Based Document Extensions.
    /// </summary>
    public static class LocationBasedDocumentExtensions
    {
        /// <summary>
        /// Location Ids.
        /// </summary>
        /// <remarks>Returns a flat array of the 4 locations as a single array.</remarks>
        /// <param name="locationPath">Document.</param>
        /// <returns>List of Locations.</returns>
        public static IEnumerable<string> LocationIds(this ILocationPath locationPath)
        {
            return new[]
            {
                locationPath.NationalLocationId,
                locationPath.RegionLocationId,
                locationPath.TrustLocationId,
                locationPath.SiteLocationId,
            };
        }
    }
}
