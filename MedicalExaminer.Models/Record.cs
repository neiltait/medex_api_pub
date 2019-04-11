using System;
using System.ComponentModel.DataAnnotations;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace MedicalExaminer.Models
{
    /// <summary>
    /// Record.
    /// </summary>
    public abstract class Record
    {
        /// <summary>
        /// Last Modified By.
        /// </summary>
        [Required]
        [Display(Name = "modified_by")]
        [DataType(DataType.Text)]
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Modified At.
        /// </summary>
        [Required]
        [Display(Name = "modified_at")]
        [DataType(DataType.DateTime)]
        public DateTimeOffset ModifiedAt { get; set; }

        /// <summary>
        /// Create At.
        /// </summary>
        [Required]
        [Display(Name = "created_at")]
        [DataType(DataType.DateTime)]
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Deleted At.
        /// </summary>
        [Display(Name = "deleted_at")]
        [DataType(DataType.DateTime)]
        public DateTimeOffset? DeletedAt { get; set; }

        /// <summary>
        /// Last Modified By.
        /// </summary>
        [Required]
        [Display(Name = "created_by")]
        [DataType(DataType.Text)]
        public string CreatedBy { get; set; }
    }
}
