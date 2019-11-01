using MedicalExaminer.API.Attributes;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.CaseOutcome
{
    public class PutOutstandingCaseItemsRequest
    {
        public bool? MccdIssued { get; set; }

        public CremationFormStatus? CremationFormStatus { get; set; }

        [RequiredIfAttributesMatch(nameof(CremationFormStatus), "Yes")]
        public bool? WaiveFee { get; set; }

        public GPNotified? GpNotifiedStatus { get; set; }
    }
}
