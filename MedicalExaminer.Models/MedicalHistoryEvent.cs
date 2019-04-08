using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;
using System;

namespace MedicalExaminer.Models
{
    public class MedicalHistoryEvent : IEvent
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
        public string UserId { get; }

        /// <summary>
        /// the type of event this is
        /// </summary>
        [JsonProperty(PropertyName = "event_type")]
        public EventType EventType => EventType.MedicalHistory;

        /// <summary>
        /// Draft is false, final true
        /// </summary>
        [JsonProperty(PropertyName = "is_final")]
        public bool IsFinal { get; }

        /// <summary>
        /// Event Text (Length to be confirmed).
        /// </summary>
        [JsonProperty(PropertyName = "medical_history_event_text")]
        public string Text { get; set; }
    }
}
