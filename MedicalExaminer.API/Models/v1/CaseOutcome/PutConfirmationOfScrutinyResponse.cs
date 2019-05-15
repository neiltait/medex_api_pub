using System;

namespace MedicalExaminer.API.Models.v1.CaseOutcome
{
    public class PutConfirmationOfScrutinyResponse : ResponseBase
    {
        public DateTime? ScrutinyConfirmedOn { get; set; }
    }
}
