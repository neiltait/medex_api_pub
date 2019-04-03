namespace MedicalExaminer.API.Models.v1.MedicalTeams
{
    /// <summary>
    ///     The response object when a new case is added.
    /// </summary>
    public class PutMedicalTeamResponse : ResponseBase
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
        public string MedicalExaminerUserId { get; set; }

        /// <summary>
        ///     Medical Examiner Officer.
        /// </summary>
        public string MedicalExaminerOfficerUserId { get; set; }
    }
}