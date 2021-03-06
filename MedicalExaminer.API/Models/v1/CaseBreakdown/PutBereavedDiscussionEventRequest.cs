﻿using System;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class PutBereavedDiscussionEventRequest
    {
        /// <summary>
        /// Event Identification.
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// Dictionary for the status (Draft or Final).
        /// </summary>
        public bool IsFinal { get; set; }

        /// <summary>
        /// Participant's full name.
        /// </summary>
        public string ParticipantFullName { get; set; }

        /// <summary>
        /// Participant's relationship.
        /// </summary>
        public string ParticipantRelationship { get; set; }

        /// <summary>
        /// Participant's phone number.
        /// </summary>
        public string ParticipantPhoneNumber { get; set; }

        /// <summary>
        /// Participant Present At Death.
        /// </summary>
        public PresentAtDeath? PresentAtDeath { get; set; }

        /// <summary>
        /// Participant Informed At Death.
        /// </summary>
        public InformedAtDeath? InformedAtDeath { get; set; }

        /// <summary>
        /// Date of Conversation.
        /// </summary>
        public DateTime? DateOfConversation { get; set; }

        /// <summary>
        /// Time of Conversation.
        /// </summary>
        public TimeSpan? TimeOfConversation { get; set; }

        /// <summary>
        /// Unable to happen.
        /// </summary>
        public bool DiscussionUnableHappen { get; set; }

        /// <summary>
        /// details as to why the discussion was unable to happen
        /// </summary>
        public string DiscussionUnableHappenDetails { get; set; }

        /// <summary>
        /// Details of the Discussion.
        /// </summary>
        public string DiscussionDetails { get; set; }

        /// <summary>
        /// Outcome of the Bereaved Discussion.
        /// </summary>
        public BereavedDiscussionOutcome? BereavedDiscussionOutcome { get; set; }

        /// <summary>
        /// Clinical governance review
        /// </summary>
        public ClinicalGovernanceReview? ClinicalGovernanceReview { get; set; }

        /// <summary>
        /// Clinical governance review text
        /// </summary>
        public string ClinicalGovernanceReviewText { get; set; }
    }
}
