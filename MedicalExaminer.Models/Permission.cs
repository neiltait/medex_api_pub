﻿using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class Permission : Record
    {
        [Required]
        [JsonProperty(PropertyName = "id")]
        public string PermissionId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "location_id")]
        public string LocationId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "user_role")]
        public int UserRole { get; set; }
    }
}