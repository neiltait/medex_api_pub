using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using System;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class MedicalHistoryEventItem : IEvent
    {
        /// <summary>
        /// Date event was created
        /// </summary>
        public DateTime? Created { get; set; }

        /// <summary>
        /// Event Identification.
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// User Identifier.
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// the type of event this is
        /// </summary>
        public EventType EventType => EventType.MedicalHistory;

        /// <summary>
        /// Draft is false, final true
        /// </summary>
        public bool IsFinal { get; }

        /// <summary>
        /// Event Text (Length to be confirmed).
        /// </summary>
        public string Text { get; set; }
    }
}
