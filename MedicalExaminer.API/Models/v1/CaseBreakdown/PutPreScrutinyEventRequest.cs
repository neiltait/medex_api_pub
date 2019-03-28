using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.API.Attributes;
using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;

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
        public string EventText { get; set; }

        //***************** uncomment after the generic event classes approved *********************
        ///// <summary>
        ///// Dictionary for the status (Draft or Final).
        ///// </summary>
        //public EventStatus EventStatus { get; set; }

        ///// <summary>
        ///// list of ammendments for this item
        ///// </summary>
        //public IEnumerable<IEvent> Amendments { get; set; }

        ///// <summary>
        ///// the type of event this is
        ///// </summary>
        //public EventType EventType => EventType.PreScrutiny;

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
