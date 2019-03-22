using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class PostOtherEventResponse : ResponseBase
    {
        /// <summary>
        ///     The id of the new case
        /// </summary>
        public string EventId { get; set; }
    }
}
