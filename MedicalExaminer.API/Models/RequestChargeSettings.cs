using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalExaminer.API.Models
{
    /// <summary>
    /// Feature settings
    /// </summary>
    public class RequestChargeSettings
    {
        /// <summary>
        /// Show request charge in header.
        /// </summary>
        public bool ShowFullBreakdownInHeader { get; set; }
    }
}
