using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Medical_Examiner_API.Models
{
    public class Location
    {
        [Required]
        [JsonProperty(PropertyName = "location_id")]
        public string LocationId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [Required]
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [Required]
        [JsonProperty(PropertyName = "parent_location_id")]
        public string ParentLocationId { get; set; }
    }
}