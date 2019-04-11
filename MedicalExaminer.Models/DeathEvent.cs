using System;
using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class DeathEvent : IEvent
    {
        /// <summary>
        /// Date the patient died
        /// </summary>
        [JsonProperty(PropertyName = "date_of_death")]
        public DateTime? DateOfDeath { get; set; }

        /// <summary>
        /// time the patient died
        /// </summary>
        [JsonProperty(PropertyName = "time_of_death")]
        public TimeSpan? TimeOfDeath { get; set; }

        /// <summary>
        /// event type
        /// </summary>
        [JsonProperty(PropertyName = "event_type")]
        public EventType EventType => EventType.PatientDied;

        /// <summary>
        /// event id
        /// </summary>
        [JsonProperty(PropertyName = "event_id")]
        public string EventId { get; set; }

        /// <summary>
        /// is the event a draft or a final version
        /// </summary>
        [JsonProperty(PropertyName = "is_final")]
        public bool IsFinal => true;

        /// <summary>
        /// user id that created the event
        /// </summary>
        [JsonProperty(PropertyName = "user_id")]
        public string UserId {get; set;}

        /// <summary>
        /// the date the event was created.
        /// </summary>
        [JsonProperty(PropertyName = "created")]
        public DateTime? Created { get; set; }
    }
}
