using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Models
{
    public interface IEvent
    {
        EventType EventType { get; }
        string EventId { get; set; }
        EventStatus EventStatus { get; }
        string UserId { get; }
    }
}
