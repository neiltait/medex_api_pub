using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Medical_Examiner_API.Models
{
    public class Examination : Record
    {
        // Linked Fields 


        [Required]
        [DataType(DataType.Text)]
        [JsonProperty(PropertyName = "examination_id")]
        public string ExaminationId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(250)]
        [JsonProperty(PropertyName = "full_name")]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(10)]
        [JsonProperty(PropertyName = "nhs_number")]
        public string NhsNumber { get; set; }

        [Required]
        [JsonProperty(PropertyName = "gender")]
        public ExaminationGender Gender { get; set; }

        [Required]
        [JsonProperty(PropertyName = "house_name_number")]
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string HouseNameNumber { get; set; }

        [Required]
        [JsonProperty(PropertyName = "street")]
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string Street { get; set; }

        [Required]
        [JsonProperty(PropertyName = "town")]
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string Town { get; set; }

        [Required]
        [JsonProperty(PropertyName = "county")]
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string County { get; set; }

        [Required]
        [JsonProperty(PropertyName = "postcode")]
        [DataType(DataType.Text)]
        [StringLength(12)]
        public string Postcode { get; set; }

        [Required]
        [JsonProperty(PropertyName = "country")]
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string Country { get; set; }

        [Required]
        [JsonProperty(PropertyName = "last_occupation")]
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string LastOccupation { get; set; }

        [Required]
        [JsonProperty(PropertyName = "organisation_care_before_death_location_id")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        public string OrganisationCareBeforeDeathLocationId { get; set; }

        // Initial thinking is that below is the location ID that is used for authorisation and permission queries 
        [Required]
        [JsonProperty(PropertyName = "location_id")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        public string DeathOccuredLocationId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "mode_of_disposal")]
        [DataType(DataType.Text)]
        [StringLength(250)]
        public ModeOfDisposal ModeOfDisposal { get; set; }

        [Required]
        [JsonProperty(PropertyName = "funderal_directors")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        public string FuneralDirectors { get; set; }

        // Personal affects 
        [Required]
        [JsonProperty(PropertyName = "personal_affects_collected")]
        public bool PersonalAffectsCollected { get; set; }

        [Required]
        [JsonProperty(PropertyName = "personal_affects_details")]
        public string PersonalAffectsDetails { get; set; }

        [Required]
        [JsonProperty(PropertyName = "jewellery_collected")]
        public bool JewelleryCollected { get; set; }

        [Required]
        [JsonProperty(PropertyName = "jewellery_details")]
        public string JewelleryDetails { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [JsonProperty(PropertyName = "date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [JsonProperty(PropertyName = "date_of_death")]
        public DateTimeOffset DateOfDeath { get; set; }

        // Flags that effect priority 
        [Required]
        [JsonProperty(PropertyName = "faith_priority")]
        public bool FaithPriority { get; set; }

        [Required]
        [JsonProperty(PropertyName = "child_priority")]
        public bool ChildPriority { get; set; }

        [Required]
        [JsonProperty(PropertyName = "coroner_priority")]
        public bool CoronerPriority { get; set; }

        [Required]
        [JsonProperty(PropertyName = "other_priority")]
        public bool OtherPriority { get; set; }

        [Required]
        [JsonProperty(PropertyName = "priority_details")]
        public string PriorityDetails { get; set; }

        // Status Fields 
        [Required]
        [JsonProperty(PropertyName = "completed")]
        public bool Completed { get; set; }

        [Required]
        [JsonProperty(PropertyName = "coroner_status")]
        public CoronerStatus CoronerStatus { get; set; }
        
        public Examination()
        {
            Completed = false;
        }
    }
}