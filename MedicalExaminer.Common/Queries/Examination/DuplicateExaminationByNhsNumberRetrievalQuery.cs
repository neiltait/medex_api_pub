namespace MedicalExaminer.Common.Queries.Examination
{
    public class DuplicateExaminationByNhsNumberRetrievalQuery : IQuery<Models.Examination>
    {
        public string NhsNumber { get; private set; }

        public DuplicateExaminationByNhsNumberRetrievalQuery(string nhsNumber)
        {
            NhsNumber = nhsNumber;
        }
    }
}
