﻿using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Models
{
    public interface IEvent
    {
        EventType EventType { get; }
        string EventId { get; set; }
        bool IsFinal { get; }
        string UserId { get; }
    }
}