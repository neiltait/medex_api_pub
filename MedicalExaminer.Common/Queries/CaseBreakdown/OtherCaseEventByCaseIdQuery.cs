using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.Common.Queries.CaseBreakdown
{
    public class OtherCaseEventByCaseIdQuery : IQuery<Models.EventNote>
    {
        public readonly string EventId;

        public OtherCaseEventByCaseIdQuery(string eventId)
        {
            EventId = eventId;
        }
    }
}
