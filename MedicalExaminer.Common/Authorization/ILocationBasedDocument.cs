using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.Common.Authorization
{
    /// <summary>
    /// Location Based Document Interface.
    /// </summary>
    public interface ILocationBasedDocument
    {
        /// <summary>
        /// National Location Id.
        /// </summary>
        string NationalLocationId { get; }

        /// <summary>
        /// Region Location Id.
        /// </summary>
        string RegionLocationId { get; }

        /// <summary>
        /// Trust Location Id.
        /// </summary>
        string TrustLocationId { get; }

        /// <summary>
        /// Site Location Id.
        /// </summary>
        string SiteLocationId { get; }
    }
}
