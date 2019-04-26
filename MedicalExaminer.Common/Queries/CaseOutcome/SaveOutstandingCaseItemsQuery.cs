using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.CaseOutcome
{
    public class SaveOutstandingCaseItemsQuery : IQuery<string>
    {
        public string ExaminationId { get; }

        public MeUser User { get; }

        public SaveOutstandingCaseItemsQuery(string examinationId, MeUser user)
        {
            ExaminationId = examinationId;
            User = user;
        }
    }
}
