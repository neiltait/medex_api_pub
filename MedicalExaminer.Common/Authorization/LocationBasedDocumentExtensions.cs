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
        /// <param name="locationBasedDocument">Document.</param>
        /// <returns>List of Locations.</returns>
        public static IEnumerable<string> LocationIds(this ILocationBasedDocument locationBasedDocument)
        {
            return new[]
            {
                locationBasedDocument.NationalLocationId,
                locationBasedDocument.RegionLocationId,
                locationBasedDocument.TrustLocationId,
                locationBasedDocument.SiteLocationId,
            };
        }
    }
}
