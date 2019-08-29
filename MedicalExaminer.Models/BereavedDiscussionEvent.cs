using System;
using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class BereavedDiscussionEvent : IEvent
    {
        /// <summary>
        /// Users full name
        /// </summary>
        [JsonProperty(PropertyName = "user_full_name")]
        public string UserFullName { get; set; }

        /// <summary>
        /// Users Role
        /// </summary>
        [JsonProperty(PropertyName = "users_role")]
        public string UsersRole { get; set; }

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
        public EventType EventType => EventType.BereavedDiscussion;

        /// <summary>
        /// Participant's full name.
        /// </summary>
        [JsonProperty(PropertyName = "participant_full_name")]
        public string ParticipantFullName { get; set; }

        /// <summary>
        /// Participant's relationship.
        /// </summary>
        [JsonProperty(PropertyName = "participant_relationship")]
        public string ParticipantRelationship { get; set; }

        /// <summary>
        /// Participant's phone number.
        /// </summary>
        [JsonProperty(PropertyName = "participant_phone_number")]
        public string ParticipantPhoneNumber { get; set; }

        /// <summary>
        /// Participant Present At Death.
        /// </summary>
        [JsonProperty(PropertyName = "present_at_death")]
        public PresentAtDeath? PresentAtDeath { get; set; }

        /// <summary>
        /// Participant Informed At Death.
        /// </summary>
        [JsonProperty(PropertyName = "informed_at_death")]
        public InformedAtDeath? InformedAtDeath { get; set; }

        /// <summary>
        /// Date of Conversation.
        /// </summary>
        [JsonProperty(PropertyName = "date_of_conversation")]
        public DateTime? DateOfConversation { get; set; }

        /// <summary>
        /// time of Conversation.
        /// </summary>
        [JsonProperty(PropertyName = "time_of_conversation")]
        public TimeSpan? TimeOfConversation { get; set; }

        /// <summary>
        /// Unable to happen.
        /// </summary>
        [JsonProperty(PropertyName = "discussion_unable_happen")]
        public bool DiscussionUnableHappen { get; set; }

        /// <summary>
        /// Unable to happen details.
        /// </summary>
        [JsonProperty(PropertyName = "discussion_unable_happen_details")]
        public string DiscussionUnableHappenDetails { get; set; }

        /// <summary>
        /// Details of the Discussion.
        /// </summary>
        [JsonProperty(PropertyName = "discussion_details")]
        public string DiscussionDetails { get; set; }

        ///// <summary>
        ///// Outcome of the Bereaved Discussion.
        ///// </summary>
        [JsonProperty(PropertyName = "bereaved_discussion_outcome")]
        public BereavedDiscussionOutcome? BereavedDiscussionOutcome { get; set; }
    }
}
