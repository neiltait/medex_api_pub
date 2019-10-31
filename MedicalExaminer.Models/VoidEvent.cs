using System;
using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class VoidEvent : IEvent
    {
        /// <summary>
        /// the reason for voiding the examination.
        /// </summary>
        [JsonProperty(PropertyName = "void_reason")]
        public string VoidReason { get; set; }

        /// <summary>
        /// the event type
        /// </summary>
        [JsonProperty(PropertyName = "event_type")]
        public EventType EventType => EventType.CaseVoid;

        /// <summary>
        /// the id of the event
        /// </summary>
        [JsonProperty(PropertyName = "event_id")]
        public string EventId { get; set; }

        /// <summary>
        /// Is the event final, not a draft
        /// </summary>
        [JsonProperty(PropertyName = "is_final")]
        public bool IsFinal => true;

        /// <summary>
        /// the users id that added the event
        /// </summary>
        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// the users full name that added the event
        /// </summary>
        [JsonProperty(PropertyName = "user_full_name")]
        public string UserFullName { get; set; }

        /// <summary>
        /// GMC Number
        /// </summary>
        [JsonProperty(PropertyName = "gmc_number")]
        public string GmcNumber { get; set; }

        /// <summary>
        /// The users role that added the event
        /// </summary>
        [JsonProperty(PropertyName = "users_role")]
        public string UsersRole { get; set; }

        /// <summary>
        /// Date the event was added
        /// </summary>
        [JsonProperty(PropertyName = "created")]
        public DateTime? Created { get; set; }
    }
}
