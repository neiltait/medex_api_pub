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
        /// <summary>
        /// Count of Total Cases.
        /// </summary>
        public int CountOfTotalCases { get; set; }

        /// <summary>
        /// Count of Urgent Cases.
        /// </summary>
        public int CountOfUrgentCases { get; set; }

        /// <summary>
        /// Count of Cases where Admission Notes have been added.
        /// </summary>
        public int CountOfCasesAdmissionNotesHaveBeenAdded { get; set; }
               
        /// <summary>
        /// Count of Cases Ready for Medical Examiner Scrutiny.
        /// </summary>
        public int CountOfCasesReadyForMEScrutiny { get; set; }
               
        /// <summary>
        /// Count of Cases Unassigned.
        /// </summary>
        public int CountOfCasesUnassigned { get; set; }
               
        /// <summary>
        /// Count of Cases which have been Scrutinised by a Medical Examiner.
        /// </summary>
        public int CountOfCasesHaveBeenScrutinisedByME { get; set; }
               
        /// <summary>
        /// Count of Cases pending Admission Notes.
        /// </summary>
        public int CountOfCasesPendingAdmissionNotes { get; set; }
               
        /// <summary>
        /// Count of Cases pending Discussion with QAP.
        /// </summary>
        public int CountOfCasesPendingDiscussionWithQAP { get; set; }
               
        /// <summary>
        /// Count of Cases pending Discussion with Represenative.
        /// </summary>
        public int CountOfCasesPendingDiscussionWithRepresentative { get; set; }
             
        /// <summary>
        /// Count of Cases that have final case outstanding outcomes.
        /// </summary>
        public int CountOfCasesHaveFinalCaseOutstandingOutcomes { get; set; }

        /// <summary>
        ///     List of Examinations.
        /// </summary>
        public IEnumerable<PatientCardItem> Examinations { get; set; }
    }
}