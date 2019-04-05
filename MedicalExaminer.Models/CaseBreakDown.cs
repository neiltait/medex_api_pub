namespace MedicalExaminer.Models
{
    public class CaseBreakDown
    {
        public OtherEventContainer OtherEvents { get; set; } = new OtherEventContainer();
        public BaseEventContainter<PreScrutinyEvent> PreScrutiny { get; set; } = new PreScrutinyEventContainer();

        public BaseEventContainter<BereavedDiscussionEvent> BereavedDiscussion { get; set; } = new BereavedDiscussionEventContainer();

        public BaseEventContainter<MeoSummaryEvent> MeoSummary { get; set; } = new MeoSummaryEventContainer();

        public BaseEventContainter<QapDiscussionEvent> QapDiscussion { get; set; } = new QapDiscussionEventContainer();

        public BaseEventContainter<MedicalHistoryEvent> MedicalHistory { get; set; } = new MedicalHistoryEventContainer();

        public BaseEventContainter<AdmissionEvent> AdmissionNotes { get; set; } = new AdmissionNotesEventContainer();

    }
}
