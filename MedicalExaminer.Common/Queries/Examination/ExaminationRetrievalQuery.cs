using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.Examination
{
    public class ExaminationRetrievalQuery : IQuery<Models.Examination>
    {
        public ExaminationRetrievalQuery(string examinationId, MeUser user)
        {
            ExaminationId = examinationId;
            User = user;
        }

        public  string ExaminationId { get; }
        public MeUser User { get; }
    }
}