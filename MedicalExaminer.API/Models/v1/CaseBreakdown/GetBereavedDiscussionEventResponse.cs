using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class GetBereavedDiscussionEventResponse : ResponseBase
    {
        /// <summary>
        /// these are the BereavedDiscussion events
        /// </summary>
        public IEnumerable<BereavedDiscussionEvent> Events { get; set; }
    }
}
