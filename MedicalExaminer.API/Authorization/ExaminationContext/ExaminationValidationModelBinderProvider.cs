using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MedicalExaminer.API.Authorization.ExaminationContext
{
    /// <summary>
    /// Examination Validation Model Binder Provider.
    /// </summary>
    /// <inheritdoc/>
    public class ExaminationValidationModelBinderProvider : IModelBinderProvider
    {
        private readonly IModelBinderProvider _originalModelBinderProvider;

        /// <summary>
        /// Initialise a new instance of <see cref="ExaminationValidationModelBinderProvider"/>.
        /// </summary>
        /// <param name="originalModelBinderProvider">Original Model Binder Provider.</param>
        public ExaminationValidationModelBinderProvider(IModelBinderProvider originalModelBinderProvider)
        {
            _originalModelBinderProvider = originalModelBinderProvider;
        }

        /// <inheritdoc/>
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(IExaminationValidationModel))
            {
                var originalModelBinderProvider = _originalModelBinderProvider.GetBinder(context);

                return new ExaminationValidationModelBinder(originalModelBinderProvider);
            }

            return null;
        }
    }
}
