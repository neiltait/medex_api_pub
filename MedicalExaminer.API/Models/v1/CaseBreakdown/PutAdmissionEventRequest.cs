using System;
using MedicalExaminer.API.Attributes;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    /// <summary>
    ///     Put Other Event Request Object.
    /// </summary>
    public class PutAdmissionEventRequest
    {
        /// <summary>
        /// Event Id
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// Event Text (Length to be confirmed).
        /// </summary>
        [RequiredIfAttributesMatch(nameof(IsFinal), true)]
        public string Notes { get; set; }

        /// <summary>
        /// Draft is false, final true
        /// </summary>
        public bool IsFinal { get; set; }

        /// <summary>
        /// the type of event this is
        /// </summary>
        public DateTime AdmittedDateTime { get; set; }

        /// <summary>
        /// Do you suspect this case may need Immediate Coroner Referral Yes = true, No = false
        /// </summary>
        public bool ImmediateCoronerReferral { get; set; }
    }
}