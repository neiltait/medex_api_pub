using System.ComponentModel.DataAnnotations;
using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;


namespace MedicalExaminer.Models
{
    public class Location
    {

        [JsonProperty(PropertyName = "id")]
        public string LocationId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "parentId")]
        public string ParentId { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }

        [JsonProperty(PropertyName = "type")]
        public LocationType Type { get; set; }
    }
}
