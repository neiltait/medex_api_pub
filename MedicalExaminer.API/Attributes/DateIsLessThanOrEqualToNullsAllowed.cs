using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalExaminer.API.Attributes
{
    /// <summary>
    ///     Validates a date is less or equal to another date or is null.
    /// </summary>
    public class DateIsLessThanOrEqualToNullsAllowed : ValidationAttribute
    {
        private readonly string endDateField;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DateIsLessThanOrEqualToNullsAllowed" /> class.
        /// </summary>
        /// <param name="endDateField">The date to compare to.</param>
        public DateIsLessThanOrEqualToNullsAllowed(string endDateField)
        {
            this.endDateField = endDateField;
        }

        /// <inheritdoc />
        /// <summary>
        /// Execute the validation on an object.
        /// </summary>
        /// <param name="value">The object to validate.</param>
        /// <param name="context">The Validation Context.</param>
        /// <returns>ValidationResult</returns>
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            DateTime startDate;
            DateTime endDate;
            if (value == null)
            {
                return ValidationResult.Success;
            }

            try
            {
                startDate = Convert.ToDateTime(value);
            }
            catch (Exception)
            {
                return new ValidationResult($"Incorrect Format for {context.DisplayName}");
            }

            try
            {
                var temp = context.ObjectInstance.GetType().GetProperty(endDateField).GetValue(context.ObjectInstance);
                if (temp == null)
                {
                    return ValidationResult.Success;
                }

                endDate = Convert.ToDateTime(temp);
            }
            catch (NullReferenceException)
            {
                return new ValidationResult($"Unable to find the end date field {endDateField} on the object");
            }
            catch (Exception ex)
            {
                var e = ex;
                return new ValidationResult($"Incorrect Format for {context.DisplayName}");
            }

            return endDate < startDate
                ? new ValidationResult("The patient cannot have died before they were born")
                : ValidationResult.Success;
        }
    }
}