using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.Models.Enums;
using System;

namespace MedicalExaminer.API.Models.v1.CaseOutcome
{
    public class GetCaseOutcomeResponse : ResponseBase
    {
        public PatientCardItem Header { get; set; }

        public CaseOutcomeSummary? CaseOutcomeSummary { get; set; }

        public BereavedDiscussionOutcome? OutcomeOfRepresentativeDiscussion { get; set; }

        public OverallOutcomeOfPreScrutiny? OutcomeOfPrescrutiny { get; set; }

        public QapDiscussionOutcome? OutcomeQapDiscussion { get; set; }

        public bool CaseCompleted { get; set; }

        public bool IsVoid { get; set; }

        public DateTime? VoidedDate { get; set; }

        public DateTime? ScrutinyConfirmedOn { get; set; }

        public bool CoronerReferralSent { get; set; }

        public string CaseMedicalExaminerFullName { get; set; }

        public string CaseMedicalExaminerGmcNumber { get; set; }

        public string CaseMedicalExaminerId { get; set; }

        public bool? MccdIssued { get; set; }

        public CremationFormStatus? CremationFormStatus { get; set; }

        public GPNotified? GpNotifiedStatus { get; set; }

        public bool? WaiveFee { get; set; }

        public DateTime? DateCaseClosed { get; set; }
    }
}
