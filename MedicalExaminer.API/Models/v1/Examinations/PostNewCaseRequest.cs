using System;
using System.ComponentModel.DataAnnotations;
using MedicalExaminer.API.Attributes;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.Examinations
{
    /// <summary>
    /// </summary>
    public class PostNewCaseRequest
    {
        /// <summary>
        ///     Where the death occured
        /// </summary>
        [Required]
        public string PlaceDeathOccured { get; set; }

        /// <summary>
        ///     Medical Examiner Office Responsible for dealing with the examination
        ///     377e5b2d-f858-4398-a51c-1892973b6537
        /// </summary>
        [ValidMedicalExaminerOffice]
        public string MedicalExaminerOfficeResponsible { get; set; }

        /// <summary>
        ///     Patients surname
        /// </summary>
        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        public string Surname { get; set; }

        /// <summary>
        ///     Patients given names
        /// </summary>
        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        public string GivenNames { get; set; }

        /// <summary>
        ///     Gender patient identifies as
        /// </summary>
        [Required]
        public ExaminationGender? Gender { get; set; }

        /// <summary>
        ///     Comments regarding the patients gender identification
        /// </summary>
        [RequiredIfAttributesMatch(nameof(Gender), ExaminationGender.Other)]
        public string GenderDetails { get; set; }

        /// <summary>
        ///     Patients NHS Number 943 476 5919
        /// </summary>
        [ValidNhsNumberNullAllowed]
        public string NhsNumber { get; set; }

        /// <summary>
        ///     Patients first hospital number
        /// </summary>
        public string HospitalNumber_1 { get; set; }

        /// <summary>
        ///     Patients second hospital number
        /// </summary>
        public string HospitalNumber_2 { get; set; }

        /// <summary>
        ///     Patients third hospital number
        /// </summary>
        public string HospitalNumber_3 { get; set; }

        /// <summary>
        ///     Patients date of birth
        /// </summary>
        [DateIsLessThanOrEqualToNullsAllowed(nameof(DateOfDeath))]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        ///     Patients date of death
        /// </summary>
        public DateTime? DateOfDeath { get; set; }

        /// <summary>
        ///     Patients time of death
        /// </summary>
        public TimeSpan? TimeOfDeath { get; set; }

        /// <summary>
        ///     Out of hours/urgent scrutiny has taken place out of hours
        /// </summary>
        public bool OutOfHours { get; set; }
    }
}