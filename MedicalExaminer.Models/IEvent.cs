using System.Collections.Generic;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Models
{
    public interface IEvent
    {
        EventType EventType { get; }
        string EventId { get; }
        EventStatus EventStatus { get; }
        string UserId { get; }
        IEnumerable<IEvent> Amendments { get; set; }
    }
}
