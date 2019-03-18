namespace MedicalExaminer.Common.Queries.Examination
{
    public class ExaminationPriorityCalculationQuery : IQuery<Models.Examination>
    {
        public Models.Examination Examination { get; }

        public ExaminationPriorityCalculationQuery(Models.Examination examination)
        {
            Examination = examination;
        }
    }
}
