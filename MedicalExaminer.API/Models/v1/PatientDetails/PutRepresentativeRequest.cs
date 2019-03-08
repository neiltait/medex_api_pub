using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.PatientDetails
{
    /// <summary>
    /// Object for creating and updating patients representative
    /// </summary>
    public class PutRepresentativeRequest
    {
        /// <summary>
        /// Internal id for the patients representative
        /// </summary>
        public Guid RepresentativeId { get; set; }
        /// <summary>
        /// Representatives full name
        /// </summary>
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
    }
}
