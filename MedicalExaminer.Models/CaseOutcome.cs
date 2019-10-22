using System;
using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class CaseOutcome
    {
        /// <summary>
        /// Case Outcome Summary
        /// </summary>
        [JsonProperty(PropertyName = "case_outcome_summary")]
        public CaseOutcomeSummary? CaseOutcomeSummary { get; set; }

        /// <summary>
        /// Outcome Of Representative Discussion
        /// </summary>
        [JsonProperty(PropertyName = "outcome_of_representative_discussion")]
        public BereavedDiscussionOutcome? OutcomeOfRepresentativeDiscussion { get; set; }

        /// <summary>
        /// Outcome Of Pre-scrutiny
        /// </summary>
        [JsonProperty(PropertyName = "outcome_of_prescrutiny")]
        public OverallOutcomeOfPreScrutiny? OutcomeOfPrescrutiny { get; set; }

        /// <summary>
        /// Outcome Qap Discussion
        /// </summary>
        [JsonProperty(PropertyName = "outcome_qap_discussion")]
        public QapDiscussionOutcome? OutcomeQapDiscussion { get; set; }

        /// <summary>
        /// Case Completed?
        /// </summary>
        [JsonProperty(PropertyName = "case_completed")]
        public bool CaseCompleted { get; set; }

        /// <summary>
        /// Scrutiny Confirmed On
        /// </summary>
        [JsonProperty(PropertyName = "scrutiny_confirmed_on")]
        public DateTime? ScrutinyConfirmedOn { get; set; }

        /// <summary>
        /// has the coroners referral been sent?
        /// </summary>
        [JsonProperty(PropertyName = "coroner_referral_sent")]
        public bool CoronerReferralSent { get; set; }

        /// <summary>
        /// Case Medical Examiner Full Name
        /// </summary>
        [JsonProperty(PropertyName = "case_medical_examiner_fullName")]
        public string CaseMedicalExaminerFullName { get; set; }

        /// <summary>
        /// MCCD Issued?
        /// </summary>
        [JsonProperty(PropertyName = "mccd_issued")]
        public bool? MccdIssued { get; set; }

        /// <summary>
        /// Cremation Form Status
        /// </summary>
        [JsonProperty(PropertyName = "cremation_form_status")]
        public CremationFormStatus? CremationFormStatus { get; set; }

        /// <summary>
        /// GP Notified Status
        /// </summary>
        [JsonProperty(PropertyName = "gp_notified_status")]
        public GPNotified? GpNotifiedStatus { get; set; }
    }
}
