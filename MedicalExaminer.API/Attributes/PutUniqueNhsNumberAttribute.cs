using System;
using System.ComponentModel.DataAnnotations;
using MedicalExaminer.API.Authorization.ExaminationContext;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Attributes
{
    /// <summary>
    /// This validates that the nhs number is unique when a new examination is created.
    /// </summary>
    public class PutUniqueNhsNumberAttribute : ExaminationValidationAttribute
    {
        /// <summary>
        /// returns if the nhs number provided is valid.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="examinationValidationContext"></param>
        /// <param name="validationContext"></param>
        /// <returns cref="ValidationResult"></returns>
        /// <exception cref="NullReferenceException"></exception>
        protected override ValidationResult IsValid(object value, ExaminationValidationContext examinationValidationContext, ValidationContext validationContext)
        {
            var thing = examinationValidationContext.Examination;
            var nhsNumber = value as string;
            if (string.IsNullOrEmpty(nhsNumber))
            {
                return ValidationResult.Success;
            }

            var examinationPersistence = (IAsyncQueryHandler<DuplicateExaminationByNhsNumberRetrievalQuery, Examination>)validationContext.GetService(typeof(IAsyncQueryHandler<DuplicateExaminationByNhsNumberRetrievalQuery, Examination>));
            if (examinationPersistence == null)
            {
                throw new NullReferenceException("examination persistence is null");
            }

            var possibleExamination = examinationPersistence.Handle(new DuplicateExaminationByNhsNumberRetrievalQuery(nhsNumber)).Result;

            return possibleExamination != null && examinationValidationContext.Examination.ExaminationId != possibleExamination.ExaminationId
                ? new ValidationResult(SystemValidationErrors.Duplicate.ToString())
                : ValidationResult.Success;
        }
    }
}
