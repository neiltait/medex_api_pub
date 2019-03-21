namespace MedicalExaminer.Models
{
    public class ExaminationsOverview
    {
        public int TotalCases { get; set; }
        public int UrgentCases { get; set; }
        public int AdmissionNotesHaveBeenAdded { get; set; }

        public int ReadyForMEScrutiny { get; set; }

        public int Unassigned { get; set; }

        public int HaveBeenScrutinisedByME { get; set; }

        public int PendingAdmissionNotes { get; set; }

        public int PendingDiscussionWithQAP { get; set; }

        public int PendingDiscussionWithRepresentative { get; set; }

        public int HaveFinalCaseOutstandingOutcomes { get; set; }
    }
}
