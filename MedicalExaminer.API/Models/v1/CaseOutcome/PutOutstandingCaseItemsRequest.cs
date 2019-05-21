using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.CaseOutcome
{
    public class PutOutstandingCaseItemsRequest
    {
        public bool? MccdIssued { get; set; }

        public CremationFormStatus? CremationFormStatus { get; set; }

        public GPNotified? GpNotifiedStatus { get; set; }
    }
}
