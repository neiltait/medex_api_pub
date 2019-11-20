using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.Extensions;
using MedicalExaminer.Common.Settings;
using MedicalExaminer.ToolBox.Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MedicalExaminer.ToolBox.TearDownDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection, configuration);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            Task.Run(async () => { await RunAsync(serviceProvider); })
                .GetAwaiter()
                .GetResult();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);

            services
                .AddLogging(configure =>
                {
                    configure.AddConfiguration(configuration.GetSection("Logging"));
                    configure.AddConsole();
                    configure.AddDebug();
                });

            var cosmosDbSettings = services.ConfigureSettings<CosmosDbSettings>(configuration, "CosmosDB");

            services.ConfigureToolBox(cosmosDbSettings);

            services.AddTransient<TearDownDatabase>();
        }

        private static async Task RunAsync(IServiceProvider provider)
        {
            var service = provider.GetService<TearDownDatabase>();

            await service.TearDown();
        }
    }
}