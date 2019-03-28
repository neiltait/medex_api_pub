using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.Common;
using MedicalExaminer.Common.Enums;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace MedicalExaminer.API.Attributes
{
    /// <summary>
    ///     Base class for validators of UserItem objects.
    /// </summary>
    public abstract class ValidUserBase : RequiredAttribute
    {
        /// <summary>
        /// Instantiate a new instance of <see cref="ValidUserBase"/>.
        /// </summary>
        /// <param name="userRole">User Role.</param>
        protected ValidUserBase(UserRoles userRole)
        {
            UserRole = userRole;
        }

        /// <summary>
        /// Role required.
        /// </summary>
        public UserRoles UserRole { get; }

        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var userPersistence = (IUserPersistence)context.GetService(typeof(IUserPersistence));

            if (!(value is string userId))
            {
                return new ValidationResult($"Item not recognised as of type useritem for {UserRole.GetDescription()}");
            }

            try
            {
                if (userPersistence == null)
                {
                    throw new NullReferenceException("User Persistence is null");
                }

                var returnedDocument = userPersistence.GetUserAsync(userId).Result;
                if (returnedDocument.UserRole != UserRole)
                {
                    return new ValidationResult($"The user is not a {UserRole.GetDescription()}");
                }
            }
            catch (ArgumentException)
            {
                return new ValidationResult($"The {UserRole.GetDescription()} has not been found");
            }

            return ValidationResult.Success;
        }
    }
}