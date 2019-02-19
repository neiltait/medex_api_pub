using System;
using System.ComponentModel.DataAnnotations;

namespace Medical_Examiner_API.Models
{
    public abstract class Record : Microsoft.Azure.Documents.Document
    {
        [Required]
        [Display(Name = "user_id")]
        [DataType(DataType.Text)]
        public string LastModifiedBy { get; set; }

        [Required]
        [Display(Name = "modified_at")]
        [DataType(DataType.DateTime)]
        public DateTimeOffset ModifiedAt { get; set; }

        [Required]
        [Display(Name = "created_at")]
        [DataType(DataType.DateTime)]
        public DateTimeOffset CreatedAt { get; set; }

        [Display(Name = "deleted_at")]
        [DataType(DataType.DateTime)]
        public Nullable<DateTimeOffset> DeletedAt { get; set; }
    }
}
