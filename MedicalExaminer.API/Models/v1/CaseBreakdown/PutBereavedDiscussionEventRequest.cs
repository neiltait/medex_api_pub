using System;
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
        /// User Identifier.
        /// </summary>
        public string UserId { get; set; }

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
        public PresentAtDeath PresentAtDeath { get; set; }

        /// <summary>
        /// Participant Informed At Death.
        /// </summary>
        public InformedAtDeath InformedAtDeath { get; set; }

        /// <summary>
        /// Date of Conversation.
        /// </summary>
        public DateTime DateOfConversation { get; set; }

        /// <summary>
        /// Unable to happen.
        /// </summary>
        public bool DiscussionUnableHappen { get; set; }

        /// <summary>
        /// Details of the Discussion.
        /// </summary>
        public string DiscussionDetails { get; set; }

        ///// <summary>
        ///// Outcome of the Bereaved Discussion.
        ///// </summary>
        public BereavedDiscussionOutcome BereavedDiscussionOutcome { get; set; }
    }
}
