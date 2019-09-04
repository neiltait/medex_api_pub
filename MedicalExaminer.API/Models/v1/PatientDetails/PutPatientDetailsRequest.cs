using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MedicalExaminer.API.Attributes;
using MedicalExaminer.API.Authorization.ExaminationContext;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.PatientDetails
{
    /// <summary>
    ///     Put Patient Details Request Object.
    /// </summary>
    public class PutPatientDetailsRequest : IExaminationValidationModel
    {
        /// <summary>
        ///     Is the case a cultural priority?.
        /// </summary>
        public bool CulturalPriority { get; set; }

        /// <summary>
        ///     Further details regarding the patients gender.
        /// </summary>
        public string GenderDetails { get; set; }

        /// <summary>
        ///     Is the case a faith priority?.
        /// </summary>
        public bool FaithPriority { get; set; }

        /// <summary>
        ///     Is the case a child priority?.
        /// </summary>
        public bool ChildPriority { get; set; }

        /// <summary>
        ///     Is the case a coroner priority?.
        /// </summary>
        public bool CoronerPriority { get; set; }

        /// <summary>
        ///     Is the case a priority for some other reason?.
        /// </summary>
        public bool OtherPriority { get; set; }

        /// <summary>
        ///     Further details as to why the case is a priority.
        /// </summary>
        public string PriorityDetails { get; set; }

        /// <summary>
        ///     The cases coroner status.
        /// </summary>
        public CoronerStatus CoronerStatus { get; set; }

        /// <summary>
        ///     Location where the death occured.
        /// </summary>
        public string PlaceDeathOccured { get; set; }

        /// <summary>
        ///     The medical examiners office responsible for investigating the death.
        /// </summary>
        [ValidLocation]
        public string MedicalExaminerOfficeResponsible { get; set; }

        /// <summary>
        ///     Gender of the patient.
        /// </summary>
        public ExaminationGender Gender { get; set; }

        /// <summary>
        ///     Details of the patients date of birth.
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        ///     Details of the patients date of death.
        /// </summary>
        public DateTime? DateOfDeath { get; set; }

        /// <summary>
        ///     Patients NHS Number.
        /// </summary>
        [ValidNhsNumberNullAllowed]
        [PutUniqueNhsNumber]
        public string NhsNumber { get; set; }

        /// <summary>
        ///     Patients first hospital number.
        /// </summary>
        public string HospitalNumber_1 { get; set; }

        /// <summary>
        ///     Patients second hospital number.
        /// </summary>
        public string HospitalNumber_2 { get; set; }

        /// <summary>
        ///     Patients third hospital number.
        /// </summary>
        public string HospitalNumber_3 { get; set; }

        /// <summary>
        ///     Patients time of death.
        /// </summary>
        public TimeSpan? TimeOfDeath { get; set; }

        /// <summary>
        ///     Patients surname
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = nameof(SystemValidationErrors.Required))]
        [MinLength(1, ErrorMessage = nameof(SystemValidationErrors.MinimumLengthOf1))]
        [MaxLength(150, ErrorMessage = nameof(SystemValidationErrors.MaximumLength150))]
        public string Surname { get; set; }

        /// <summary>
        ///     Patients given names
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = nameof(SystemValidationErrors.Required))]
        [MinLength(1, ErrorMessage = nameof(SystemValidationErrors.MinimumLengthOf1))]
        [MaxLength(150, ErrorMessage = nameof(SystemValidationErrors.MaximumLength150))]
        public string GivenNames { get; set; }

        /// <summary>
        ///     Patients Postcode.
        /// </summary>
        public string PostCode { get; set; }

        /// <summary>
        ///     First line of patients address.
        /// </summary>
        public string HouseNameNumber { get; set; }

        /// <summary>
        ///     Second line of patients address.
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        ///     Patients town or city.
        /// </summary>
        public string Town { get; set; }

        /// <summary>
        ///     Patients county.
        /// </summary>
        public string County { get; set; }

        /// <summary>
        ///     Patients country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        ///     Free text for any relevant patient occupation details.
        /// </summary>
        public string LastOccupation { get; set; }

        /// <summary>
        ///     Organisation responsible for patient at time of death.
        /// </summary>
        public string OrganisationCareBeforeDeathLocationId { get; set; }

        /// <summary>
        ///     Patients funeral arrangements.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = nameof(SystemValidationErrors.Required))]
        public ModeOfDisposal ModeOfDisposal { get; set; }

        /// <summary>
        ///     Does the patient have any implants that may impact on cremation.
        /// </summary>
        public bool? AnyImplants { get; set; }

        /// <summary>
        ///     Free text for the implant details.
        /// </summary>
        [RequiredIfAttributesMatch(nameof(AnyImplants), true, ErrorMessage = nameof(SystemValidationErrors.Required))]
        public string ImplantDetails { get; set; }

        /// <summary>
        ///     Details of the patients funeral directors.
        /// </summary>
        public string FuneralDirectors { get; set; }

        /// <summary>
        ///     Does the patient have any personal effects.
        /// </summary>
        public bool AnyPersonalEffects { get; set; }

        /// <summary>
        ///     Free text details of any personal effects.
        /// </summary>
        public string PersonalEffectDetails { get; set; }

        /// <summary>
        ///     An array of representatives for the patient.
        /// </summary>
        public IEnumerable<RepresentativeItem> Representatives { get; set; }
    }
}