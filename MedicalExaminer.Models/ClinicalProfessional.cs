using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class ClinicalProfessional : Contact
    {
        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set;  }

        [Required]
        [JsonProperty(PropertyName = "role")]
        public string Role { get; set; }

        [Required]
        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }

        [Required]
        [JsonProperty(PropertyName = "organisation")]
        public string Organisation { get; set; }

        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }
    }
}