using System.Collections.Generic;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Models
{
    public class CaseBreakDown
    {
        public BaseEventContainter<OtherEvent> OtherEvents { get; set; }
        public BaseEventContainter<NoteEvent> NoteworthyEvents { get; set; }
    }

    public class NoteEvent : IEvent
    {
        public string EventId { get; set; }

        public EventStatus EventStatus { get; set; }

        public string UserId { get; set; }

    public IEnumerable<IEvent> Amendments { get; set; }
        public string SomethingDifferent { get; set; }

        public EventType EventType => EventType.Notes;
    }
}
