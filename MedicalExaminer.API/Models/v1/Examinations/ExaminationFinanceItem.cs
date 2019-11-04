using MedicalExaminer.Models.Enums;
using System;

namespace MedicalExaminer.API.Models.v1.Examinations
{

    /// <summary>
    /// Examination Finance Item
    /// </summary>
    public class ExaminationFinanceItem
    {
        /// <summary>
        /// Examination Id
        /// </summary>
        public string ExaminationId { get; set; }

        /// <summary>
        /// Location: Site Name
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// Location: Trust Name
        /// </summary>
        public string TrustName { get; set; }

        /// <summary>
        /// Location: Region Name
        /// </summary>
        public string RegionName { get; set; }

        /// <summary>
        /// Location: National
        /// </summary>
        public string NationalName { get; set; }

        /// <summary>
        /// Medical Examiner Id
        /// </summary>
        public string MedicalExaminerId { get; set; }

        /// <summary>
        /// Date case Created
        /// </summary>
        public DateTime CaseCreated { get; set; }

        /// <summary>
        /// Date Case Closed
        /// </summary>
        public DateTime? CaseClosed { get; set; }

        /// <summary>
        /// Whether the case has Nhs Number
        /// </summary>
        public bool HasNhsNumber { get; set; }

        /// <summary>
        /// Has a cremation form been completed (Yes, No, Unknown)
        /// </summary>
        public CremationFormStatus? CremationFormStatus { get; set; }

        /// <summary>
        /// Indicate whether the fee can be waived 
        /// </summary>
        public bool? WaiverFee { get; set; }
    }
}
