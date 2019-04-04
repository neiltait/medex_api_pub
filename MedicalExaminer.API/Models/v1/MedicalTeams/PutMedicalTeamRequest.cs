using System.ComponentModel.DataAnnotations;
using MedicalExaminer.API.Attributes;

namespace MedicalExaminer.API.Models.v1.MedicalTeams
{
    /// <summary>
    ///     Put Medical Team Request.
    /// </summary>
    public class PutMedicalTeamRequest
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
        [ValidMedicalExaminer]
        public string MedicalExaminerUserId { get; set; }

        /// <summary>
        ///     Medical Examiner Officer.
        /// </summary>
        [ValidMedicalExaminerOfficer]
        public string MedicalExaminerOfficerUserId { get; set; }
    }
}