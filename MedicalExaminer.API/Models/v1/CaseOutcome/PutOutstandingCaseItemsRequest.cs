using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.CaseOutcome
{
    public class PutOutstandingCaseItemsRequest
    {
        public bool? MCCDIssed { get; set; }
        public CremationFormStatus? CremationFormStatus { get; set; }
        public GPNotified? GPNotifedStatus { get; set; }
    }
}
