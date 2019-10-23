using MedicalExaminer.API.Models.v1.Examinations;

namespace MedicalExaminer.API.Models.v1.CaseOutcome
{
    /// <summary>
    /// Put Cremation Fee Waive Response
    /// </summary>
    public class PutCremationFeeWaiveResponse : ResponseBase
    {
        /// <summary>
        /// Patient card header
        /// </summary>
        public PatientCardItem Header { get; set; }

        /// <summary>
        /// Should the cremation fee be waived?
        /// </summary>
        public bool? WaiveFee { get; set; }
    }
}
