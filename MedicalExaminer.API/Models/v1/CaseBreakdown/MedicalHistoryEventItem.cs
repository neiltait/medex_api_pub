using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class MedicalHistoryEventItem
    {
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
