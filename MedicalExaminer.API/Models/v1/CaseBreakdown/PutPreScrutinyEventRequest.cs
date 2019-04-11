using MedicalExaminer.API.Attributes;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class PutPreScrutinyEventRequest
    {
        /// <summary>
        /// Event Identification.
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// Event Text.
        /// </summary>
        public string MedicalExaminerThoughts { get; set; }

        ///// <summary>
        ///// IsFinal true for final, false for draft
        ///// </summary>
        public bool IsFinal { get; set; }


        /// <summary>
        /// Dictionary for circumstances of death radio button.
        /// </summary>
        public OverallCircumstancesOfDeath CircumstancesOfDeath { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Medical Examiner 1a
        /// </summary>
        public string CauseOfDeath1a { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Medical Examiner 1b
        /// </summary>
        public string CauseOfDeath1b { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Medical Examiner 1c
        /// </summary>
        public string CauseOfDeath1c { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Medical Examiner 2
        /// </summary>
        public string CauseOfDeath2 { get; set; }

        /// <summary>
        /// Dictionary for Outcome Of Pre-Scrutiny radio button.
        /// </summary>
        public OverallOutcomeOfPreScrutiny OutcomeOfPreScrutiny { get; set; }

        /// <summary>
        /// Dictionary for Clinical Governance Review radio button.
        /// </summary>
        public ClinicalGovernanceReview ClinicalGovernanceReview { get; set; }

        /// <summary>
        /// Details of Clinical Governance Review if said yes for Clinical Governance Review radio button.
        /// </summary>
        [RequiredIfAttributesMatch(nameof(ClinicalGovernanceReview), ClinicalGovernanceReview.Yes)]
        public string ClinicalGovernanceReviewText { get; set; }
    }
}
