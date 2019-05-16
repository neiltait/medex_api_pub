namespace MedicalExaminer.Common.Queries.CaseBreakdown
{
    public class EventCreationResult
    {
        public EventCreationResult(string eventId, Models.Examination examination)
        {
            EventId = eventId;
            Examination = examination;
        }

        public string EventId { get; set; }

        public Models.Examination Examination { get; set; }
    }
}
