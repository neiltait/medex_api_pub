using System;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class AdmissionEventItem
    {
        public class AdmissionEvent
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
            /// the type of event this is
            /// </summary>
            public DateTime AdmittedDateTime { get; set; }

            /// <summary>
            /// Do you suspect this case may need Immediate Coroner Referral Yes = true, No = false
            /// </summary>
            public bool ImmediateCoronerReferral { get; set; }
        }
    }
}
