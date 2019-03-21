namespace MedicalExaminer.Common.Queries.Examination
{
    public class ExaminationRetrievalQuery : IQuery<Models.Examination>
    {
        public ExaminationRetrievalQuery(string examinationId)
        {
            ExaminationId = examinationId;
        }

        public string ExaminationId { get; }
    }
}