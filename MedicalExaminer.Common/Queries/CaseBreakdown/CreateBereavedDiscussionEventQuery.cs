using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.CaseBreakdown
{
    public class CreateBereavedDiscussionEventQuery : IQuery<string>
    {
        public Models.BereavedDiscussionEvent BereavedDiscussionEvent { get; }

        public string CaseId { get; }

        public CreateBereavedDiscussionEventQuery(string caseId, BereavedDiscussionEvent bereavedDiscussionEvent)
        {
            CaseId = caseId;
            BereavedDiscussionEvent = bereavedDiscussionEvent;
        }
    }
}
