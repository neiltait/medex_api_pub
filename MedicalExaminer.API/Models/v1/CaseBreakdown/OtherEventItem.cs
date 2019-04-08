﻿using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class OtherEventItem
    {
        /// <summary>
        /// Event Identication.
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// User Identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Event Text (Length to be confirmed).
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// IsFinal, final = true, draft = false
        /// </summary>
        public bool IsFinal { get; set; }

        /// <summary>
        /// the type of event this is
        /// </summary>
        public EventType EventType => EventType.Other;
    }
}
