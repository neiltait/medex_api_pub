using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.Common.Queries.CaseBreakdown
{
    public class OtherCaseEventByCaseIdQuery : IQuery<Models.OtherEvent>
    {
        public readonly string CaseId;

        public OtherCaseEventByCaseIdQuery(string caseId)
        {
            CaseId = caseId;
        }
    }
}
