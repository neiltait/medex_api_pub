using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalExaminer.API.Extensions
{
    /// <summary>
    /// Settings Extensions.
    /// </summary>
    public static class SettingsExtensions
    {
        /// <summary>
        /// Configure Settings.
        /// </summary>
        /// <typeparam name="T">Type of Setting.</typeparam>
        /// <param name="services">The services collection.</param>
        /// <param name="configuration">Startup configuration.</param>
        /// <param name="name">String name in settings file.</param>
        /// <returns>The settings object.</returns>
        public static T ConfigureSettings<T>(this IServiceCollection services, IConfiguration configuration, string name)
            where T : class
        {
            var settingsSection = configuration.GetSection(name);
            var settings = settingsSection.Get<T>();
            services.Configure<T>(settingsSection);
            return settings;
        }
    }
}
