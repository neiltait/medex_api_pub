using System.Collections.Generic;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Models.v1.Examinations
{
    /// <inheritdoc />
    /// <summary>
    ///     Response object for a list of examinations.
    /// </summary>
    public class GetExaminationsResponse : ResponseBase
    {
        public int CountOfTotalCases { get; set; }
        public int CountOfUrgentCases { get; set; }
        public int CountOfCasesAdmissionNotesHaveBeenAdded { get; set; }
               
        public int CountOfCasesReadyForMEScrutiny { get; set; }
               
        public int CountOfCasesUnassigned { get; set; }
               
        public int CountOfCasesHaveBeenScrutinisedByME { get; set; }
               
        public int CountOfCasesPendingAdmissionNotes { get; set; }
               
        public int CountOfCasesPendingDiscussionWithQAP { get; set; }
               
        public int CountOfCasesPendingDiscussionWithRepresentative { get; set; }
               
        public int CountOfCasesHaveFinalCaseOutstandingOutcomes { get; set; }

        /// <summary>
        ///     List of Examinations.
        /// </summary>
        public IEnumerable<PatientCardItem> Examinations { get; set; }
    }
}