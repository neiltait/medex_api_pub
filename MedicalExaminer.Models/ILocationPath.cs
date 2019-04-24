using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.Models
{
    /// <summary>
    /// Location Path Interface.
    /// </summary>
    public interface ILocationPath
    {
        /// <summary>
        /// National Location Id.
        /// </summary>
        string NationalLocationId { get; set; }

        /// <summary>
        /// Region Location Id.
        /// </summary>
        string RegionLocationId { get; set; }

        /// <summary>
        /// Trust Location Id.
        /// </summary>
        string TrustLocationId { get; set; }

        /// <summary>
        /// Site Location Id.
        /// </summary>
        string SiteLocationId { get; set; }
    }
}
