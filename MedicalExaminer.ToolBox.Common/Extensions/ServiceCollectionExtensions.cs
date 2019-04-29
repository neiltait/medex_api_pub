using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper.Configuration;
using Cosmonaut;
using Cosmonaut.Extensions.Microsoft.DependencyInjection;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Common.Services.Examination;
using MedicalExaminer.Common.Services.User;
using MedicalExaminer.Models;
using MedicalExaminer.ToolBox.Common.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace MedicalExaminer.ToolBox.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureToolBox(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            var cosmosSettings = new CosmosStoreSettings(
                configuration["CosmosDB:DatabaseId"],
                configuration["CosmosDB:URL"],
                configuration["CosmosDB:PrimaryKey"]);

            services.AddCosmosStore<MeUser>(cosmosSettings, "Users");
            services.AddCosmosStore<Location>(cosmosSettings, "Locations");
            services.AddCosmosStore<Permission>(cosmosSettings, "Permissions");


            services.AddScoped<IDocumentClientFactory, DocumentClientFactory>();
            services.AddScoped<IDatabaseAccess, DatabaseAccess>();

            services.AddScoped<IUserConnectionSettings>(s => new UserConnectionSettings(
                new Uri(configuration["CosmosDB:URL"]),
                configuration["CosmosDB:PrimaryKey"],
                configuration["CosmosDB:DatabaseId"]));

            services.AddScoped<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>, UserRetrievalByIdService>();
            services.AddScoped<IAsyncQueryHandler<UserUpdateQuery, MeUser>, UserUpdateService>();

            services.AddScoped<IAsyncQueryHandler<UsersRetrievalQuery, IEnumerable<MeUser>>, UsersRetrievalService>();

            services.AddScoped<ImpersonateUserService>();
            services.AddScoped<GenerateConfigurationService>();

            return services;
        }
    }
}
