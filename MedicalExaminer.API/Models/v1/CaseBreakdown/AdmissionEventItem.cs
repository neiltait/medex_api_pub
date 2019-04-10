using System;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class AdmissionEventItem : IEvent
    {
        
            /// <summary>
            /// Event Identification.
            /// </summary>
            public string EventId { get; set; }

            /// <summary>
            /// User Identifier.
            /// </summary>
            public string UserId { get; set; }

            /// <summary>
            /// Event Text (Length to be confirmed).
            /// </summary>
            public string Notes { get; set; }

            /// <summary>
            /// IsFinal, final = true, draft = false
            /// </summary>
            public bool IsFinal { get; set; }

            /// <summary>
            /// the type of event this is
            /// </summary>
            public EventType EventType => EventType.Admission;

        /// <summary>
        /// date of last admission, if known
        /// </summary>
        public DateTime? AdmittedDate { get; set; }

        /// <summary>
        /// time of last admission, if known
        /// </summary>
        public TimeSpan? AdmittedTime { get; set; }

        /// <summary>
        /// Do you suspect this case may need Immediate Coroner Referral Yes = true, No = false
        /// </summary>
        public bool ImmediateCoronerReferral { get; set; }

        /// <summary>
        /// Date event was created
        /// </summary>
        public DateTime? Created { get; set; }

    }
}
