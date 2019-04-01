using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class MEUserPermission
    {
        [Required]
        [JsonProperty(PropertyName = "permission_id")]
        public string PermissionId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "location_id")]
        public string LocationId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "user_role")]
        public int UserRole { get; set; }
    }
}
