using System.ComponentModel.DataAnnotations;

namespace MedicalExaminer.API.Models.v1.CaseOutcome
{
    /// <summary>
    /// Put Cremation Fee Waive Request
    /// </summary>
    public class PutCremationFeeWaiveRequest
    {
        /// <summary>
        /// Waive Fee
        /// </summary>
        [Required]
        public bool WaiveFee { get; set; }
    }
}
