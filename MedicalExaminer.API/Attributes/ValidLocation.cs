using System;
using System.ComponentModel.DataAnnotations;
using MedicalExaminer.Common;

namespace MedicalExaminer.API.Attributes
{
    /// <summary>
    /// Valid Location.
    /// </summary>
    /// <remarks>Checks that the location is a valid location in the system.</remarks>
    public class ValidLocation : RequiredAttribute
    {
        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var locationPersistence = (ILocationPersistence)context.GetService(typeof(ILocationPersistence));
            var locationString = value as string;
            if (string.IsNullOrEmpty(locationString))
            {
                return new ValidationResult("The location Id must be supplied");
            }

            // Edge case!
            if (locationPersistence == null)
            {
                throw new NullReferenceException("locationPersistence is null");
            }

            var validatedLocation = locationPersistence.GetLocationAsync(locationString).Result;

            return validatedLocation == null
                ? new ValidationResult("The location Id has not been found")
                : ValidationResult.Success;
        }
    }
}