using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Medical_Examiner_API.Models
{
    public class ClinicalProfessional : Contact
    {
        [Required]
        [JsonProperty(PropertyName = "location_id")]
        public string LocationId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }
    }
}
