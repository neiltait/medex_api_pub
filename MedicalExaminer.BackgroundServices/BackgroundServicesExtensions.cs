using MedicalExaminer.BackgroundServices.Services;
using MedicalExaminer.Common.Settings;
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
        /// <param name="backgroundServicesSettings">The background services settings.</param>
        /// <returns>The Service Collection.</returns>
        public static IServiceCollection AddBackgroundServices(
            this IServiceCollection services, 
            BackgroundServicesSettings backgroundServicesSettings)
        {
            if (backgroundServicesSettings == null)
            {
                return services;
            }

            services.AddSingleton<IScheduler, Scheduler>();

            // All hosted services will use this same configuration unless different interfaces are used.
            services.AddSingleton<IScheduledServiceConfiguration, ScheduledServiceEveryDayAtSetTime>(
                serviceProvider => new ScheduledServiceEveryDayAtSetTime(
                    backgroundServicesSettings.TimeToRunEachDay,
                    backgroundServicesSettings.SampleRate));

            services.AddHostedService<UpdateExaminationsService>();

            return services;
        }
    }
}
