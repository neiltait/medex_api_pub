using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.Validators
{
    /// <summary>
    /// Checks the input of the new examination 
    /// </summary>
    public class CheckExaminationItemValidator : IValidator<ExaminationItem>
    {
        private readonly IValidator<string> _nhsNumberValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckExaminationItemValidator"/> class.
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="nhsNumberValidator">NHS Number Validator</param>
        public CheckExaminationItemValidator(IValidator<string> nhsNumberValidator/*MetadataStoreContext context*/)
        {
            _nhsNumberValidator = nhsNumberValidator;
        }
        /// <summary>
        /// Performs the validation
        /// </summary>
        /// <param name="entity">The entity to validate</param>
        /// <returns>List of <see cref="ValidationError"/></returns>
        public async Task<IEnumerable<ValidationError>> ValidateAsync(ExaminationItem evaluationItem)
        {
            var errors = new List<ValidationError>();

            if (string.IsNullOrEmpty(evaluationItem.GivenNames) || evaluationItem.GivenNames.Length <= 1)
            {
                errors.Add(new ValidationError(ValidationErrorCode.Invalid, "GivenNames", "Invalid Given Name"));
            }

            if (string.IsNullOrEmpty(evaluationItem.Surname) || evaluationItem.Surname.Length <= 1)
            {
                errors.Add(new ValidationError(ValidationErrorCode.Invalid, "Surname", "Invalid Surname"));
            }

            if (evaluationItem.DateOfBirthKnown)
            {
                if (evaluationItem.DateOfBirth == null)
                {
                    errors.Add(new ValidationError(ValidationErrorCode.IsNull, "DateOfBirth", "If date of birth is known a date must be provided"));
                }
            }

            if (evaluationItem.DateOfDeathKnown)
            {
                if (evaluationItem.DateOfDeath == null)
                {
                    errors.Add(new ValidationError(ValidationErrorCode.IsNull, "DateOfDeath", "If date of death is known a date must be provided."));
                }
            }

            if (evaluationItem.DateOfBirthKnown && evaluationItem.DateOfDeathKnown)
            {
                if (evaluationItem.DateOfBirth != null && evaluationItem.DateOfDeath != null)
                {
                    if (evaluationItem.DateOfBirth > evaluationItem.DateOfDeath)
                    {
                        errors.Add(new ValidationError(ValidationErrorCode.Invalid, "DateOfBirth", "Date of birth must be before date of death."));
                    }
                }
            }

            if (evaluationItem.TimeOfDeathKnown)
            {
                if (evaluationItem.TimeOfDeath == null)
                {
                    errors.Add(new ValidationError(ValidationErrorCode.IsNull, "TimeOfDeath", "If time of death is known a time must be provided."));
                }
            }

            if (evaluationItem.NhsNumberKnown)
            {
                errors.AddRange(_nhsNumberValidator.ValidateAsync(evaluationItem.NhsNumber).Result);
            }

            errors.AddRange(ValidateGender(evaluationItem));

            return errors;
        }

        private static IEnumerable<ValidationError> ValidateGender(ExaminationItem evaluationItem)
        {
            var errors = new List<ValidationError>();
            if (evaluationItem.Gender == null)
            {
                errors.Add(new ValidationError(ValidationErrorCode.IsNull, "Gender", "Gender must be specified"));
                return errors;
            }

            if (evaluationItem.Gender == ExaminationGender.Other)
            {
                if (evaluationItem.GenderDetails == string.Empty)
                {
                    errors.Add(new ValidationError(ValidationErrorCode.IsNull, "GenderDetails", "If gender is other then Gender Details must be given"));
                    return errors;
                }
            }

            return errors;
        }
    }
}
