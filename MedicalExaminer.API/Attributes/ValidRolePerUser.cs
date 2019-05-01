using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MedicalExaminer.API.Models.v1.Permissions;
using MedicalExaminer.Common;
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

            var permissionPersistence = (IPermissionPersistence)context.GetService(typeof(IPermissionPersistence));

            if (permissionPersistence == null)
            {
                throw new NullReferenceException("permissionPersistence is null");
            }

            var instance = (PutPermissionRequest)context.ObjectInstance;

            var type = instance.GetType();
            var propertyValue = (string)type.GetProperty(UserIdField).GetValue(instance, null);

            var existingPermissions = permissionPersistence.GetPermissionsAsync(propertyValue).Result;

            if (IsTargetRole(role))
            {
                if (existingPermissions.Any(p => IsTargetRole((UserRoles)p.UserRole)))
                {
                    return new ValidationResult("User already has permission");
                }
            }

            return ValidationResult.Success;
        }

        private bool IsTargetRole(UserRoles role)
        {
            return role == UserRoles.MedicalExaminer || role == UserRoles.MedicalExaminerOfficer;
        }
    }
}