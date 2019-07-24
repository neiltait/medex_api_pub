using System.Collections.Generic;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class EventContainerItem<TEventType, TPrepopulated>
    {
        public IEnumerable<TEventType> History { get; set; }

        public TEventType Latest { get; set; }

        public TEventType UsersDraft { get; set; }

        public TPrepopulated Prepopulated { get; set; }
    }
}
