using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.Models;
using Newtonsoft.Json;

namespace MedicalExaminer.API.Models.v1.Examinations
{
    public class MedicalTeamItem
    {
        /// <summary>
        /// Consultant primarily responsible for care
        /// </summary>
        public ClinicalProfessional ConsultantResponsible { get; set; }

        /// <summary>
        /// Other consultants involved in care of the patient
        /// </summary>
        public ClinicalProfessional[] ConsultantsOther { get; set; }

        /// <summary>
        /// Consultant primarily responsible for care
        /// </summary>
        public ClinicalProfessional GeneralPractitioner { get; set; }

        /// <summary>
        /// Clinician responsible for certification
        /// </summary>
        public ClinicalProfessional Qap { get; set; }

        /// <summary>
        /// Nursing information
        /// </summary>
        public string NursingTeamInformation { get; set; }

        /// <summary>
        /// Medical Examiner
        /// </summary>
        public UserItem MedicalExaminer { get; set; }

        /// <summary>
        /// Medical Examiner Officer
        /// </summary>
        public UserItem MedicalExaminerOfficer { get; set; }
    }
}
