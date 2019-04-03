using System;
using System.Collections.Generic;
using System.Text;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.CaseBreakdown
{
    public class CreateQapDiscussionEventQuery : IQuery<string>
    {
        public Models.QapDiscussionEvent QapDiscussionEvent { get; }

        public string CaseId { get; }

        public CreateQapDiscussionEventQuery(string caseId, QapDiscussionEvent qapDiscussionEvent)
        {
            CaseId = caseId;
            QapDiscussionEvent = qapDiscussionEvent;
        }

    }
}
