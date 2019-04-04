namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class PutQapDiscussionEventResponse : ResponseBase
    {
        /// <summary>
        ///     The id of the new case
        /// </summary>
        public string EventId { get; set; }
    }
}
