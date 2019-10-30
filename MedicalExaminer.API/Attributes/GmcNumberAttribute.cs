using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MedicalExaminer.API.Attributes
{
    /// <summary>
    /// Valid GMC Number.
    /// Ensures that the value for supplied for the user's GMC number is valid.
    /// </summary>
    public sealed class GmcNumberAttribute : ValidationAttribute
    {
        // TODO: Confirm length and whether we can do the spaces? https://methods.atlassian.net/browse/MES-1158
        private readonly string patternMatch = "^[0-9 ]{0,100}$";

        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException(nameof(validationContext));
            }

            var gmcNumber = value as string;
            if (string.IsNullOrEmpty(gmcNumber))
            {
                return ValidationResult.Success;
            }

            var regex = new Regex(patternMatch);
            if (!regex.IsMatch(gmcNumber))
            {
                return new ValidationResult("GMC Number not valid.");
            }

            return ValidationResult.Success;
        }
    }
}
