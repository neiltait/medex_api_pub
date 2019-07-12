using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.Common.Queries.Examination
{
    public class ExaminationByNhsNumberRetrievalQuery : IQuery<Models.Examination>
    {
        public string NhsNumber { get; private set; }

        public ExaminationByNhsNumberRetrievalQuery(string nhsNumber)
        {
            NhsNumber = nhsNumber;
        }
    }
}
