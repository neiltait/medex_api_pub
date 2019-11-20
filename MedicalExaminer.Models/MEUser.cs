using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    /// <summary>
    /// Medical Examiner User.
    /// </summary>
    public class MeUser : Record
    {
        /// <summary>
        /// User Id.
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        [JsonProperty(PropertyName = "id")]
        public string UserId { get; set; }

        /// <summary>
        /// First Name.
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Last Name.
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        /// <summary>
        /// Okta Id.
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        [JsonProperty(PropertyName = "okta_id")]
        public string OktaId { get; set; }

        /// <summary>
        /// GMC Number.
        /// </summary>
        [DataType(DataType.Text)]
        [JsonProperty(PropertyName = "gmc_number")]
        public string GmcNumber { get; set; }

        /// <summary>
        /// Permissions.
        /// </summary>
        [JsonProperty(PropertyName = "permissions")]
        public IEnumerable<MEUserPermission> Permissions { get; set; }
    }
}
