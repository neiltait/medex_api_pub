using System;
using System.ComponentModel.DataAnnotations;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Models
{
    public class Representative
    {
        public string FullName { get; set; }

        /// <summary>
        /// Representatives relationship to the patient
        /// </summary>
        public string Relationship { get; set; }

        /// <summary>
        /// Representatives phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Was the representative present at the death?
        /// </summary>
        public PresentAtDeath PresentAtDeath { get; set; }

        /// <summary>
        /// Has the representative been informed?
        /// </summary>
        public Informed Informed { get; set; }
        public DateTime? AppointmentDate { get; set; }
    }
}