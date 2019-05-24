using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    /// <inheritdoc />
    public class MedicalTeam : IMedicalTeam
    {
        /// <inheritdoc />
        [Required]
        [JsonProperty(PropertyName = "consultant_responsible")]
        public ClinicalProfessional ConsultantResponsible { get; set; }

        /// <inheritdoc />
        [Required]
        [JsonProperty(PropertyName = "consultants_other")]
        public ClinicalProfessional[] ConsultantsOther { get; set; }

        /// <inheritdoc />
        [Required]
        [JsonProperty(PropertyName = "general_practitioner")]
        public ClinicalProfessional GeneralPractitioner { get; set; }

        /// <inheritdoc />
        [Required]
        [JsonProperty(PropertyName = "qap")]
        public ClinicalProfessional Qap { get; set; }

        /// <inheritdoc />
        [JsonProperty(PropertyName = "nursing_team_information")]
        public string NursingTeamInformation { get; set; }

        /// <inheritdoc />
        [Required]
        [JsonProperty(PropertyName = "medical_examiner")]
        public string MedicalExaminerUserId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "medical_examiner_full_name")]
        public string MedicalExaminerFullName { get; set; }

        /// <inheritdoc />
        [Required]
        [JsonProperty(PropertyName = "medical_examiner_officer")]
        public string MedicalExaminerOfficerUserId { get; set; }

        /// <inheritdoc />
        [Required]
        [JsonProperty(PropertyName = "medical_examiner_officer_full_name")]
        public string MedicalExaminerOfficerFullName { get; set; }
    }
}