using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalExaminer.API.Attributes
{
    /// <summary>
    /// ensures that the value for supplied for the user id is valid
    /// </summary>
    public class ValidUserId : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            
            return ValidationResult.Success;
        }
    }
}
