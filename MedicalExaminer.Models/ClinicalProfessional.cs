using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class ClinicalProfessional
    {
        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "role")]
        public string Role { get; set; }

        [JsonProperty(PropertyName = "organisation")]
        public string Organisation { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }

        [JsonProperty(PropertyName = "gmc_number")]
        public string GMCNumber { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Clinical Professional 1a
        /// </summary>
        [JsonProperty(PropertyName = "cause_of_death_1a")]
        public string CauseOfDeath1a { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Clinical Professional 1b
        /// </summary>
        [JsonProperty(PropertyName = "cause_of_death_1b")]
        public string CauseOfDeath1b { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Clinical Professional 1c
        /// </summary>
        [JsonProperty(PropertyName = "cause_of_death_1c")]
        public string CauseOfDeath1c { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Clinical Professional 2
        /// </summary>
        [JsonProperty(PropertyName = "cause_of_death_2")]
        public string CauseOfDeath2 { get; set; }
    }
}