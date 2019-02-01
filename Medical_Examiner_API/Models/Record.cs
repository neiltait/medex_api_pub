using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Medical_Examiners_API.Models
{
    public abstract class Record
    {
        [Display(Name = "User UID")]
        [DataType(DataType.Text)]
        public string LastModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
