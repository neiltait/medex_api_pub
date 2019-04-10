using System;
using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class QapDiscussionEvent : IEvent
    {
        /// <summary>
        /// Date event was created
        /// </summary>
        [JsonProperty(PropertyName = "created")]
        public DateTime? Created { get; set; }

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
        /// Draft is false, final true
        /// </summary>
        [JsonProperty(PropertyName = "is_final")]
        public bool IsFinal { get; set; }

        /// <summary>
        /// the type of event this is
        /// </summary>
        [JsonProperty(PropertyName = "event_type")]
        public EventType EventType => EventType.QapDiscussion;

        /// <summary>
        /// Participant's roll.
        /// </summary>
        [JsonProperty(PropertyName = "participant_roll")]
        public string ParticipantRoll { get; set; }

        /// <summary>
        /// Participant's organisation.
        /// </summary>
        [JsonProperty(PropertyName = "participant_organisation")]
        public string ParticipantOrganisation { get; set; }

        /// <summary>
        /// Participant's phone number.
        /// </summary>
        [JsonProperty(PropertyName = "participant_phone_number")]
        public string ParticipantPhoneNumber { get; set; }
        
        /// <summary>
        /// Date of Conversation.
        /// </summary>
        [JsonProperty(PropertyName = "date_of_conversation")]
        public DateTime DateOfConversation { get; set; }

        /// <summary>
        /// Unable to happen.
        /// </summary>
        [JsonProperty(PropertyName = "discussion_unable_happen")]
        public bool DiscussionUnableHappen { get; set; }

        /// <summary>
        /// Details of the Discussion.
        /// </summary>
        [JsonProperty(PropertyName = "discussion_details")]
        public string DiscussionDetails { get; set; }

        ///// <summary>
        ///// Outcome of the Bereaved Discussion.
        ///// </summary>
        [JsonProperty(PropertyName = "bereaved_discussion_outcome")]
        public QapDiscussionOutcome QapDiscussionOutcome { get; set; }

        ///// <summary>
        ///// Qap particpant name
        ///// </summary>
        [JsonProperty(PropertyName = "participant_name")]
        public string ParticipantName { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Medical Examiner 1a
        /// </summary>
        [JsonProperty(PropertyName = "cause_of_death_1a")]
        public string CauseOfDeath1a { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Medical Examiner 1b
        /// </summary>
        [JsonProperty(PropertyName = "cause_of_death_1b")]
        public string CauseOfDeath1b { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Medical Examiner 1c
        /// </summary>
        [JsonProperty(PropertyName = "cause_of_death_1c")]
        public string CauseOfDeath1c { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Medical Examiner 2
        /// </summary>
        [JsonProperty(PropertyName = "cause_of_death_2")]
        public string CauseOfDeath2 { get; set; }

    }
}
