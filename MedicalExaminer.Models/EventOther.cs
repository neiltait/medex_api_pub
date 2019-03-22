using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace MedicalExaminer.Models
{
    public class EventOther
    {
        /// <summary>
        /// Event ID.
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// Event Text (Length to be confirmed).
        /// </summary>
        public string EventText { get; set; }

        /// <summary>
        /// Enum for the status (Draft or Final).
        /// </summary>
        public enum Status
        {
            Draft,
            Final
        }

        // public MeUser User { get; set; }
        // public DateTime DateTimeEntered { get; set; }
    }
}