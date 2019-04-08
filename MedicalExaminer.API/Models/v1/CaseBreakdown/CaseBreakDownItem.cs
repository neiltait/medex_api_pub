using MedicalExaminer.Models;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class CaseBreakDownItem
    {

        public EventContainerItem<OtherEvent> OtherEvents { get; set; }

        public EventContainerItem<PreScrutinyEvent> PreScrutiny { get; set; }

        public EventContainerItem<BereavedDiscussionEvent> BereavedDiscussion { get; set; }

        public EventContainerItem<MeoSummaryEvent> MeoSummary { get; set; }

        public EventContainerItem<QapDiscussionEvent> QapDiscussion { get; set; }

        public EventContainerItem<MedicalHistoryEvent> MedicalHistory { get; set; }

        public EventContainerItem<AdmissionEvent> AdmissionNotes { get; set; }
    }
}
