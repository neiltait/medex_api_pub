using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class EventOther
    {
        /// <summary>
        /// Event ID.
        /// </summary>
        [JsonProperty(PropertyName = "Other_Event_Id")]
        public string EventId { get; set; }

        /// <summary>
        /// Event Text (Length to be confirmed).
        /// </summary>
        [JsonProperty(PropertyName = "Other_Event_Text")]
        public string EventText { get; set; }

        /// <summary>
        /// Enum for the status (Draft or Final).
        /// </summary>
        [JsonProperty(PropertyName = "Event_Status")]
        public EventStatus EventStatus { get; set; }

        // public MeUser User { get; set; }
        // public DateTime DateTimeEntered { get; set; }
    }
}