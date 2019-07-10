using System;
using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class CaseClosedEvent : IEvent
    {
        /// <summary>
        /// Date the patient died
        /// </summary>
        [JsonProperty(PropertyName = "date_case_closed")]
        public DateTime? DateCaseClosed { get; set; }

        /// <summary>
        /// event type
        /// </summary>
        [JsonProperty(PropertyName = "event_type")]
        public EventType EventType => EventType.CaseClosed;

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
        public string UserId { get; set; }

        /// <summary>
        /// the date the event was created.
        /// </summary>
        [JsonProperty(PropertyName = "created")]
        public DateTime? Created { get; set; }

        /// <summary>
        /// Users full name
        /// </summary>
        [JsonProperty(PropertyName = "user_full_name")]
        public string UserFullName { get; set; }

        /// <summary>
        /// Users Role
        /// </summary>
        [JsonProperty(PropertyName = "user_role")]
        public string UsersRole { get; set; }

        /// <summary>
        /// Case Outcome
        /// </summary>
        [JsonProperty(PropertyName = "case_outcome")]
        public CaseOutcomeSummary CaseOutcome { get; set; }
    }
}
