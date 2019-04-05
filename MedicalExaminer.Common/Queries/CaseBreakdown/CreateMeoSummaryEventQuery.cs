using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.CaseBreakdown
{
    public class CreateMeoSummaryEventQuery : IQuery<string>
    {
        public MeoSummaryEvent MeoSummaryEvent { get; }

        public string CaseId { get; }

        public CreateMeoSummaryEventQuery(string caseId, MeoSummaryEvent meoSummaryEvent)
        {
            CaseId = caseId;
            MeoSummaryEvent = meoSummaryEvent;
        }

    }
}
