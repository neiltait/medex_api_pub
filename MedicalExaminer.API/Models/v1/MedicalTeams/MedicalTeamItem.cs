using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Models.v1.MedicalTeams
{
    /// <summary>
    ///     Instance if MedicalTeamItem, subset of Examination.
    /// </summary>
    public class MedicalTeamItem
    {
        /// <summary>
        ///     Consultant primarily responsible for care.
        /// </summary>
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
        public UserItem MedicalExaminer { get; set; }

        /// <summary>
        ///     Medical Examiner Officer.
        /// </summary>
        public UserItem MedicalExaminerOfficer { get; set; }
    }
}