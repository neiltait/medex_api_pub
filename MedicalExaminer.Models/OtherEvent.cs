using System;
using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class OtherEvent : IEvent
    {
        /// <summary>
        /// Users full name
        /// </summary>
        [JsonProperty(PropertyName = "user_full_name")]
        public string UserFullName { get; set; }

        /// <summary>
        /// GMC Number
        /// </summary>
        [JsonProperty(PropertyName = "gmc_number")]
        public string GmcNumber { get; set; }

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
        /// Event Identication.
        /// </summary>
        [JsonProperty(PropertyName = "event_id")]
        public string EventId { get; set; }

        /// <summary>
        /// User Identifier.
        /// </summary>
        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// Event Text (Length to be confirmed).
        /// </summary>
        [JsonProperty(PropertyName = "other_event_text")]
        public string Text { get; set; }

        /// <summary>
        /// IsFinal, final = true, draft = false
        /// </summary>
        [JsonProperty(PropertyName = "is_final")]
        public bool IsFinal { get; set; }

        /// <summary>
        /// the type of event this is
        /// </summary>
        [JsonProperty(PropertyName = "event_type")]
        public EventType EventType => EventType.Other;
    }
}