using MedicalExaminer.API.Models.v1.Examinations;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class GetCaseBreakdownResponse : ResponseBase
    {
        /// <summary>
        /// Patient Details Header
        /// </summary>
        public PatientCardItem Header { get; set; }

        /// <summary>
        /// Case breakdown item which consists of different types of events
        /// </summary>
        public CaseBreakDownItem CaseBreakdown { get; set; }
    }
}
