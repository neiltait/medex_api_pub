using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.CaseOutcome
{
    public class CoronerReferralQuery : IQuery<string>
    {
        public string ExaminationId { get; }

        public MeUser User { get; }

        public CoronerReferralQuery(string examinationId, MeUser user)
        {
            ExaminationId = examinationId;
            User = user;
        }
    }
}
