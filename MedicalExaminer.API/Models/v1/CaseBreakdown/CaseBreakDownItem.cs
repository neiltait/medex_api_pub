namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class CaseBreakDownItem
    {

        public EventContainerItem OtherEvents { get; set; }

        public EventContainerItem PreScrutiny { get; set; }

        public EventContainerItem BereavedDiscussion { get; set; }

        public EventContainerItem MeoSummary { get; set; }

        public EventContainerItem QapDiscussion { get; set; }

        public EventContainerItem MedicalHistory { get; set; }

        public EventContainerItem AdmissionNotes { get; set; }
    }
}
