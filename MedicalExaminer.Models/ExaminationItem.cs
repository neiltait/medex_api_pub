using System;
using System.ComponentModel.DataAnnotations;
using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class ExaminationItem : IExaminationItem
    {
        /// <summary>
        /// Examination Id
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        /// <summary>
        ///  Where the death occured
        /// </summary>
        [DataType(DataType.Text)]
        [JsonProperty(PropertyName = "place_death_occured")]
        public string PlaceDeathOccured { get; set; }

        /// <summary>
        /// Medical Examiner Office Responsible for dealing with the examination
        /// </summary>
        [DataType(DataType.Text)]
        [JsonProperty(PropertyName = "medical_examiner_office_responsible")]
        public string MedicalExaminerOfficeResponsible { get; set; }

        /// <summary>
        /// Patients surname
        /// </summary>
        [JsonProperty(PropertyName = "surname")]
        public string Surname { get; set; }

        /// <summary>
        /// Patients given names
        /// </summary>
        [JsonProperty(PropertyName = "given_names")]
        public string GivenNames { get; set; }

        /// <summary>
        /// Gender patient identifies as
        /// </summary>
        [JsonProperty(PropertyName = "examination_gender")]
        public ExaminationGender? Gender { get; set; }

        /// <summary>
        /// Comments regarding the patients gender identification
        /// </summary>
        [JsonProperty(PropertyName = "gender_details")]
        public string GenderDetails { get; set; }

        /// <summary>
        /// Patients NHS Number
        /// </summary>
        [JsonProperty(PropertyName = "nhs_number")]
        public string NhsNumber { get; set; }

        /// <summary>
        /// Patients first hospital number
        /// </summary>
        [JsonProperty(PropertyName = "hospital_number_1")]
        public string HospitalNumber_1 { get; set; }

        /// <summary>
        /// Patients second hospital number
        /// </summary>
        [JsonProperty(PropertyName = "hospital_number_2")]
        public string HospitalNumber_2 { get; set; }

        /// <summary>
        /// Patients third hospital number
        /// </summary>
        [JsonProperty(PropertyName = "hospital_number_3")]
        public string HospitalNumber_3 { get; set; }

        /// <summary>
        /// Patients date of birth
        /// </summary>
        [JsonProperty(PropertyName = "date_of_birth")]
        public DateTime? DateOfBirth { get; set; }

        
        /// <summary>
        /// Patients date of death
        /// </summary>
        [JsonProperty(PropertyName = "date_of_death")]
        public DateTime? DateOfDeath { get; set; }

        
        /// <summary>
        /// Patients time of death
        /// </summary>
        [JsonProperty(PropertyName = "time_of_death")]
        public TimeSpan? TimeOfDeath { get; set; }

        /// <summary>
        /// Out of hours/urgent scrutiny has taken place out of hours
        /// </summary>
        [JsonProperty(PropertyName = "out_of_hours")]
        public bool OutOfHours { get; set; }
    }
}
