using MedicalExaminer.API.Attributes;

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
        [RequiredIfAttributesMatch(nameof(IsFinal), true)]
        public string Text { get; set; }

        /// <summary>
        /// Draft is false, final true
        /// </summary>
        public bool IsFinal { get; set; }
    }
}