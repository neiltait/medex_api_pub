using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MedicalExaminer.Common.Extensions
{
    /// <summary>
    /// Shared Settings Extension
    /// </summary>
    public static class SharedSettingsExtension
    {
        /// <summary>
        /// Filename of shared settings.
        /// </summary>
        private const string SharedSettingsFileName = "sharedsettings";

        /// <summary>
        /// Configure Shared Settings.
        /// </summary>
        /// <param name="webHostBuilder">The web host builder.</param>
        /// <returns>The web host builder passed in.</returns>
        public static IWebHostBuilder ConfigureSharedSettings(this IWebHostBuilder webHostBuilder)
        {
            return webHostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;

                var paths = new[]
                {
                    Path.Combine(
                        env.ContentRootPath,
                        "..",
                        $"{SharedSettingsFileName}.json"),

                    Path.Combine(
                        env.ContentRootPath,
                        "..",
                        $"{SharedSettingsFileName}.{env.EnvironmentName}.json")
                };

                foreach (var path in paths)
                {
                    config.AddJsonFile(path, optional: true);
                }
            });
        }

        /// <summary>
        /// Configure Shared Settings.
        /// </summary>
        /// <param name="hostBuilder">The host builder.</param>
        /// <returns>The web host builder passed in.</returns>
        public static IHostBuilder ConfigureSharedSettings(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;

                var paths = new[]
                {
                    Path.Combine(
                        env.ContentRootPath,
                        $"{SharedSettingsFileName}.json"),

                    Path.Combine(
                        env.ContentRootPath,
                        $"{SharedSettingsFileName}.{env.EnvironmentName}.json")
                };

                foreach (var path in paths)
                {
                    config.AddJsonFile(path, optional: true);
                }
            });
        }
    }
}
