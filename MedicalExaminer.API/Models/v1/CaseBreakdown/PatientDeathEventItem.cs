using MedicalExaminer.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class PatientDeathEventItem
    {
        /// <summary>
        /// Event Identification.
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// User Identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// IsFinal, final = true, draft = false
        /// </summary>
        public bool IsFinal { get; set; }

        /// <summary>
        /// the type of event this is
        /// </summary>
        public EventType EventType => EventType.PatientDied;

        /// <summary>
        /// date of last admission, if known
        /// </summary>
        public DateTime? DateOfDeath { get; set; }

        /// <summary>
        /// time of last admission, if known
        /// </summary>
        public TimeSpan? TimeOfDeath { get; set; }

        /// <summary>
        /// Date event was created
        /// </summary>
        public DateTime Created { get; set; }

    }
}
