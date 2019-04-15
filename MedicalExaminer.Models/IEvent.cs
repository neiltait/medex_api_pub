using MedicalExaminer.Models.Enums;
using System;

namespace MedicalExaminer.Models
{
    public interface IEvent
    {
        EventType EventType { get; }
        string EventId { get; set; }
        bool IsFinal { get; }
        string UserId { get; }
        DateTime? Created { get; set; }
    }
}
