namespace MedicalExaminer.Common.Queries.Examination
{
    public class ExaminationRetrivalQuery : IQuery<Models.IExamination>
    {
        public string ExaminationId { get; }
        public ExaminationRetrivalQuery(string examinationId)
        {
            ExaminationId = examinationId;
        }
    }
}
