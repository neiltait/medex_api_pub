using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalExaminer.API.Models.v1.CaseOutcome
{
    /// <summary>
    /// Put Void Case Request
    /// </summary>
    public class PutVoidCaseRequest
    {
        /// <summary>
        /// Reason for voiding the examination
        /// </summary>
        public string VoidReason { get; set; }
    }
}
