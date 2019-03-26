using System.ComponentModel.DataAnnotations;
using MedicalExaminer.Common;

namespace MedicalExaminer.API.Attributes
{
    public class ValidMedicalExaminerOfficeNullAllowed : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {

            var locationPersistence = (ILocationPersistence)context.GetService(typeof(ILocationPersistence));
            var locationString = value as string;
            if (string.IsNullOrEmpty(locationString))
            {
                return ValidationResult.Success;
            }

            var validatedLocation = locationPersistence.GetLocationAsync(locationString).Result;

            return validatedLocation == null ? new ValidationResult("The location Id has not been found") : ValidationResult.Success;
        }

    }
}
