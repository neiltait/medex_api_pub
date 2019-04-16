using MedicalExaminer.API.Models.v1.Examinations;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class PutCaseBreakdownEventResponse : ResponseBase
    {
        /// <summary>
        /// Patient Details Header
        /// </summary>
        public PatientCardItem Header { get; set; }

        /// <summary>
        /// The id of the event
        /// </summary>
        public string EventId { get; set; }
    }
}
