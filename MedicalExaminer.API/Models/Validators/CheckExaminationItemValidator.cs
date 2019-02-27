using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedicalExaminer.API.Models.v1.Examinations;
using Microsoft.Azure.Documents.SystemFunctions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.API.Models.Validators
{
    public class CheckExaminationItemValidator : IValidator<ExaminationItem>
    {
        private NhsNumberValidator _nhsNumberValidator;
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckExistingSpecificationVersionValidator"/> class.
        /// </summary>
        /// <param name="context">Database context</param>
        public CheckExaminationItemValidator(NhsNumberValidator nhsNumberValidator/*MetadataStoreContext context*/)
        {
            _nhsNumberValidator = nhsNumberValidator;
        }
        /// <summary>
        /// Performs the validation
        /// </summary>
        /// <param name="entity">The entity to validate</param>
        /// <returns>List of <see cref="ValidationError"/></returns>
        public async Task<IList<ValidationError>> ValidateAsync(ExaminationItem evaluationItem)
        {
            var errors = new List<ValidationError>();

            if (string.IsNullOrEmpty(evaluationItem.Surname) || evaluationItem.GivenName.Length <= 1)
            {
                errors.Add(new ValidationError(ValidationErrorCode.Invalid, "Invalid Given Name"));
            }

            if (string.IsNullOrEmpty(evaluationItem.Surname) || evaluationItem.Surname.Length <= 1)
            {
                errors.Add(new ValidationError(ValidationErrorCode.Invalid, "Invalid Surname"));
            }

            if (evaluationItem.DateOfBirthKnown)
            {
                if (evaluationItem.DateOfBirth == null)
                {
                    errors.Add(new ValidationError(ValidationErrorCode.IsNull, "If date of birth is known a date must be provided"));
                }
            }

            if (evaluationItem.DateOfDeathKnown)
            {
                if (evaluationItem.DateOfDeath == null)
                {
                    errors.Add(new ValidationError(ValidationErrorCode.IsNull, "If date of death is known a date must be provided."));
                }
            }

            if (evaluationItem.DateOfBirthKnown && evaluationItem.DateOfDeathKnown)
            {
                if (evaluationItem.DateOfBirth != null && evaluationItem.DateOfDeath != null)
                {
                    if (evaluationItem.DateOfBirth > evaluationItem.DateOfDeath)
                    {
                        errors.Add(new ValidationError(ValidationErrorCode.Invalid, "Date of birth must be before date of death."));
                    }
                }
            }

            if (evaluationItem.TimeOfDeathKnown)
            {
                if (evaluationItem.TimeOfDeath == null)
                {
                    errors.Add(new ValidationError(ValidationErrorCode.IsNull, "If time of death is known a time must be provided."));
                }
            }

            if (evaluationItem.NhsNumberKnown)
            {
                errors.AddRange(_nhsNumberValidator.ValidateAsync(evaluationItem.NhsNumber).Result);
            }

            if (evaluationItem.Gender == null)
            {
                errors.Add(new ValidationError(ValidationErrorCode.IsNull, "Gender must be specified"));
            }

            return errors;
        }
    }
}
