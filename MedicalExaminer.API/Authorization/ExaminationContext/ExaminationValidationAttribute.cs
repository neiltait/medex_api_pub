using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalExaminer.API.Authorization.ExaminationContext
{
    /// <summary>
    /// Examination Validation Attribute.
    /// </summary>
    /// <remarks>Validation that requires an examination context to validate.</remarks>
    public abstract class ExaminationValidationAttribute : RequiredAttribute
    {
        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var examinationValidationContextProvider =
                (ExaminationValidationContextProvider)validationContext.GetService(typeof(ExaminationValidationContextProvider));

            if (examinationValidationContextProvider == null)
            {
                throw new InvalidOperationException("The Examination Validation context provider has not been registered.");
            }

            return IsValid(value, examinationValidationContextProvider.Current, validationContext);
        }

        /// <summary>
        /// Is Valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="examinationValidationContext">Examination Validation Context.</param>
        /// <param name="validationContext">Validation Context.</param>
        /// <returns><see cref="ValidationResult"/></returns>
        protected abstract ValidationResult IsValid(
            object value,
            ExaminationValidationContext examinationValidationContext,
            ValidationContext validationContext);
    }
}
