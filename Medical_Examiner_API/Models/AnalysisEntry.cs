using System.ComponentModel.DataAnnotations;
using Medical_Examiner_API.Enums;
using Newtonsoft.Json;

namespace Medical_Examiner_API.Models
{
    public class AnalysisEntry : Record
    {
        [Required]
        [JsonProperty(PropertyName = "analysis_entry_id")]
        public string AnalysisEntryId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "examination_id")]
        public string ExaminationId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }

        [Required]
        [JsonProperty(PropertyName = "analasis_entry_type")]
        public AnalysisEntryType AnalysisEntryType { get; set; }

        [Required]
        [JsonProperty(PropertyName = "present_at_death")]
        public bool PresentAtDeath { get; set; }

        [Required]
        [JsonProperty(PropertyName = "informed_of_death")]
        public bool InformedOfDeath { get; set; }
    }
}