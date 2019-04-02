using System;
using System.ComponentModel.DataAnnotations;
using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class MeUser : Record
    {
        [Required]
        [DataType(DataType.Text)]
        [JsonProperty(PropertyName = "id")]
        public string UserId { get; set; }

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

        [Required]
        [DataType(DataType.Text)]
        [JsonProperty(PropertyName = "okta_token")]
        public string OktaToken { get; set; }

        [Required]
        [Display(Name = "okta_token_expiry")]
        [DataType(DataType.DateTime)]
        public DateTimeOffset OktaTokenExpiry { get; set; }
    }
}