using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Medical_Examiner_API.Models
{
    public abstract class Contact : Record
    {
        [Required]
        [JsonProperty(PropertyName = "full_name")]
        public string FullName { get; set; }

        [Required]
        [JsonProperty(PropertyName = "contact_phone_number")]
        public string ContactPhoneNumber { get; set; }

        [Required]
        [JsonProperty(PropertyName = "relation_or_role")]
        public string RelationOrRole { get; set; }
    }
}