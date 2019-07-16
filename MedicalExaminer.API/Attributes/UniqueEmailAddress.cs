using System;
using System.ComponentModel.DataAnnotations;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Attributes
{
    /// <summary>
    /// Valid unique Email Address.
    /// </summary>
    /// <remarks>Checks that the location is a valid location in the system.</remarks>
    public class UniqueEmailAddress : RequiredAttribute
    {
        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var emailString = value as string;
            if (string.IsNullOrEmpty(emailString))
            {
                return new ValidationResult("The users email must be supplied");
            }

            var userPersistence = (IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>)context.GetService(typeof(IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>));
            if (userPersistence == null)
            {
                throw new NullReferenceException("userPersistence is null");
            }

            var possibleEmail = userPersistence.Handle(new UserRetrievalByEmailQuery(emailString)).Result;

            return possibleEmail != null
                ? new ValidationResult("The email already exists, try another")
                : ValidationResult.Success;
        }
    }
}
