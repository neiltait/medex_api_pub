using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.CaseOutcome
{
    public class PutOutstandingCaseItemsRequest
    {

        public bool? MCCDIssued { get; set; }

        public CremationFormStatus? CremationFormStatus { get; set; }

        public GPNotified? GPNotifiedStatus { get; set; }
    }
}
