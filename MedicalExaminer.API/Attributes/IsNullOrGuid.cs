using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalExaminer.API.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    public class IsNullOrGuid : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var potentialGuid = value as string;
            if (potentialGuid == null)
            {
                return null;
            }

            Guid theGuid;
            if (Guid.TryParse(potentialGuid, out theGuid))
            {
                return null;
            }

            return new ValidationResult("Value should be null or a GUID.");
        }
    }
}
