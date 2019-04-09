using MedicalExaminer.Models;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class CaseBreakDownItem
    {
        public PatientDeathEventItem PatientDeathEvent { get; set; }
        /// <summary>
        /// Case breakdown other events
        /// </summary>
        public EventContainerItem<OtherEvent> OtherEvents { get; set; }

        /// <summary>
        /// Case breakdown pre scrutiny notes
        /// </summary>
        public EventContainerItem<PreScrutinyEvent> PreScrutiny { get; set; }

        /// <summary>
        /// Case breakdown berevaed discussion notes
        /// </summary>
        public EventContainerItem<BereavedDiscussionEvent> BereavedDiscussion { get; set; }

        /// <summary>
        /// Case breakdown meo summary notes
        /// </summary>
        public EventContainerItem<MeoSummaryEvent> MeoSummary { get; set; }

        /// <summary>
        /// Case breakdown qap discussion
        /// </summary>
        public EventContainerItem<QapDiscussionEvent> QapDiscussion { get; set; }

        /// <summary>
        /// case breakdown medical history
        /// </summary>
        public EventContainerItem<MedicalHistoryEvent> MedicalHistory { get; set; }

        /// <summary>
        /// case breakdown admission notes
        /// </summary>
        public EventContainerItem<AdmissionEvent> AdmissionNotes { get; set; }
    }
}
