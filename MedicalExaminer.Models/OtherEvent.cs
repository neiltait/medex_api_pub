using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MedicalExaminer.Models
{
    public class OtherEvent : IEvent
    {
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
        /// Enum for the status (Draft or Final).
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