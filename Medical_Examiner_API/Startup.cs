using System;
using Medical_Examiner_API.Loggers;
using Medical_Examiner_API.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Medical_Examiner_API
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initialise a new instance of Startup
        /// </summary>
        /// <param name="configuration">The Configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Add services to the container.
        /// </summary>
        /// <param name="services">Service Collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped<IMELogger, MELogger>();

            services.AddScoped<ControllerActionFilter>();

            services.AddScoped<IExaminationPersistence>((s) =>
            {
                return new ExaminationPersistence(
                    new Uri(Configuration["CosmosDB:URL"]),
                    Configuration["CosmosDB:PrimaryKey"]);
            });

            services.AddScoped<IUserPersistence>((s) =>
            {
                return new UserPersistence(
                    new Uri(Configuration["CosmosDB:URL"]),
                    Configuration["CosmosDB:PrimaryKey"]);
            });

            services.AddScoped<IMELoggerPersistence>((s) =>
            {
                return new MELoggerPersistence(
                    new Uri(Configuration["CosmosDB:URL"]),
                    Configuration["CosmosDB:PrimaryKey"]);
            });
        }

        /// <summary>
        /// Configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The App.</param>
        /// <param name="env">The Environment.</param>
        /// <param name="loggerFactory">The Logger Factory.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                    });
                });
            }

            app.UseHttpsRedirection();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
