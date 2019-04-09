using System;
using System.ComponentModel.DataAnnotations;
using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class Representative
    {
        [JsonProperty(PropertyName = "full_name")]
        [Required]
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string FullName { get; set; }

        /// <summary>
        ///     Representatives relationship to the patient
        /// </summary>
        [JsonProperty(PropertyName = "relationship")]
        [Required]
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string Relationship { get; set; }

        /// <summary>
        ///     Representatives phone number
        /// </summary>
        [JsonProperty(PropertyName = "phone_number")]
        [Required]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     Was the representative present at the death?
        /// </summary>
        [JsonProperty(PropertyName = "present_at_death")]
        [Required]
        public PresentAtDeath PresentAtDeath { get; set; }

        /// <summary>
        ///     Has the representative been informed?
        /// </summary>
        [JsonProperty(PropertyName = "informed")]
        [Required]
        public Informed Informed { get; set; }

        [JsonProperty(PropertyName = "appointment_date")]
        [Required]
        public DateTime? AppointmentDate { get; set; }

        [JsonProperty(PropertyName = "appointment_time")]
        [Required]
        public TimeSpan? AppointmentTime { get; set; }


        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }
    }
}