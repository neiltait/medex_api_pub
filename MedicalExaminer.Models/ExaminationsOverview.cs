using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.Models
{
    public class ExaminationsOverview
    {
        public int AdmissionNotesHaveBeenAdded { get; set; }

        public int ReadyForMEScrutiny { get; set; }

        public int Assigned { get; set; }

        public int HaveBeenScrutinisedByME { get; set; }

        public int PendingAdmissionNotes { get; set; }

        public int PendingDiscussionWithQAP { get; set; }

        public int PendingDiscussionWithRepresentative { get; set; }

        public int HaveFinalCaseOutstandingOutcomes { get; set; }
    }
}
