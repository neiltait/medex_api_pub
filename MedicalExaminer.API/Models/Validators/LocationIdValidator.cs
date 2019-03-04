using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalExaminer.Common;

namespace MedicalExaminer.API.Models.Validators
{
    public class LocationIdValidator : IValidator<LocationIdString>
    {
        private readonly ILocationPersistence _locationPersistence;

        public LocationIdValidator(ILocationPersistence locationPersistence)
        {
            _locationPersistence = locationPersistence;
        }

        public async Task<IEnumerable<ValidationError>> ValidateAsync(LocationIdString locationString)
        {
            var errors = new List<ValidationError>();

            if (locationString.Value == string.Empty)
            {
                errors.Add(new ValidationError(ValidationErrorCode.IsEmpty, "Location",
                    "The location id must be supplied"));
                return errors;
            }

            var validatedLocation = await _locationPersistence.GetLocationAsync(locationString.Value);
            if (validatedLocation != null)
                {
                    return errors;
                }

            errors.Add(new ValidationError(ValidationErrorCode.NotFound, "Location",
                    "The supplied location id was not found"));

            return errors;
        }
    }
}
