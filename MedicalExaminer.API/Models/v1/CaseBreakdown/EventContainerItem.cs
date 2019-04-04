using System.Collections.Generic;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class EventContainerItem
    {
        public IEnumerable<IEvent> History { get; set; }
        public IEvent Latest { get; set; }
        public IEvent UsersDraft { get; set; }
    }
}
