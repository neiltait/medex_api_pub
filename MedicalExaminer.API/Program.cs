using MedicalExaminer.Common.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace MedicalExaminer.API
{
    /// <summary>
    /// Program hosting the API.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main Entry Point.
        /// </summary>
        /// <param name="args">Argyments.</param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Create the Web Host Builder.
        /// </summary>
        /// <param name="args">Pass through of arguments.</param>
        /// <returns>Web Host Builder.</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            // Not using https for now until we resolve certificates.
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureSharedSettings()
                .UseStartup<Startup>()
                .UseApplicationInsights();
        }
    }
}
