using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MedicalExaminer.API.Attributes;
using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;

namespace MedicalExaminer.API.Models.v1.PatientDetails
{
    public class PutPatientDetailsRequest
    {
        public bool CulturalPriority { get; set; }
        public string GenderDetails { get; set; }
        public bool FaithPriority { get; set; }
        public bool ChildPriority { get; set; }
        public bool CoronerPriority { get; set; }
        public bool OtherPriority { get; set; }
        public string PriorityDetails { get; set; }
        public bool Completed { get; set; }
        public CoronerStatus CoronerStatus { get; set; }
        public bool OutOfHours { get; set; }
        public string PlaceDeathOccured { get; set; }
        public string MedicalExaminerOfficeResponsible { get; set; }
        public ExaminationGender Gender { get; set; }
        /// <summary>
        /// Details of the patients date of birth
        /// </summary>
        public DateTime DateOfBirth { get; set; }
        /// <summary>
        /// Details of the patients date of death
        /// </summary>
        public DateTime DateOfDeath { get; set; }
        /// <summary>
        /// Patients NHS Number
        /// </summary>
        public string NhsNumber { get; set; }
        /// <summary>
        /// Patients first hospital number
        /// </summary>
        public string HospitalNumber_1 { get; set; }

        /// <summary>
        /// Patients second hospital number
        /// </summary>
        public string HospitalNumber_2 { get; set; }

        /// <summary>
        /// Patients third hospital number
        /// </summary>
        public string HospitalNumber_3 { get; set; }
        /// <summary>
        /// Patients time of death
        /// </summary>
        public TimeSpan? TimeOfDeath { get; set; }
        /// <summary>
        /// patients given names
        /// </summary>
        public string GivenNames { get; set; }
        /// <summary>
        /// patients surname
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// Patients Postcode
        /// </summary>
        [Required]
        public string PostCode { get; set; }

        /// <summary>
        /// First line of patients addess
        /// </summary>
        [Required]
        public string HouseNameNumber { get; set; }

        /// <summary>
        /// Second line of patients address
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Patients town or city
        /// </summary>
        [Required]
        public string Town { get; set; }

        /// <summary>
        /// Patients county
        /// </summary>
        [Required]
        public string County { get; set; }

        /// <summary>
        /// Patients country
        /// </summary>
        [Required]
        public string Country { get; set; }

        /// <summary>
        /// Free text for any relevant patient occupation details
        /// </summary>
        public string LastOccupation { get; set; }

        /// <summary>
        /// Organisation responsible for patient at time of death
        /// </summary>
        public string OrganisationCareBeforeDeathLocationId { get; set; }

        /// <summary>
        /// Patients funeral arrangements
        /// </summary>
        [Required]
        public ModeOfDisposal ModeOfDisposal { get; set; }

        /// <summary>
        /// Does the patient have any implants that may impact on cremation
        /// </summary>
        public bool AnyImplants { get; set; }

        /// <summary>
        /// Free text for the implant details
        /// </summary>
        [RequiredIf(nameof(AnyImplants), true)]
        public string ImplantDetails { get; set; }

        /// <summary>
        /// Details of the patients funeral directors
        /// </summary>
        public string FuneralDirectors { get; set; }

        /// <summary>
        /// Does the patient have any personal effects
        /// </summary>
        public bool AnyPersonalEffects { get; set; }

        /// <summary>
        /// Free text details of any personal effects
        /// </summary>
        public string PersonalEffectDetails { get; set; }

        public IEnumerable<RepresentativeItem> Representatives { get; set; }
    }
}
