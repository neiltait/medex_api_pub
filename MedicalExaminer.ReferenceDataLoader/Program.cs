using System;
using System.IO;
using System.Threading.Tasks;
using MedicalExaminer.Common.Extensions;
using MedicalExaminer.Common.Settings;
using MedicalExaminer.ReferenceDataLoader.Loaders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MedicalExaminer.ReferenceDataLoader
{
    /// <summary>
    /// Program Entry Point.
    /// </summary>
    public class Program
    {
        private static async Task Main(string[] args)
        {
            var hostBuilder = new HostBuilder()
                .UseEnvironment(EnvironmentName.Development)
                .ConfigureSharedSettings()
                .ConfigureServices((context, services) =>
                {
                    services.ConfigureSettings<CosmosDbSettings>(context.Configuration, "CosmosDB");

                    services.AddHostedService<LocationsLoader>();
                });

            await hostBuilder.RunConsoleAsync();

            Console.ReadLine();
        }
    }
}