using System.ComponentModel.DataAnnotations;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Common.Services.User;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Attributes
{
    /// <summary>
    /// Valid User Id.
    /// Ensures that the value for supplied for the user id is valid
    /// </summary>
    public class ValidUserId : ValidationAttribute
    {
        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var userService = (UserRetrievalByIdService)context.GetService(typeof(IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>));
            var userIdString = value as string;
            if (string.IsNullOrEmpty(userIdString))
            {
                return ValidationResult.Success;
            }

            var validUser = userService.Handle(new UserRetrievalByIdQuery(userIdString)).Result;

            return validUser == null ? new ValidationResult("The user Id has not been found") : ValidationResult.Success;
        }
    }
}
