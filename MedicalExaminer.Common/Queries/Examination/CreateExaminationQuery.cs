namespace MedicalExaminer.Common.Queries.Examination
{
    public class CreateExaminationQuery : IQuery<Models.Examination>
    {
        public CreateExaminationQuery(Models.Examination examination, string userId)
        {
            Examination = examination;
            UserId = userId;
        }

        public Models.Examination Examination { get; }
        public string UserId { get; }
    }
}