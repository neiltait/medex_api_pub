using System;
using System.Collections.Generic;
using Cosmonaut;
using Cosmonaut.Extensions.Microsoft.DependencyInjection;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Common.Services.User;
using MedicalExaminer.Common.Settings;
using MedicalExaminer.Models;
using MedicalExaminer.ToolBox.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalExaminer.ToolBox.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureToolBox(this IServiceCollection services, CosmosDbSettings cosmosDbSettings)
        {
            var cosmosSettings = new CosmosStoreSettings(
                cosmosDbSettings.DatabaseId,
                cosmosDbSettings.URL,
                cosmosDbSettings.PrimaryKey);

            services.AddCosmosStore<MeUser>(cosmosSettings, "Users");
            services.AddCosmosStore<Location>(cosmosSettings, "Locations");
            services.AddCosmosStore<Examination>(cosmosSettings, "Examinations");

            services.AddScoped<IDocumentClientFactory, DocumentClientFactory>();
            services.AddScoped<IDatabaseAccess, DatabaseAccess>();

            services.AddScoped<IUserConnectionSettings>(s => new UserConnectionSettings(
                new Uri(cosmosDbSettings.URL),
                cosmosDbSettings.PrimaryKey,
                cosmosDbSettings.DatabaseId));

            services.AddScoped<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>, UserRetrievalByIdService>();
            services.AddScoped<IAsyncQueryHandler<UserUpdateQuery, MeUser>, UserUpdateService>();

            services.AddScoped<IAsyncQueryHandler<UsersRetrievalQuery, IEnumerable<MeUser>>, UsersRetrievalService>();

            services.AddScoped<ImpersonateUserService>();
            services.AddScoped<GenerateConfigurationService>();
            services.AddScoped<ImportDocumentService>();
            services.AddScoped<GetLocationTreeService>();

            return services;
        }
    }
}
