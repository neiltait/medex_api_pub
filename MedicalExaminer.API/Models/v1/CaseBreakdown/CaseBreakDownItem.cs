using MedicalExaminer.Models;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class CaseBreakDownItem
    {
        public PatientDeathEventItem PatientDeathEvent { get; set; }
        /// <summary>
        /// Case breakdown other events
        /// </summary>
        public EventContainerItem<OtherEventItem, object> OtherEvents { get; set; }

        /// <summary>
        /// Case breakdown pre scrutiny notes
        /// </summary>
        public EventContainerItem<PreScrutinyEventItem, object> PreScrutiny { get; set; }

        /// <summary>
        /// Case breakdown berevaed discussion notes
        /// </summary>
        public EventContainerItem<BereavedDiscussionEventItem, object> BereavedDiscussion { get; set; }

        /// <summary>
        /// Case breakdown meo summary notes
        /// </summary>
        public EventContainerItem<MeoSummaryEventItem, object> MeoSummary { get; set; }

        /// <summary>
        /// Case breakdown qap discussion
        /// </summary>
        public EventContainerItem<QapDiscussionEventItem, object> QapDiscussion { get; set; }

        /// <summary>
        /// case breakdown medical history
        /// </summary>
        public EventContainerItem<MedicalHistoryEventItem, object> MedicalHistory { get; set; }

        /// <summary>
        /// case breakdown admission notes
        /// </summary>
        public EventContainerItem<AdmissionEventItem, object> AdmissionNotes { get; set; }

        public CaseClosedEventItem CaseClosed { get; set; }
    }
}
