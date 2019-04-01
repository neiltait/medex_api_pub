using System.Collections.Generic;
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

        /// <summary>
        /// Permissions.
        /// </summary>
        [JsonProperty(PropertyName = "permissions")]
        public IEnumerable<MEUserPermission> Permissions { get; set; }

        // TODO: Remove.
        [Required]
        [DataType(DataType.Text)]
        [JsonProperty(PropertyName = "UserRole")]
        public UserRoles UserRole { get; set; }
    }
}