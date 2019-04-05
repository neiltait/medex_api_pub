namespace MedicalExaminer.Models
{
    public class CaseBreakDown
    {
        public BaseEventContainter<OtherEvent> OtherEvents { get; set; } = new OtherEventContainer();
        public BaseEventContainter<PreScrutinyEvent> PreScrutiny { get; set; } = new PreScrutinyEventContainer();

        public BaseEventContainter<BereavedDiscussionEvent> BereavedDiscussion { get; set; } = new BereavedDiscussionEventContainer();

        public BaseEventContainter<MeoSummaryEvent> MeoSummary { get; set; } = new MeoSummaryEventContainer();

        public BaseEventContainter<QapDiscussionEvent> QapDiscussion { get; set; } = new QapDiscussionEventContainer();
    }
}
