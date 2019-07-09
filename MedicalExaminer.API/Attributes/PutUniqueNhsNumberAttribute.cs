using System;
using System.ComponentModel.DataAnnotations;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Attributes
{
    public class PutUniqueNhsNumberAttribute : ValidationAttribute
    {
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

            var validatedLocation = examinationPersistence.Handle(new ExaminationByNhsNumberRetrievalQuery(nhsNumber)).Result;

            return validatedLocation != null
                ? new ValidationResult("There is already an Examination for this patient")
                : ValidationResult.Success;
        }
    }
}
