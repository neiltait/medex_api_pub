namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class PutMedicalHistoryEventRequest
    {
        /// <summary>
        /// Event Identification.
        /// </summary>
        public string EventId { get; set; }


        /// <summary>
        /// Dictionary for the status (Draft or Final).
        /// </summary>
        public bool IsFinal { get; set; }

        /// <summary>
        /// Event Text
        /// </summary>
        public string Text { get; set; }
    }
}
