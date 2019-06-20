using System;
using System.ComponentModel.DataAnnotations;
using MedicalExaminer.Common;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Attributes
{
    /// <summary>
    /// Validate Medical Examiner Office Null Allowed
    /// </summary>
    public class ValidMedicalExaminerOfficeNullAllowed : ValidationAttribute
    {
        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var locationString = value as string;
            if (string.IsNullOrEmpty(locationString))
            {
                return ValidationResult.Success;
            }

            var locationPersistence = (IAsyncQueryHandler<LocationRetrievalByIdQuery, Location>)context.GetService(typeof(IAsyncQueryHandler<LocationRetrievalByIdQuery, Location>));
            if (locationPersistence == null)
            {
                throw new NullReferenceException("locationPersistence is null");
            }

            var validatedLocation = locationPersistence.Handle(new LocationRetrievalByIdQuery(locationString)).Result;

            return validatedLocation == null
                ? new ValidationResult("The location Id has not been found")
                : ValidationResult.Success;
        }
    }
}
