using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;


namespace MedicalExaminer.Models
{
    public class Location
    {
        [Required]
        [JsonProperty(PropertyName = "Id")]
        public string LocationId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [Required]
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [Required]
        [JsonProperty(PropertyName = "parent")]
        public string Parent { get; set; }
        }
    }
