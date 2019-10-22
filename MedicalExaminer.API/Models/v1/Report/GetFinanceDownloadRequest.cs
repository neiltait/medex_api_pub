using System;
using System.ComponentModel.DataAnnotations;
using MedicalExaminer.API.Attributes;

namespace MedicalExaminer.API.Models.v1.Report
{
    /// <summary>
    /// DTO object for the finance report.
    /// </summary>
    public class GetFinanceDownloadRequest
    {
        /// <summary>
        /// The start date to run the report from based off the Examination Created Date
        /// </summary>
        [Required]
        public DateTime ExaminationsCreatedFrom { get; set; }

        /// <summary>
        /// The end date to run the report to based off the Examination Created Date
        /// </summary>
        [Required]
        public DateTime ExaminationsCreatedTo { get; set; }

        /// <summary>
        /// The location ID to run the report for.
        /// </summary>
        [ValidMedicalExaminerOfficeNullAllowed]
        [Required]
        public string LocationId { get; set; }
    }
}
