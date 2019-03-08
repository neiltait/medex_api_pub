namespace MedicalExaminer.Common.Queries.Examination
{
    public class CreateExaminationQuery : IQuery<string>
    {
        public Models.IExamination Examination { get; }
        public CreateExaminationQuery(Models.IExamination examination)
        {
            Examination = examination;
        }
    }
}
