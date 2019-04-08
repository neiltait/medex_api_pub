using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Extensions.Examination
{
    /// <summary>
    /// Examination Extensions.
    /// </summary>
    public static class ExaminationExtensions
    {
        /// <summary>
        /// Update Location Cache on Examination
        /// </summary>
        /// <param name="examination">The Examination to Update</param>
        /// <param name="locations">Locations.</param>
        public static void UpdateLocationCache(this Models.Examination examination, IEnumerable<Location> locations)
        {
            foreach (var location in locations)
            {
                switch (location.Type)
                {
                    case LocationType.National:
                        examination.NationalLocationId = location.LocationId;
                        break;
                    case LocationType.Region:
                        examination.RegionLocationId = location.LocationId;
                        break;
                    case LocationType.Trust:
                        examination.TrustLocationId = location.LocationId;
                        break;
                    case LocationType.Site:
                        examination.SiteLocationId = location.LocationId;
                        break;
                }
            }
        }
    }
}
