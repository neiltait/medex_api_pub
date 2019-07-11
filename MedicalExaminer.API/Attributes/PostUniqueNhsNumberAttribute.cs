using System;
using System.ComponentModel.DataAnnotations;
using MedicalExaminer.API.Models.v1;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Attributes
{
    /// <summary>
    /// This validates that the nhs number is unique when a new examination is created.
    /// </summary>
    public class PostUniqueNhsNumberAttribute : ValidationAttribute
    {
        /// <summary>
        /// returns if the nhs number provided is valid.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="context"></param>
        /// <returns cref="ValidationResult"></returns>
        /// <exception cref="NullReferenceException"></exception>
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var nhsNumber = value as string;
            if (string.IsNullOrEmpty(nhsNumber))
            {
                return ValidationResult.Success;
            }

            var examinationPersistence = (IAsyncQueryHandler<ExaminationByNhsNumberRetrievalQuery, Examination>)context.GetService(typeof(IAsyncQueryHandler<ExaminationByNhsNumberRetrievalQuery, Examination>));
            if (examinationPersistence == null)
            {
                throw new NullReferenceException("examination persistence is null");
            }

            var existingExamination = examinationPersistence.Handle(new ExaminationByNhsNumberRetrievalQuery(nhsNumber)).Result;

            return existingExamination != null
                ? new ValidationResultEnumCodes(SystemValidationErrors.DuplicateNhsNumber)
                : ValidationResult.Success;
        }
    }
}
