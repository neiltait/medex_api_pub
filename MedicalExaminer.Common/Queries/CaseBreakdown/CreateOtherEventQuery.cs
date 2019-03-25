using System;
using System.Collections.Generic;
using System.Text;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.CaseBreakdown
{
    public class CreateOtherEventQuery : IQuery<string>
    {
        public Models.OtherEvent OtherEvent { get; }

        public string CaseId { get; }

        public CreateOtherEventQuery(string caseId, OtherEvent otherEvent)
        {
            CaseId = caseId;
            OtherEvent = otherEvent;
        }
    }
}
