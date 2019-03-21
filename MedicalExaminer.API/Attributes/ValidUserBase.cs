using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.Common;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace MedicalExaminer.API.Attributes
{
    /// <summary>
    ///     Base class for validators of UserItem objects
    /// </summary>
    public abstract class ValidUserBase : RequiredAttribute
    {
        /// <summary>
        /// Validate User Base class. 
        /// </summary>
        /// <param name="value">object to be validated</param>
        /// <param name="context">Validation Context</param>
        /// <returns>VaidationResult</returns>
        /// <exception cref="NotImplementedException">Its not implemented</exception>
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// runs the validation
        /// </summary>
        /// <param name="value">Object to validate</param>
        /// <param name="context">Validation Context</param>
        /// <param name="userRole">User Role</param>
        /// <param name="userTypeName">User Type Name</param>
        /// <returns>ValidationResult</returns>
        /// <exception cref="NullReferenceException">Null reference error</exception>
        protected ValidationResult IsValid(
            object value,
            ValidationContext context,
            UserRoles userRole,
            string userTypeName)
        {
            var userPersistence = (IUserPersistence)context.GetService(typeof(IUserPersistence));
            var mapper = (IMapper)context.GetService(typeof(IMapper));

            if (!(value is UserItem userItem))
            {
                return new ValidationResult($"Item not recognised as of type useritem for {userTypeName}");
            }

            var meUser = mapper.Map<MeUser>(userItem);

            if (meUser == null || string.IsNullOrEmpty(meUser.UserId))
            {
                return new ValidationResult($"Cannot get id for {userTypeName}");
            }

            try
            {
                if (userPersistence == null)
                {
                    throw new NullReferenceException("User Persistence is null");
                }

                var returnedDocument = userPersistence.GetUserAsync(meUser.UserId).Result;
                if (returnedDocument.UserRole != userRole)
                {
                    return new ValidationResult($"The user is not a {userTypeName}");
                }
            }
            catch (ArgumentException)
            {
                return new ValidationResult($"The {userTypeName} has not been found");
            }

            return ValidationResult.Success;
        }
    }
}