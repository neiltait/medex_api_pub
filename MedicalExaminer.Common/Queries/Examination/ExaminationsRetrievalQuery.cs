using System.Collections.Generic;

namespace MedicalExaminer.Common.Queries.Examination
{
    public class ExaminationsRetrievalQuery : IQuery<IEnumerable<Models.Examination>>
    {
        public ExaminationsRetrievalQuery()
        {
            QueryString = "SELECT* FROM Examinations";
        }
        public string QueryString { get; }
    }
}
