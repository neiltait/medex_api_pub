namespace MedicalExaminer.Common.Queries.Examination
{
    public class CreateExaminationQuery : IQuery<string>
    {
        public Models.Examination Examination { get; }
        public CreateExaminationQuery(Models.Examination examination)
        {
            Examination = examination;
        }
    }
}
