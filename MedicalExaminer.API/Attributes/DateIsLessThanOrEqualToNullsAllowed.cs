using MedicalExaminer.API.Models.v1;
using MedicalExaminer.Models.Enums;
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
        private readonly DateTime FrontEndNullDateValue = new DateTime(1, 1, 1);
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
                return new ValidationResultEnumCodes(SystemValidationErrors.InvalidFormat);
            }

            try
            {
                var temp = context.ObjectInstance.GetType().GetProperty(endDateField).GetValue(context.ObjectInstance);
                if (temp == null)
                {
                    return ValidationResult.Success;
                }

                endDate = Convert.ToDateTime(temp);

                if (endDate == FrontEndNullDateValue)
                {
                    return ValidationResult.Success;
                }

            }
            catch (NullReferenceException)
            {
                return new ValidationResultEnumCodes(SystemValidationErrors.EndDateNotFound);
            }
            catch (Exception ex)
            {
                var e = ex;
                return new ValidationResultEnumCodes(SystemValidationErrors.InvalidFormat);
            }

            return endDate < startDate
                ? new ValidationResultEnumCodes(SystemValidationErrors.EndDateBeforeStartDate)
                : ValidationResult.Success;
        }
    }
}