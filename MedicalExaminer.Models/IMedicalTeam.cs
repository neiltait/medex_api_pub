namespace MedicalExaminer.Models
{
    /// <summary>
    ///     Details of medical team associated with medical examination
    /// </summary>
    public interface IMedicalTeam
    {
        /// <summary>
        ///     Consultant primarily responsible for care
        /// </summary>
        ClinicalProfessional ConsultantResponsible { get; set; }

        /// <summary>
        ///     Other consultants involved in care of the patient
        /// </summary>
        ClinicalProfessional[] ConsultantsOther { get; set; }

        /// <summary>
        ///     General practitioner responsible for patient
        /// </summary>
        ClinicalProfessional GeneralPractitioner { get; set; }

        /// <summary>
        ///     Clinician responsible for certification
        /// </summary>
        ClinicalProfessional Qap { get; set; }

        /// <summary>
        ///     Nursing team information
        /// </summary>
        string NursingTeamInformation { get; set; }

        /// <summary>
        ///     Medical Examiner
        /// </summary>
        string MedicalExaminerUserId { get; set; }

        /// <summary>
        ///     Medical Examiner Officer
        /// </summary>
        string MedicalExaminerOfficerUserId { get; set; }
    }
}