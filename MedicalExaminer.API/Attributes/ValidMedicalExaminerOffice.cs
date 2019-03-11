using System.ComponentModel.DataAnnotations;
using MedicalExaminer.Common;

namespace MedicalExaminer.API.Attributes
{
    /// <summary>
    /// Validates that a given nhs number has the correct format and passes the check sum
    /// </summary>
    public class ValidMedicalExaminerOffice : RequiredAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            // TODO
            //return ValidationResult.Success;
            var locationPersistence = (ILocationPersistence) context.GetService(typeof(ILocationPersistence));
            var locationString = value as string;
            if (string.IsNullOrEmpty(locationString))
            {
                return new ValidationResult("The location Id must be supplied");
            }

            var validatedLocation = locationPersistence.GetLocationAsync(locationString).Result;

            return validatedLocation == null ? new ValidationResult("The location Id has not been found") : ValidationResult.Success;
        }
    }
}
