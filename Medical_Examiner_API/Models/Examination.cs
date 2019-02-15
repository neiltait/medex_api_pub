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
        public string NHSNumber { get; set; }

        [Required]
        public int Gender { get; set; }

        [Required]
        public string HouseNameNumber { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string Town { get; set; }

        [Required]
        public string County { get; set; }

        [Required]
        public string Postcode { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string LastOccupation { get; set; }

        [Required]
        public string OrganisationCareBeforeDeathLocationId { get; set; }

        [Required]
        public Representative[] Representatives { get; set; }

        [Required]
        public string DeathOccuredLocationId { get; set; }

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
        public bool PriorityDeath { get; set; }

        [Required]
        public bool IsChild { get; set; }

        [Required]
        public bool ReferToCoroner { get; set; }

        [Required]
        public bool PriorityCase { get; set; }


        [Required]
        public bool Completed { get; set; }

        public Examination()
        {
            Completed = false;
        }
    }
}
