using System;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Models
{
    public class DeathEvent : IEvent
    {
        public DateTime? DateOfDeath { get; set; }
        public TimeSpan? TimeOfDeath { get; set; }
        public EventType EventType => EventType.PatientDied;

        public string EventId { get; set; }

        public bool IsFinal => true;

        public string UserId {get; set;}

        public DateTime? Created { get; set; }
    }
}
