using System;
using MedicalExaminer.BackgroundServices.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalExaminer.BackgroundServices
{
    /// <summary>
    /// Background Services Extensions.
    /// </summary>
    public static class BackgroundServicesExtensions
    {
        /// <summary>
        /// Add Background Services.
        /// </summary>
        /// <param name="services">The Service Collection.</param>
        /// <returns>The Service Collection.</returns>
        public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
        {
            services.AddSingleton<IScheduledServiceConfiguration, ScheduledServiceEveryDayAtSetTime>(
                serviceProvider => new ScheduledServiceEveryDayAtSetTime(
                    TimeSpan.Parse("02:00"), 
                    TimeSpan.FromSeconds(30)));
            services.AddHostedService<UpdateExaminationsService>();

            return services;
        }
    }
}
