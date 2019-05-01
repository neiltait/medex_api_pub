using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.CaseOutcome
{
    public class CloseCaseQuery : IQuery<string>
    {
        public string ExaminationId { get; }

        public MeUser User { get; }

        public CloseCaseQuery(string examinationId, MeUser user)
        {
            ExaminationId = examinationId;
            User = user;
        }
    }
}
