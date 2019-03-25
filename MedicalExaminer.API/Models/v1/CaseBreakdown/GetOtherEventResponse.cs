using MedicalExaminer.Models;
using System.Collections.Generic;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class GetOtherEventResponse : ResponseBase
    {
        /// <summary>
        /// these are the other events
        /// </summary>
        public IEnumerable<OtherEvent> Events { get; set; }
    }
}
