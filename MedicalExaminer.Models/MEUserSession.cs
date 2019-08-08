using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    /// <summary>
    /// Medical Examiner User Session.
    /// </summary>
    public class MeUserSession
    {
        [Required]
        [DataType(DataType.Text)]
        [JsonProperty(PropertyName = "id")]
        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [JsonProperty(PropertyName = "okta_id")]
        public string OktaId { get; set; }

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
