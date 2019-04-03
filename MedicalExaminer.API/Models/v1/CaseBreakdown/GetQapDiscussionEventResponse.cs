using System.Collections.Generic;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class GetQapDiscussionEventResponse : ResponseBase
    {
        /// <summary>
        /// these are the BereavedDiscussion events
        /// </summary>
        public IEnumerable<QapDiscussionEvent> Events { get; set; }
    }
}
