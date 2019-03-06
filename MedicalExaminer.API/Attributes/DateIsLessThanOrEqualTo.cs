using System;
using System.ComponentModel.DataAnnotations;
namespace MedicalExaminer.API.Attributes
{
    public class DateIsLessThanOrEqualToNullsAllowed : ValidationAttribute
    {
        private readonly string _endDateField;
        public DateIsLessThanOrEqualToNullsAllowed(string endDateField)
        {
            _endDateField = endDateField;
        }
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
                var temp = context.ObjectInstance.GetType().GetProperty(_endDateField).GetValue(context.ObjectInstance);
                if (temp == null)
                {
                    return ValidationResult.Success;
                }

                endDate = Convert.ToDateTime(temp);
            }
            catch(NullReferenceException nre)
            {
                return new ValidationResult($"Unable to find the end date field {_endDateField} on the object");
            }
            catch (Exception ex)
            {
                var e = ex;
                return new ValidationResult($"Incorrect Format for {context.DisplayName}");
            }

            if (endDate < startDate)
            {
                return new ValidationResult($"The patient cannot have died before they were born");
            }

            return ValidationResult.Success;
        }

        
    }
}
