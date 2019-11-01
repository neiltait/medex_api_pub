using System;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Models
{
    public interface IEvent
    {
        EventType EventType { get; }

        string EventId { get; set; }

        bool IsFinal { get; }

        string UserId { get; set; }

        string UserFullName { get; set; }

        string GmcNumber { get; set; }

        string UsersRole { get; set; }

        DateTime? Created { get; set; }
    }
}
