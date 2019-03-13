using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.Common;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace MedicalExaminer.API.Attributes
{
    /// <summary>
    /// Base class for validators of UserItem objects
    /// </summary>
    public abstract class ValidateUserBase : RequiredAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            throw new NotImplementedException();
        }

        protected ValidationResult IsValid(object value, ValidationContext context, UserRoles userRole, string userTypeName)
        {
            var userPersistence = (IUserPersistence)context.GetService(typeof(IUserPersistence));
            var mapper = (IMapper)context.GetService(typeof(IMapper));
            var userItem = value as UserItem;

            if (userItem == null)
            {
                return new ValidationResult($"Cannot get id for {userTypeName}");
            }

            var meUser = mapper.Map<MeUser>(userItem);


            if (meUser == null || string.IsNullOrEmpty(meUser.UserId))
            {
                return new ValidationResult($"Cannot get id for {userTypeName}");
            }

            MeUser returnedDocument;
            try
            {
                returnedDocument = userPersistence.GetUserAsync(meUser.UserId).Result;
            }
            catch (Exception e)
            {
                return new ValidationResult($"The {userTypeName} has not been found");
            }

            if (returnedDocument.UserRole != userRole)
            {
                return new ValidationResult($"The user is not a {userTypeName}");
            }

            return ValidationResult.Success;
        }
    }
}
