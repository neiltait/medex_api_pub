﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class PreScrutinyEvent : IEvent
    {
        /// <summary>
        /// Event Identification.
        /// </summary>
        [JsonProperty(PropertyName = "event_id")]
        public string EventId { get; set; }

        /// <summary>
        /// User Identifier.
        /// </summary>
        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// Event Text.
        /// </summary>
        [JsonProperty(PropertyName = "pre_scrutiny_event_text")]
        public string EventText { get; set; }


        /// <summary>
        /// IsFinal, final = true, draft = false
        /// </summary>
        [JsonProperty(PropertyName = "is_final")]
        public bool IsFinal { get; set; }


        ///// <summary>
        ///// the type of event this is
        ///// </summary>
        [JsonProperty(PropertyName = "event_type")]
        public EventType EventType => EventType.PreScrutiny;

        /// <summary>
        /// Dictionary for circumstances of death radio button.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "circumstances_of_death")]
        public OverallCircumstancesOfDeath CircumstancesOfDeath { get; set; }

        /// <summary>
        /// Cause Of Death. (Data type to be confirmed).
        /// </summary>
        [JsonProperty(PropertyName = "cause_of_death")]
        public IEnumerable<string> CauseOfDeath { get; set; }

        /// <summary>
        /// Dictionary for Outcome Of Pre-Scrutiny radio button.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "outcome_of_pre_scrutiny")]
        public OverallOutcomeOfPreScrutiny OutcomeOfPreScrutiny { get; set; }

        /// <summary>
        /// Dictionary for Clinical Governance Review radio button.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "clinical_governance_review")]
        public ClinicalGovernanceReview ClinicalGovernanceReview { get; set; }

        /// <summary>
        /// Details of Clinical Governance Review if said yes for Clinical Governance Review radio button.
        /// </summary>
        [JsonProperty(PropertyName = "clinical_governance_review_text")]
        public ClinicalGovernanceReview ClinicalGovernanceReviewText { get; set; }
    }
}