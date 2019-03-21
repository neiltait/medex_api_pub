using System.ComponentModel.DataAnnotations;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Attributes
{
    /// <summary>
    ///     Validates that a user exists and is in the role of medical examiner officer.
    /// </summary>
    public class ValidMedicalExaminerOfficer : ValidUserBase
    {
        /// <summary>
        /// ValidMedicalExaminerOfficer validator.
        /// </summary>
        /// <param name="value">Object being validated.</param>
        /// <param name="context">Validation Context.</param>
        /// <returns>ValidationResult.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            return IsValid(value, context, UserRoles.MedicalExaminerOfficer, "medical examiner officer");
        }
    }
}