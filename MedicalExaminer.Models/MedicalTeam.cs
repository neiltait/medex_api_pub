using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    /// <summary>
    /// Details of medical team associated with medical examination
    /// </summary>
    public class MedicalTeam
    {
        /// <summary>
        /// Consultant primarily responsible for care
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "consultant_responsible")]
        public ClinicalProfessional ConsultantResponsible { get; set; }

        /// <summary>
        /// Other consultants involved in care of the patient
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "consultants_other")]
        public ClinicalProfessional[] ConsultantsOther { get; set; }

        /// <summary>
        /// Consultant primarily responsible for care
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "general_practitioner")]
        public ClinicalProfessional GeneralPractitioner { get; set; }

        /// <summary>
        /// Clinician responsible for certification
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "qap")]
        public ClinicalProfessional Qap { get; set; }

        /// <summary>
        /// Nursing information
        /// </summary>
        [JsonProperty(PropertyName = "nursing_team_information")]
        public string NursingTeamInformation { get; set; }

        /// <summary>
        /// Medical Examiner Id
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "medical_examiner")]
        public string MedicalExaminer { get; set; }

        /// <summary>
        /// Medical Examiner Officer Id
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "medical_examiner_officer")]
        public string MedicalExaminerOfficer { get; set; }
    }
}
