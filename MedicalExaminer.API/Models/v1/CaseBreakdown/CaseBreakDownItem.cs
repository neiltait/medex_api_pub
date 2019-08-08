using MedicalExaminer.Models;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class CaseBreakDownItem
    {
        public PatientDeathEventItem PatientDeathEvent { get; set; }
        /// <summary>
        /// Case breakdown other events
        /// </summary>
        public EventContainerItem<OtherEventItem, NullPrepopulated> OtherEvents { get; set; }

        /// <summary>
        /// Case breakdown pre scrutiny notes
        /// </summary>
        public EventContainerItem<PreScrutinyEventItem, NullPrepopulated> PreScrutiny { get; set; }

        /// <summary>
        /// Case breakdown berevaed discussion notes
        /// </summary>
        public EventContainerItem<BereavedDiscussionEventItem, BereavedDiscussionPrepopulated> BereavedDiscussion { get; set; }

        /// <summary>
        /// Case breakdown meo summary notes
        /// </summary>
        public EventContainerItem<MeoSummaryEventItem, NullPrepopulated> MeoSummary { get; set; }

        /// <summary>
        /// Case breakdown qap discussion
        /// </summary>
        public EventContainerItem<QapDiscussionEventItem, QapDiscussionPrepopulated> QapDiscussion { get; set; }

        /// <summary>
        /// case breakdown medical history
        /// </summary>
        public EventContainerItem<MedicalHistoryEventItem, NullPrepopulated> MedicalHistory { get; set; }

        /// <summary>
        /// case breakdown admission notes
        /// </summary>
        public EventContainerItem<AdmissionEventItem, NullPrepopulated> AdmissionNotes { get; set; }

        public CaseClosedEventItem CaseClosed { get; set; }
    }
}
