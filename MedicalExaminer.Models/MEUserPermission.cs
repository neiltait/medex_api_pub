using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    /// <summary>
    /// MedEx User Permission.
    /// </summary>
    public class MEUserPermission
    {
        /// <summary>
        /// Permission Id.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "permission_id")]
        public string PermissionId { get; set; }

        /// <summary>
        /// Location Id.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "location_id")]
        public string LocationId { get; set; }

        /// <summary>
        /// User Role.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "user_role")]
        public int UserRole { get; set; }
    }
}
