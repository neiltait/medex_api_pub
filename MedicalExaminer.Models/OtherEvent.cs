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
        public string EventText { get; set; }

        /// <summary>
        /// Enum for the status (Draft or Final).
        /// </summary>
        [JsonProperty(PropertyName = "event_status")]
        public EventStatus EventStatus { get; set; }

        /// <summary>
        /// list of ammendments for this item
        /// </summary>
        [JsonProperty(PropertyName ="amendments")]
        public IEnumerable<IEvent> Amendments { get; set; }
    }
}