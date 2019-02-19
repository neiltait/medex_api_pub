using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Medical_Examiner_API.Models
{
    public class Permission : Record
    {
        [Required]
        [JsonProperty(PropertyName = "permission_id")]
        public string PermissionId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "location_id")]
        public string LocationId { get; set; }
    }
}