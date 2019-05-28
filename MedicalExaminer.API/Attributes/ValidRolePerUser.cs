using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MedicalExaminer.API.Models.v1;
using MedicalExaminer.API.Models.v1.Permissions;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Attributes
{
    /// <summary>
    /// Validates that the role is valid for the user.
    /// </summary>
    /// <remarks>Can't be an ME and MEO in the system.</remarks>
    public class ValidRolePerUser : RequiredAttribute
    {
        public string UserIdField { get; private set; }

        public ValidRolePerUser(string userIdField)
        {
            UserIdField = userIdField;
        }

        /// <summary>
        ///     Runs the validation.
        /// </summary>
        /// <param name="value">The object being validated.</param>
        /// <param name="context">The ValidationContext.</param>
        /// <returns>ValidationResult.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var role = (UserRoles)Enum.Parse(typeof(UserRoles), value.ToString());

            var uerRetrievalByIdService = (IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>)context.GetService(typeof(IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>));

            if (uerRetrievalByIdService == null)
            {
                throw new NullReferenceException("uerRetrievalByIdService is null");
            }

            var instance = (IUserRequest)context.ObjectInstance;

            var type = instance.GetType();
            var userIdValue = (string)type.GetProperty(UserIdField).GetValue(instance, null);

            var existingPermissions = uerRetrievalByIdService.Handle(new UserRetrievalByIdQuery(userIdValue)).Result;
            if (existingPermissions != null)
            {
                if (IsTargetRole(role))
                {
                    if (existingPermissions.Permissions == null)
                    {
                        existingPermissions.Permissions = new List<MEUserPermission>();
                    }

                    if (existingPermissions.Permissions.Any(p => IsTargetRole((UserRoles) p.UserRole)))
                    {
                        var permission =
                            existingPermissions.Permissions.First(p => IsTargetRole((UserRoles) p.UserRole));

                        return new ValidationResult(
                            $"Cannot add Role `{role}`, user is already `{(UserRoles) permission.UserRole}` elsewhere in the system.");
                    }
                }
            }

            return base.IsValid(value, context);
        }

        private bool IsTargetRole(UserRoles role)
        {
            return role == UserRoles.MedicalExaminer || role == UserRoles.MedicalExaminerOfficer;
        }
    }
}