using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Medical_Examiner_API.Models
{
    public class MEUser : Record
    {
        [Required]
        [DataType(DataType.Text)]
        [JsonProperty(PropertyName = "me_user_id")]
        public string MEUserId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
    }
}
