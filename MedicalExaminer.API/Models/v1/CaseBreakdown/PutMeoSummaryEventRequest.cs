namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class PutMeoSummaryEventRequest
    {
        /// <summary>
        /// Event Identification.
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// User Identifier.
        /// </summary>
        public string UserId { get; set; }

        //***************** uncomment after the generic event classes approved *********************
        ///// <summary>
        ///// Dictionary for the status (Draft or Final).
        ///// </summary>
        //public EventStatus EventStatus { get; set; }

        ///// <summary>
        ///// list of amendments for this item
        ///// </summary>
        //public IEnumerable<IEvent> Amendments { get; set; }

        ///// <summary>
        ///// the type of event this is
        ///// </summary>
        //public EventType EventType => EventType.BereavedDiscussion;

        /// <summary>
        /// Details of the MEO Summary.
        /// </summary>
        public string SummaryDetails { get; set; }
    }
}
