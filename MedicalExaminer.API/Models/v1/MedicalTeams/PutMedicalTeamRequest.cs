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
        public ClinicalProfessional ConsultantResponsible { get; set; }

        /// <summary>
        ///     Other consultants involved in care of the patient.
        /// </summary>
        public ClinicalProfessional[] ConsultantsOther { get; set; }

        /// <summary>
        ///     Consultant primarily responsible for care.
        /// </summary>
        public ClinicalProfessional GeneralPractitioner { get; set; }

        /// <summary>
        ///     Clinician responsible for certification.
        /// </summary>
        public ClinicalProfessional Qap { get; set; }

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