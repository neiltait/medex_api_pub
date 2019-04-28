using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.CaseOutcome
{
    public class SaveOutstandingCaseItemsQuery : IQuery<string>
    {
        public Models.Examination Examination { get; }

        public MeUser User { get; }

        public SaveOutstandingCaseItemsQuery(Models.Examination examination, MeUser user)
        {
            Examination = examination;
            User = user;
        }
    }
}
