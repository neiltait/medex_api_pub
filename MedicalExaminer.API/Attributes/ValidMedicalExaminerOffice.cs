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
            var locationPersistence = (ILocationPersistence) context.GetService(typeof(ILocationPersistence));
            var locationString = value as string;
            if (locationString == null)
            {
                return new ValidationResult("The location id must be supplied");
            }

            if (locationString == string.Empty)
            {
                return new ValidationResult("The location id must be supplied");
            }

            var validatedLocation = locationPersistence.GetLocationAsync(locationString).Result;
            return validatedLocation == null ? new ValidationResult("The location id has not been found") : ValidationResult.Success;
        }
    }
}
