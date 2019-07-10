using System;
using System.ComponentModel.DataAnnotations;
using MedicalExaminer.API.Models.v1;
using MedicalExaminer.Common;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;

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
            var locationString = value as string;
            if (string.IsNullOrEmpty(locationString))
            {
                return new ValidationResultEnumCodes(MedicalExaminer.Models.Enums.SystemValidationErrors.LocationIdMustBeProvided);
            }

            var locationPersistence = (IAsyncQueryHandler<LocationRetrievalByIdQuery, Location>)context.GetService(typeof(IAsyncQueryHandler<LocationRetrievalByIdQuery, Location>));
            if (locationPersistence == null)
            {
                throw new NullReferenceException("locationPersistence is null");
            }

            var validatedLocation = locationPersistence.Handle(new LocationRetrievalByIdQuery(locationString)).Result;

            return validatedLocation == null
                ? new ValidationResultEnumCodes(MedicalExaminer.Models.Enums.SystemValidationErrors.LocationIdNotFound)
                : ValidationResult.Success;
        }
    }
}