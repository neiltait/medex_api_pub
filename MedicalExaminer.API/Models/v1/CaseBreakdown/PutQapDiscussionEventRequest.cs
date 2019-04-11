using System;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class PutQapDiscussionEventRequest
    {
        /// <summary>
        /// Event Identification.
        /// </summary>
        public string EventId { get; set; }

        ///// <summary>
        ///// IsFinal true for final, false for draft
        ///// </summary>
        public bool IsFinal { get; set; }

        /// <summary>
        /// Participant's roll.
        /// </summary>
        public string ParticipantRoll { get; set; }

        /// <summary>
        /// Participant's organisation.
        /// </summary>
        public string ParticipantOrganisation { get; set; }

        /// <summary>
        /// Participant's phone number.
        /// </summary>
        public string ParticipantPhoneNumber { get; set; }

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
        public QapDiscussionOutcome QapDiscussionOutcome { get; set; }

        /// <summary>
        /// Participants Full name
        /// </summary>
        public string ParticipantName { get; set; }

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
    }
}
