using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.Models.Enums;
using System;

namespace MedicalExaminer.API.Models.v1.CaseOutcome
{
    public class GetCaseOutcomeResponse
    {
        public PatientCardItem CaseHeader { get; set; }
        public CaseOutcomeSummary? CaseOutcomeSummary { get; set; }
        public BereavedDiscussionOutcome? OutcomeOfRepresentativeDiscussion { get; set; }
        public OverallOutcomeOfPreScrutiny? OutcomeOfPrescrutiny { get; set; }
        public QapDiscussionOutcome? OutcomeQapDiscussion { get; set; }
        public bool CaseOpen { get; set; }
        public DateTime? ScrutinyConfirmedOn { get; set; }
        public string CaseMedicalExaminerFullName { get; set; }
        public bool? MCCDIssed { get; set; }
        public CremationFormStatus? CremationFormStatus { get; set; }
        public GPNotified? GPNotifedStatus { get; set; }
    }
}
