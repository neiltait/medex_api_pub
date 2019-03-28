using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class GetPreScrutinyEventResponse : ResponseBase
    {
        /// <summary>
        /// These are the Pre-Scrutiny Events.
        /// </summary>
        public IEnumerable<PreScrutinyEvent> PreScrutinyEvents { get; set; }
    }
}
