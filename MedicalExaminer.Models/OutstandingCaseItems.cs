using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Models
{
    public class OutstandingCaseItems
    {
        public bool? MCCDIssued { get; set; }

        public CremationFormStatus? CremationFormStatus { get; set; }

        public GPNotified? GPNotifiedStatus { get; set; }
    }
}
