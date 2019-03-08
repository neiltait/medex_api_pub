namespace MedicalExaminer.Common.Queries.Examination
{
    public class ExaminationRetrievalQuery : IQuery<Models.Examination>
    {
        public string ExaminationId { get; }
        public ExaminationRetrievalQuery(string examinationId)
        {
            ExaminationId = examinationId;
        }
    }
}
