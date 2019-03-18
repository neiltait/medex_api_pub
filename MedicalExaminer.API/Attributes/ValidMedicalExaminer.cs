using System.ComponentModel.DataAnnotations;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Attributes
{
    /// <summary>
    ///     Validates that a user exists and is in the role of medical examiner
    /// </summary>
    public class ValidMedicalExaminer : ValidUserBase
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            return IsValid(value, context, UserRoles.MedicalExaminer, "medical examiner");
        }
    }
}