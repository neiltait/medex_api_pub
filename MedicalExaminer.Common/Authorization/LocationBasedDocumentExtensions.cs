using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.Common.Authorization
{
    public static class LocationBasedDocumentExtensions
    {
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
