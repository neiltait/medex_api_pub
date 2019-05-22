using System;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Models
{
    public class CaseOutcome
    {
        public CaseOutcomeSummary? CaseOutcomeSummary { get; set; }

        public BereavedDiscussionOutcome? OutcomeOfRepresentativeDiscussion { get; set; }

        public OverallOutcomeOfPreScrutiny? OutcomeOfPrescrutiny { get; set; }

        public QapDiscussionOutcome? OutcomeQapDiscussion { get; set; }

        public bool CaseCompleted { get; set; }

        public DateTime? ScrutinyConfirmedOn { get; set; }

        public string CaseMedicalExaminerFullName { get; set; }

        public OutstandingCaseItems OutstandingCaseItems { get; set; }
    }
}
