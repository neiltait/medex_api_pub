using MedicalExaminer.API.Attributes;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    /// <summary>
    ///     Put Other Event Request Object.
    /// </summary>
    public class PutOtherEventRequest
    {
        /// <summary>
        /// Event Id
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// Event Text (Length to be confirmed).
        /// </summary>
        [RequiredIfAttributesMatch(nameof(EventStatus), EventStatus.Final)]
        public string Text { get; set; }

        /// <summary>
        /// Enum for the status (Draft or Final).
        /// </summary>
        public EventStatus EventStatus { get; set; }
    }
}