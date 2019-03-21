using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.Common.Queries.CaseBreakdown
{
    public class OtherCaseEventByCaseIdQuery : IQuery<Models.CaseEvent>
    {
        public readonly string CaseEventId;

        public OtherCaseEventByCaseIdQuery(string caseEventId)
        {
            CaseEventId = caseEventId;
        }
    }
}
