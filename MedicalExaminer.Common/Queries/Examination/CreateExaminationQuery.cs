using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.Examination
{
    public class CreateExaminationQuery : IQuery<Models.Examination>
    {
        public CreateExaminationQuery(Models.Examination examination, MeUser user)
        {
            Examination = examination;
            User = user;
        }

        public Models.Examination Examination { get; }

        public MeUser User { get; }
    }
}