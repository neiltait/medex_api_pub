using System;
using System.ComponentModel.DataAnnotations;
using MedicalExaminer.API.Authorization.ExaminationContext;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Attributes
{
    public class PutUniqueNhsNumberAttribute : ExaminationValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ExaminationValidationContext examinationValidationContext, ValidationContext validationContext)
        {
            var thing = examinationValidationContext.Examination;
            var nhsNumber = value as string;
            if (string.IsNullOrEmpty(nhsNumber))
            {
                return ValidationResult.Success;
            }

            var examinationPersistence = (IAsyncQueryHandler<ExaminationByNhsNumberRetrievalQuery, Examination>)validationContext.GetService(typeof(IAsyncQueryHandler<ExaminationByNhsNumberRetrievalQuery, Examination>));
            if (examinationPersistence == null)
            {
                throw new NullReferenceException("examination persistence is null");
            }

            var possibleExamination = examinationPersistence.Handle(new ExaminationByNhsNumberRetrievalQuery(nhsNumber)).Result;

            return possibleExamination != null && examinationValidationContext.Examination.ExaminationId != possibleExamination.ExaminationId
                ? new ValidationResult("There is already an Examination for this patient")
                : ValidationResult.Success;
        }
    }
}
