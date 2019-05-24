using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalExaminer.API.Authorization.ExaminationContext
{
    /// <summary>
    /// Examination Context Extensions.
    /// </summary>
    public static class ExaminationContextExtensions
    {
        /// <summary>
        /// User Examination Context Model Binding Provider.
        /// </summary>
        /// <remarks>Model binding happens in order from first to last. Once a binder completes no further binders are checked so we have to inject ourselves into the list above the default one.</remarks>
        /// <param name="options">Mvc Options.</param>
        public static void UseExaminationContextModelBindingProvider(this MvcOptions options)
        {
            var originalModelBinderProvider =
                options.ModelBinderProviders.FirstOrDefault(x => x.GetType() == typeof(BodyModelBinderProvider));

            if (originalModelBinderProvider == null)
            {
                return;
            }

            var index = options.ModelBinderProviders.IndexOf(originalModelBinderProvider);

            options.ModelBinderProviders.Insert(index, new ExaminationValidationModelBinderProvider(originalModelBinderProvider));
        }

        /// <summary>
        /// Add Examination Validation.
        /// </summary>
        /// <param name="services">Service Collection.</param>
        public static void AddExaminationValidation(this IServiceCollection services)
        {
            services.AddScoped<ExaminationValidationContextFactory>();
            services.AddScoped<ExaminationValidationContextProvider>();
        }
    }
}
