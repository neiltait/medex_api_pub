using System;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class AdmissionEventItem : IEvent
    {
        /// <summary>
        /// Users full name
        /// </summary>
        public string UserFullName { get; set; }

        /// <summary>
        /// Users Role
        /// </summary>
        public string UsersRole { get; set; }

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
        /// date of last admission - Unknown or not
        /// </summary>
        public bool? AdmittedDateUnknown { get; set; }

        /// <summary>
        /// time of last admission, if known
        /// </summary>
        public TimeSpan? AdmittedTime { get; set; }

        /// <summary>
        /// time of last admission - Unknown or not
        /// </summary>
        public bool? AdmittedTimeUnknown { get; set; }

        /// <summary>
        /// Do you suspect this case may need Immediate Coroner Referral Yes = true, No = false
        /// </summary>
        public bool? ImmediateCoronerReferral { get; set; }

        /// <summary>
        /// Date event was created
        /// </summary>
        public DateTime? Created { get; set; }

        /// <summary>
        /// The route of admission
        /// </summary>
        public RouteOfAdmission? RouteOfAdmission { get; set; }

    }
}
