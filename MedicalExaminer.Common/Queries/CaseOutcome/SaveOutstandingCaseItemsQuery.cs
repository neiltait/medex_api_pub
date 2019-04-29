using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.CaseOutcome
{
    public class SaveOutstandingCaseItemsQuery : IQuery<string>
    {
        public string ExaminationId { get; set; }

        public Models.CaseOutcome CaseOutcome { get; }

        public MeUser User { get; }

        public SaveOutstandingCaseItemsQuery(string examinationId, Models.CaseOutcome caseOutcome, MeUser user)
        {
            ExaminationId = examinationId;
            CaseOutcome = caseOutcome;
            User = user;
        }
    }
}
