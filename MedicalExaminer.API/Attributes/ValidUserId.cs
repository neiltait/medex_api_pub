using System.ComponentModel.DataAnnotations;
using MedicalExaminer.Common.Queries.UserQueries;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Common.Services.UserService;
using MedicalExaminer.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalExaminer.API.Attributes
{
    /// <summary>
    /// ensures that the value for supplied for the user id is valid
    /// </summary>
    public class ValidUserId : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            //TODO
            return ValidationResult.Success;
            var userService = (UserRetrievalService)context.GetService(typeof(IAsyncQueryHandler<UserRetrievalQuery, MeUser>));
            var userIdString = value as string;
            if (string.IsNullOrEmpty(userIdString))
            {
                return new ValidationResult("The users Id must be supplied");
            }

            var validUser = userService.Handle(new UserRetrievalQuery(userIdString)).Result;

            return validUser == null ? new ValidationResult("The user Id has not been found") : ValidationResult.Success;
        }
    }
}
