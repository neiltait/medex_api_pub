using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MedicalExaminer.API.Attributes;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    /// <summary>
    ///     Put Patient Details Request Object.
    /// </summary>
    public class PostOtherEventRequest
    {
        /// <summary>
        ///     case id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// Event Text (Length to be confirmed).
        /// </summary>
        [RequiredIfAttributesMatch(nameof(Status), Status.Final)]
        public string EventText { get; set; }

        /// <summary>
        /// Enum for the status (Draft or Final).
        /// </summary>
        public enum Status
        {
            Draft,
            Final
        }
    }
}