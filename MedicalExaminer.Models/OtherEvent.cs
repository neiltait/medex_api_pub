using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class OtherEvent
    {
        /// <summary>
        /// Event ID.
        /// </summary>
        [JsonProperty(PropertyName = "other_event_id")]
        public string EventId { get; set; }

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

        // public MeUser User { get; set; }
        // public DateTime DateTimeEntered { get; set; }
    }
}