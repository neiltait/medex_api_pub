using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.Common;
using Microsoft.Azure.Documents;

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
            try
            {
                var validatedLocation = await _locationPersistence.GetLocationAsync(locationString.Value);
                if (validatedLocation != null)
                {
                    return errors;
                }
            }
            catch (DocumentClientException ex)
            {
                errors.Add(new ValidationError(ValidationErrorCode.NotFound, "Location",
                    "The supplied location id was not found"));
            }
            catch (Exception ex)
            {
                errors.Add(new ValidationError(ValidationErrorCode.NotFound, "Location",
                    "The supplied location id was not found"));
            }
            return errors;
        }
    }
}
