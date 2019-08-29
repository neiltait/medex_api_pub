using System;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace MedicalExaminer.API.Authorization.ExaminationContext
{
    /// <summary>
    /// Examination Validation Model Binder.
    /// </summary>
    /// <inheritdoc/>
    public class ExaminationValidationModelBinder : IModelBinder
    {
        private readonly IModelBinder _originalModelBinder;

        /// <summary>
        /// Initialise a new instance of <see cref="ExaminationValidationModelBinder"/>.
        /// </summary>
        /// <param name="originalModelBinder">Original Model Binder.</param>
        public ExaminationValidationModelBinder(IModelBinder originalModelBinder)
        {
            _originalModelBinder = originalModelBinder;
        }

        /// <inheritdoc/>
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (_originalModelBinder == null)
            {
                throw new InvalidOperationException("Original Model Binder Required to do Examination Validation Model Binding");
            }

            // Re use the existing binder to do the actual binding of the model.
            await _originalModelBinder.BindModelAsync(bindingContext).ConfigureAwait(false);

            // If it failed then don't continue.
            if (bindingContext.Result.Model == null)
            {
                return;
            }

            if (!(bindingContext.ModelMetadata is DefaultModelMetadata defaultModelMetadata))
            {
                return;
            }

            var attribute = (ExaminationValidationModelBinderContextAttribute)defaultModelMetadata
                .Attributes
                .Attributes
                .FirstOrDefault(a => a is ExaminationValidationModelBinderContextAttribute);

            if (attribute == null)
            {
                return;
            }

            // This gets the field being used to reference an examination, should return "examinationId"
            var examinationIdName = attribute.ParameterName;

            // This then gets the value of the field. The examination id itself.
            var examinationId = bindingContext.ValueProvider.GetValue(examinationIdName).FirstValue;

            var examinationRetrievalService =
                (IAsyncQueryHandler<ExaminationRetrievalQuery, MedicalExaminer.Models.Examination>) bindingContext
                    .HttpContext.RequestServices.GetService(
                        typeof(IAsyncQueryHandler<ExaminationRetrievalQuery, MedicalExaminer.Models.Examination>));

            if (examinationRetrievalService == null)
            {
                throw new InvalidOperationException("Examination retrieval service has not been registered.");
            }

            // Get the examination.
            var examination = await examinationRetrievalService
                .Handle(new ExaminationRetrievalQuery(examinationId, null));

            var examinationValidationContextFactory =
                (ExaminationValidationContextFactory) bindingContext.HttpContext.RequestServices.GetService(
                    typeof(ExaminationValidationContextFactory));
            var examinationValidationContextProvider =
                (ExaminationValidationContextProvider) bindingContext.HttpContext.RequestServices.GetService(
                    typeof(ExaminationValidationContextProvider));

            // Create the context with the examination and set.
            var examinationValidationContext = await examinationValidationContextFactory.Create(examination);
            examinationValidationContextProvider.Set(examinationValidationContext);
        }
    }
}
