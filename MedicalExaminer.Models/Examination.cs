using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class Examination : Record, IExamination
    {
        public Examination()
        {
            Completed = false;
        }

        // Linked Fields
        [DataType(DataType.Custom)]
        [JsonProperty(PropertyName = "patient_details")]
        public PatientDetails PatientDetails { get; set; }

        [Required]
        [DataType(DataType.Custom)]
        [JsonProperty(PropertyName = "medical_team")]
        public MedicalTeam MedicalTeam { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [JsonProperty(PropertyName = "id")]
        public string ExaminationId { get; set; }

        [JsonProperty(PropertyName = "out_of_hours")]
        public bool OutOfHours { get; set; }

        /// <summary>
        ///     Patients first hospital number
        /// </summary>
        [JsonProperty(PropertyName = "hospital_number_1")]
        public string HospitalNumber_1 { get; set; }

        /// <summary>
        ///     Patients second hospital number
        /// </summary>
        [JsonProperty(PropertyName = "hospital_number_2")]
        public string HospitalNumber_2 { get; set; }

        /// <summary>
        ///     Patients third hospital number
        /// </summary>
        [JsonProperty(PropertyName = "hospital_number_3")]
        public string HospitalNumber_3 { get; set; }

        [JsonProperty(PropertyName = "time_of_death")]
        [Required]
        public TimeSpan? TimeOfDeath { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(250)]
        [JsonProperty(PropertyName = "given_names")]
        public string GivenNames { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(250)]
        [JsonProperty(PropertyName = "surname")]
        public string Surname { get; set; }

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
        [JsonProperty(PropertyName = "personal_effects_collected")]
        public bool AnyPersonalEffects { get; set; }

        [Required]
        [JsonProperty(PropertyName = "personal_effects_details")]
        public string PersonalEffectDetails { get; set; }

        [Required]
        [JsonProperty(PropertyName = "place_death_occured")]
        public string PlaceDeathOccured { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [JsonProperty(PropertyName = "date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [JsonProperty(PropertyName = "date_of_death")]
        public DateTimeOffset DateOfDeath { get; set; }

        [Required]
        [JsonProperty(PropertyName = "cultural_priority")]
        public bool CulturalPriority { get; set; }

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
        [DataType(DataType.Text)]
        public string PriorityDetails { get; set; }

        // Status Fields
        [Required]
        [JsonProperty(PropertyName = "completed")]
        public bool Completed { get; set; }

        [Required]
        [JsonProperty(PropertyName = "coroner_status")]
        public CoronerStatus CoronerStatus { get; set; }

        [Required]
        [JsonProperty(PropertyName = "any_implants")]
        public bool AnyImplants { get; set; }

        [JsonProperty(PropertyName = "implant_details")]
        [DataType(DataType.Text)]
        public string ImplantDetails { get; set; }

        [Required]
        [JsonProperty(PropertyName = "medical_examiner_office_responsible")]
        [DataType(DataType.Text)]
        public string MedicalExaminerOfficeResponsible { get; set; }

        [JsonProperty(PropertyName = "gender_details")]
        [DataType(DataType.Text)]
        public string GenderDetails { get; set; }

        public IEnumerable<Representative> Representatives { get; set; }
    }
}