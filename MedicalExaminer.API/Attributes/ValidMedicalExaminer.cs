using System.ComponentModel.DataAnnotations;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Attributes
{
    /// <inheritdoc />
    /// <summary>
    ///     Validates that a user exists and is in the role of medical examiner.
    /// </summary>
    public class ValidMedicalExaminer : ValidUserBase
    {
        /// <summary>
        ///     Initialises an instance of the ValidMedicalExaminer attribute class.
        /// </summary>
        /// <param name="value">The object being validated.</param>
        /// <param name="context">The ValidationContext.</param>
        /// <returns>ValidationResult.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            return IsValid(value, context, UserRoles.MedicalExaminer, "medical examiner");
        }
    }
}