using System.ComponentModel.DataAnnotations;

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
        [Required]
        public string VoidReason { get; set; }
    }
}
