using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class PatientDetailsHeader
    {
        /// <summary>
        /// Patients given names.
        /// </summary>
        [JsonProperty(PropertyName = "patient_given_names")]
        public string PatientGivenNames { get; set; }

        /// <summary>
        /// Patients surname.
        /// </summary>
        [JsonProperty(PropertyName = "patient_surname")]
        public string Surname { get; set; }

        /// <summary>
        /// Patients NHS Number.
        /// </summary>
        [JsonProperty(PropertyName = "nhs_number")]
        public string NhsNumber { get; set; }

        /// <summary>
        /// Details of the patient date of death.
        /// </summary>
        [JsonProperty(PropertyName = "date_of_death")]
        public DateTime DateOfDeath { get; set; }

        /// <summary>
        /// Case Status - Admission Notes Have Been Added
        /// </summary>
        [JsonProperty(PropertyName = "admission_notes_have_been_added")]
        public bool AdmissionNotesHaveBeenAdded { get; set; }

        /// <summary>
        /// Case Status - Ready For ME Scrutiny
        /// </summary>
        [JsonProperty(PropertyName = "ready_for_me_scrutiny")]
        public bool ReadyForMEScrutiny { get; set; }

        /// <summary>
        /// Case Status - Unassigned
        /// </summary>
        [JsonProperty(PropertyName = "unassigned")]
        public bool Unassigned { get; set; }

        /// <summary>
        /// Case Status - Have Been Scrutinised By ME
        /// </summary>
        [JsonProperty(PropertyName = "have_been_scrutinised_by_me")]
        public bool HaveBeenScrutinisedByME { get; set; }

        /// <summary>
        /// Case Status - Pending Admission Notes
        /// </summary>
        [JsonProperty(PropertyName = "pending_admission_notes")]
        public bool PendingAdmissionNotes { get; set; }

        /// <summary>
        /// Case Status - Pending Discussion With QAP
        /// </summary>
        [JsonProperty(PropertyName = "pending_discussion_with_qap")]
        public bool PendingDiscussionWithQAP { get; set; }

        /// <summary>
        /// Case Status - Pending Discussion With Representative
        /// </summary>
        [JsonProperty(PropertyName = "pending_discussion_with_representative")]
        public bool PendingDiscussionWithRepresentative { get; set; }

        /// <summary>
        /// Case Status - Have Final Case Outstanding Outcomes
        /// </summary>
        [JsonProperty(PropertyName = "have_final_case_outstanding_outcomes")]
        public bool HaveFinalCaseOutstandingOutcomes { get; set; }
    }
}
