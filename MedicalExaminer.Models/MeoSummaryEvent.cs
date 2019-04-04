using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class MeoSummaryEvent
    {
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

        //***************** uncomment after the generic event classes approved *********************
        ///// <summary>
        ///// Dictionary for the status (Draft or Final).
        ///// </summary>
        //[JsonProperty(PropertyName = "event_status")]
        //public EventStatus EventStatus { get; set; }

        ///// <summary>
        ///// list of ammendments for this item
        ///// </summary>
        //[JsonProperty(PropertyName = "amendments")]
        //public IEnumerable<IEvent> Amendments { get; set; }

        ///// <summary>
        ///// the type of event this is
        ///// </summary>
        //[JsonProperty(PropertyName = "event_type")]
        //public EventType EventType => EventType.BereavedDiscussion;

        /// <summary>
        /// Details of the MEO Summary.
        /// </summary>
        [JsonProperty(PropertyName = "summary_details")]
        public string SummaryDetails { get; set; }
    }
}
