using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.CaseOutcome
{
    public class SaveOutstandingCaseItemsQuery : IQuery<string>
    {
        public string ExaminationId { get; set; }

        public Models.OutstandingCaseItems OutstandingCaseItems { get; }

        public MeUser User { get; }

        public SaveOutstandingCaseItemsQuery(string examinationId, Models.OutstandingCaseItems outstandingCaseItems, MeUser user)
        {
            ExaminationId = examinationId;
            OutstandingCaseItems = outstandingCaseItems;
            User = user;
        }
    }
}
