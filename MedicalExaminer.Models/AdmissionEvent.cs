using System;
using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class AdmissionEvent : IEvent
    {
        /// <summary>
        /// Users full name
        /// </summary>
        [JsonProperty(PropertyName = "users_full_name")]
        public string UserFullName { get; set; }

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
        /// Event Identification.
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
        [JsonProperty(PropertyName = "admission_event_notes")]
        public string Notes { get; set; }

        /// <summary>
        /// IsFinal, final = true, draft = false
        /// </summary>
        [JsonProperty(PropertyName = "is_final")]
        public bool IsFinal { get; set; }

        /// <summary>
        /// the type of event this is
        /// </summary>
        [JsonProperty(PropertyName = "event_type")]
        public EventType EventType => EventType.Admission;

        /// <summary>
        /// date of last admission
        /// </summary>
        [JsonProperty(PropertyName = "admitted_date")]
        public DateTime? AdmittedDate { get; set; }

        /// <summary>
        /// date of last admission - Unknown or not
        /// </summary>
        [JsonProperty(PropertyName = "admitted_date_unknown")]
        public bool? AdmittedDateUnknown { get; set; } = false;

        /// <summary>
        /// time of last admission
        /// </summary>
        [JsonProperty(PropertyName = "admitted_time")]
        public TimeSpan? AdmittedTime { get; set; }

        /// <summary>
        /// time of last admission - Unknown or not
        /// </summary>
        [JsonProperty(PropertyName = "admitted_time_unknown")]
        public bool? AdmittedTimeUnknown { get; set; } = false;

        /// <summary>
        /// Do you suspect this case may need Immediate Coroner Referral Yes = true, No = false
        /// </summary>
        [JsonProperty(PropertyName = "immediate_coroner_referral")]
        public bool? ImmediateCoronerReferral { get; set; }

        /// <summary>
        /// Route of Admission
        /// </summary>
        [JsonProperty(PropertyName = "route_of_admission")]
        public RouteOfAdmission? RouteOfAdmission { get; set; }

    }
}