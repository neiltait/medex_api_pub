using System;
using System.Collections.Generic;
using System.Text;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.CaseBreakdown
{
    public class CreateOtherCaseEventQuery : IQuery<CaseEvent>
    {
        public Models.CaseEvent CaseEvent { get; }

        public CreateOtherCaseEventQuery(Models.CaseEvent caseEvent)
        {
            CaseEvent = caseEvent;
        }
    }
}
