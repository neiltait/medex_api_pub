using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.CaseBreakdown
{
    public class CreateEventQuery : IQuery<string>
    {
        public IEvent Event { get; }

        public string CaseId { get; }

        public CreateEventQuery(string caseId, IEvent theEvent)
        {
            CaseId = caseId;
            Event = theEvent;
        }
    }
}
