using System.ComponentModel.DataAnnotations;
using MedicalExaminer.API.Attributes;
using MedicalExaminer.API.Authorization.ExaminationContext;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.MedicalTeams
{
    /// <summary>
    ///     Put Medical Team Request.
    /// </summary>
    public class PutMedicalTeamRequest : IExaminationValidationModel
    {
        /// <summary>
        ///     Consultant primarily responsible for care of patient.
        /// </summary>
        [Required]
        public ClinicalProfessionalItem ConsultantResponsible { get; set; }

        /// <summary>
        ///     Other consultants involved in care of the patient.
        /// </summary>
        public ClinicalProfessionalItem[] ConsultantsOther { get; set; }

        /// <summary>
        ///     Consultant primarily responsible for care.
        /// </summary>
        public ClinicalProfessionalItem GeneralPractitioner { get; set; }

        /// <summary>
        ///     Clinician responsible for certification.
        /// </summary>
        public ClinicalProfessionalItem Qap { get; set; }

        /// <summary>
        ///     Nursing information.
        /// </summary>
        public string NursingTeamInformation { get; set; }

        /// <summary>
        ///     Medical Examiner.
        /// </summary>
        [ValidRoleOnExamination(UserRoles.MedicalExaminer)]
        [IsNullOrGuid]
        public string MedicalExaminerUserId { get; set; }

        /// <summary>
        ///     Medical Examiner Officer.
        /// </summary>
        [ValidRoleOnExamination(UserRoles.MedicalExaminerOfficer)]
        [IsNullOrGuid]
        public string MedicalExaminerOfficerUserId { get; set; }
    }
}