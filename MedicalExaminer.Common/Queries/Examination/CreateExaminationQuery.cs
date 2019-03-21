namespace MedicalExaminer.Common.Queries.Examination
{
    public class CreateExaminationQuery : IQuery<string>
    {
        public CreateExaminationQuery(Models.Examination examination)
        {
            Examination = examination;
        }

        public Models.Examination Examination { get; }
    }
}