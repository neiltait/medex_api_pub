namespace MedicalExaminer.Models
{
    public class ExaminationsOverview
    {
        public int TotalCases { get; set; }
        public int CountOfUrgentCases { get; set; }
        public int CountOfAdmissionNotesHaveBeenAdded { get; set; }

        public int CountOfReadyForMEScrutiny { get; set; }

        public int CountOfUnassigned { get; set; }

        public int CountOfHaveBeenScrutinisedByME { get; set; }

        public int CountOfPendingAdmissionNotes { get; set; }

        public int CountOfPendingDiscussionWithQAP { get; set; }

        public int CountOfPendingDiscussionWithRepresentative { get; set; }

        public int CountOfHaveFinalCaseOutstandingOutcomes { get; set; }
    }
}
