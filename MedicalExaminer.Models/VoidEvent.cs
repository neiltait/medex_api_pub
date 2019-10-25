using System;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Models
{
    public class VoidEvent : IEvent
    {
        /// <summary>
        /// the reason for voiding the examination.
        /// </summary>
        public string VoidReason { get; set; }

        /// <summary>
        /// the event type
        /// </summary>
        public EventType EventType => EventType.CaseVoid;

        /// <summary>
        /// the id of the event
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// Is the event final, not a draft
        /// </summary>
        public bool IsFinal => true;

        /// <summary>
        /// the users id that added the event
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// the users full name that added the event
        /// </summary>
        public string UserFullName { get; set; }

        /// <summary>
        /// The users role that added the event
        /// </summary>
        public string UsersRole { get; set; }

        /// <summary>
        /// Date the event was added
        /// </summary>
        public DateTime? Created { get; set; }
    }
}
