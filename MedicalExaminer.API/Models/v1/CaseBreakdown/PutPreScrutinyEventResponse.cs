using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class PutPreScrutinyEventResponse : ResponseBase
    {
        /// <summary>
        ///     The id of the new Pre-Scrutiny Event.
        /// </summary>
        public string EventId { get; set; }
    }
}
