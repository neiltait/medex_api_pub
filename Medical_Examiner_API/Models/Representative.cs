using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Medical_Examiner_API.Models
{
    public class Representative
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string RelationshipToDeceased { get; set; }

        [Required]
        public string ContactPhoneNumber { get; set; }

        [Required]
        public bool PresentAtDeath { get; set; }

        [Required]
        public bool InformedOfDeath { get; set; }
    }
}
