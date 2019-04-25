using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.CaseOutcome
{
    public class ConfirmationOfScrutinyQuery : IQuery<Models.Examination>
    {
        public ConfirmationOfScrutinyQuery(string examinationId, MeUser user)
        {
            ExaminationId = examinationId;
            User = user;
        }

        public string ExaminationId { get; }

        public MeUser User { get; }
    }
}
