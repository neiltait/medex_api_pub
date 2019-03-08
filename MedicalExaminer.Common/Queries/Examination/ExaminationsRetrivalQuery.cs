using System.Collections.Generic;

namespace MedicalExaminer.Common.Queries.Examination
{
    public class ExaminationsRetrivalQuery : IQuery<IEnumerable<Models.Examination>>
    {
        public ExaminationsRetrivalQuery()
        {
            QueryString = "SELECT* FROM Examinations";
        }
        public string QueryString { get; }
    }
}
