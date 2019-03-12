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
    /// Validates that a user exists and is in the role of medical examiner
    /// </summary>
    public class ValidateMedicalExaminer : RequiredAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var userPersistence = (IUserPersistence) context.GetService(typeof(IUserPersistence));
            var mapper = (IMapper) context.GetService(typeof(IMapper));
            var userItem = value as UserItem;

            if (userItem == null)
            {
                return new ValidationResult("Cannot get id for medical examiner");
            }

            var meUser = mapper.Map<MeUser>(userItem);


            if (meUser == null || string.IsNullOrEmpty(meUser.UserId))
            {
                return new ValidationResult("Cannot get id for medical examiner");
            }

            var found =  userPersistence.GetUserAsync(meUser.UserId).Result;

            if (found == null)
            {
                return new ValidationResult("The medical examiner has not been found");
            }

            if (found.UserRole != UserRoles.MedicalExaminer)
            {
                return new ValidationResult("The user is not a medical examiner");
            }

            return ValidationResult.Success;
        }
    }
}
