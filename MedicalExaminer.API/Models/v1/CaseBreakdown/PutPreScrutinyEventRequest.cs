using System.Collections.Generic;
using MedicalExaminer.API.Attributes;
using MedicalExaminer.Models;
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
        /// User Identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Event Text.
        /// </summary>
        public string MedicalExaminerThoughts { get; set; }

        ///// <summary>
        ///// IsFinal true for final, false for draft
        ///// </summary>
        public bool IsFinal { get; set; }



        /// <summary>
        /// the type of event this is
        /// </summary>
        public EventType EventType => EventType.PreScrutiny;

        /// <summary>
        /// Dictionary for circumstances of death radio button.
        /// </summary>
        public OverallCircumstancesOfDeath CircumstancesOfDeath { get; set; }

        /// <summary>
        /// Cause Of Death. (Data type to be confirmed).
        /// </summary>
        public IEnumerable<string> CauseOfDeath { get; set; }

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
        public ClinicalGovernanceReview ClinicalGovernanceReviewText { get; set; }
    }
}
