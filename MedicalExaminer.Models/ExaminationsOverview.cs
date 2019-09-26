namespace MedicalExaminer.Models
{
    public class ExaminationsOverview
    {
        /// <summary>
        /// Total Cases
        /// </summary>
        public int TotalCases { get; set; }

        /// <summary>
        /// Count Of Urgent Cases
        /// </summary>
        public int CountOfUrgentCases { get; set; }

        /// <summary>
        /// Count Of Have Unknown Basic Details
        /// </summary>
        public int CountOfHaveUnknownBasicDetails { get; set; }

        /// <summary>
        /// Count Of Ready For ME Scrutiny
        /// </summary>
        public int CountOfReadyForMEScrutiny { get; set; }

        /// <summary>
        /// Count Of Unassigned
        /// </summary>
        public int CountOfUnassigned { get; set; }

        /// <summary>
        /// Count Of Have Been Scrutinised By ME
        /// </summary>
        public int CountOfHaveBeenScrutinisedByME { get; set; }

        /// <summary>
        /// Count Of Pending Additional Details
        /// </summary>
        public int CountOfPendingAdditionalDetails { get; set; }

        /// <summary>
        /// Count Of Pending Discussion With QAP
        /// </summary>
        public int CountOfPendingDiscussionWithQAP { get; set; }

        /// <summary>
        /// Count Of Pending Discussion With Representative
        /// </summary>
        public int CountOfPendingDiscussionWithRepresentative { get; set; }

        /// <summary>
        /// Count Of Have Final Case Outstanding Outcomes
        /// </summary>
        public int CountOfHaveFinalCaseOutstandingOutcomes { get; set; }

        /// <summary>
        /// Count of cases that meet the filtered status
        /// </summary>
        public int CountOfFilteredCases { get; set; }
    }
}