using System.Collections.Generic;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class EventContainerItem<TEventType>
    {
        public IEnumerable<TEventType> History { get; set; }
        public TEventType Latest { get; set; }
        public TEventType UsersDraft { get; set; }
    }
}
