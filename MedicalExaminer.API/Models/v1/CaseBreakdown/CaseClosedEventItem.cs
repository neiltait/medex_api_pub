using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using System;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class CaseClosedEventItem : IEvent
    {
        /// <summary>
        /// Date the patient died
        /// </summary>
        public DateTime? DateCaseClosed { get; set; }

        /// <summary>
        /// event type
        /// </summary>
        public EventType EventType => EventType.CaseClosed;

        /// <summary>
        /// event id
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// is the event a draft or a final version
        /// </summary>
        public bool IsFinal => true;

        /// <summary>
        /// user id that created the event
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// the date the event was created.
        /// </summary>
        public DateTime? Created { get; set; }

        /// <summary>
        /// Users full name
        /// </summary>
        public string UserFullName { get; set; }

        /// <summary>
        /// Users Role
        /// </summary>
        public string UsersRole { get; set; }
    }
}
