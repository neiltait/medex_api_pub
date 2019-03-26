using System;
using System.Collections.Generic;
using System.Text;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Models
{
    public interface IEvent
    {
        string EventId { get; }
        EventStatus EventStatus { get; }
        string UserId { get; }
        IEnumerable<IEvent> Amendments { get; set; }
    }
}
