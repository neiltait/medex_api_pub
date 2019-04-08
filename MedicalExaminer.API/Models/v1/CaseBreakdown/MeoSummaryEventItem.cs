using MedicalExaminer.Models.Enums;
using System;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class MeoSummaryEventItem
    {
        /// <summary>
        /// Date event was created
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Event Identification.
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// User Identifier.
        /// </summary>
        public string UserId { get; set; }


        /// <summary>
        /// draft = false, final = true
        /// </summary>
        public bool IsFinal { get; set; }

        /// <summary>
        /// the type of event this is
        /// </summary>
        public EventType EventType => EventType.MeoSummary;

        /// <summary>
        /// Details of the MEO Summary.
        /// </summary>
        public string SummaryDetails { get; set; }
    }
}
