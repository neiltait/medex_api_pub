using System;
using System.IO;
using Cosmonaut;
using Cosmonaut.Extensions.Microsoft.DependencyInjection;
using MedicalExaminer.Models;
using MedicalExaminer.Task.UpdateExaminations.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalExaminer.Task.UpdateExaminations
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("MedicalExaminer.Task.UpdateExaminations");

            var services = new ServiceCollection();

            services.AddLogging();
            services.AddSingleton<UpdateExaminationsService>();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json");

            var configuration = builder.Build();

            var cosmosSettings = new CosmosStoreSettings(
                configuration["CosmosDB:DatabaseId"],
                configuration["CosmosDB:URL"],
                configuration["CosmosDB:PrimaryKey"]);

            services.AddCosmosStore<Examination>(cosmosSettings, "Examinations");

            var serviceProvider = services.BuildServiceProvider();

            var service = serviceProvider.GetService<UpdateExaminationsService>();

            service.Handle().Wait();
        }
    }
}
