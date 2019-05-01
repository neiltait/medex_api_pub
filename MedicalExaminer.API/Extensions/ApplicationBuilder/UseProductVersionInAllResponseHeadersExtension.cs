using System.Reflection;
using Microsoft.AspNetCore.Builder;

namespace MedicalExaminer.API.Extensions.ApplicationBuilder
{
    /// <summary>
    /// Use Product Version In All Response Headers Extension
    /// </summary>
    public static class UseProductVersionInAllResponseHeadersExtension
    {
        /// <summary>
        /// Key in the Header Response for the Version
        /// </summary>
        public const string VersionKey = "X-Version";

        /// <summary>
        /// User Product Version In All Response Headers.
        /// </summary>
        /// <param name="app">The Application Builder.</param>
        public static void UseProductVersionInAllResponseHeaders(this IApplicationBuilder app)
        {
            app.Use((context, next) =>
            {
                var version = Assembly
                    .GetEntryAssembly()
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                    .InformationalVersion;

                context.Response.Headers[VersionKey] = version;
                return next.Invoke();
            });
        }
    }
}
