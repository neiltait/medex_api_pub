using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Medical_Examiner_API.Models
{
    public class Examination : Record
    {
        [JsonProperty(PropertyName = "id")]

        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(250)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(250)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTimeOffset DateOfDeath { get; set; }

        [Required]
        public bool Completed { get; set; }

        public Examination()
        {
            Completed = false;
        }
    }
}
