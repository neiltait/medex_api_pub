using System;
using System.ComponentModel.DataAnnotations;
using MedicalExaminer.API.Extensions.Models;
using MedicalExaminer.Common.Enums;
using MedicalExaminer.Common.Extensions.MeUser;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Authorization.ExaminationContext
{
    /// <summary>
    /// Is Role On Examination
    /// </summary>
    /// <remarks>Does the user have the required role on the examination in the validation context.</remarks>
    public class ValidRoleOnExamination : ExaminationValidationAttribute
    {
        private readonly UserRoles _requiredRole;

        /// <summary>
        /// Initialise a new instance of <see cref="ValidRoleOnExamination"/>.
        /// </summary>
        /// <param name="requiredRole">Role Required.</param>
        public ValidRoleOnExamination(UserRoles requiredRole)
        {
            _requiredRole = requiredRole;
        }

        /// <inheritdoc/>
        protected override ValidationResult IsValid(
            object value,
            ExaminationValidationContext examinationValidationContext,
            ValidationContext validationContext)
        {
            if (examinationValidationContext.Examination == null)
            {
                return new ValidationResult($"The examination was not present to confirm role validation.");
            }

            var userRetrievalByIdService =
                (IAsyncQueryHandler<UserRetrievalByIdQuery, MedicalExaminer.Models.MeUser>)
                validationContext.GetService(
                    typeof(IAsyncQueryHandler<UserRetrievalByIdQuery, MedicalExaminer.Models.MeUser>));

            if (userRetrievalByIdService == null)
            {
                throw new InvalidOperationException("The user retrieval by id service has not been registered.");
            }

            var userId = value as string;

            if (userId == null)
            {
                return new ValidationResult($"Item not recognised as of type useritem for `{_requiredRole.GetDescription()}`.");
            }

            var user = userRetrievalByIdService.Handle(new UserRetrievalByIdQuery(userId)).Result;

            if (user == null)
            {
                return new ValidationResult($"The user with id `{userId}` was not found.");
            }

            var role = user.RoleForExamination(examinationValidationContext.Examination);

            if (role != _requiredRole)
            {
                return new ValidationResult($"The user is not a `{_requiredRole.GetDescription()}`.");
            }

            return ValidationResult.Success;
        }
    }
}
