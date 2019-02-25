using System;
using System.Text;
using Medical_Examiner_API.Helpers;
using Medical_Examiner_API.Loggers;
using Medical_Examiner_API.Persistence;
using Medical_Examiner_API.Services;
using Medical_Examiner_API.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Medical_Examiner_API
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Key for Authentication Section
        /// </summary>
        private const string AuthenticationSection = "Authentication";

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
            // Basic authentication service
            ConfigureAuthenticationSettings(services);
            ConfigureAuthentication(services);
            services.AddScoped<IAuthenticationService, AuthenticationService>();

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

            // Must be above use mvc
            app.UseAuthentication();

            app.UseMvc();
        }

        /// <summary>
        /// Configure Authentication settings so we can use it elsewhere in the app using DI.
        /// </summary>
        /// <param name="services">Services.</param>
        private void ConfigureAuthenticationSettings(IServiceCollection services)
        {
            var authenticationSection = Configuration.GetSection(AuthenticationSection);
            services.Configure<AuthenticationSettings>(authenticationSection);
        }

        /// <summary>
        /// Configure basic authentication so we can use tokens.
        /// </summary>
        /// <param name="services">Services.</param>
        private void ConfigureAuthentication(IServiceCollection services)
        {
            var authenticationSection = Configuration.GetSection(AuthenticationSection);
            var authenticationSettings = authenticationSection.Get<AuthenticationSettings>();

            var key = Encoding.ASCII.GetBytes(authenticationSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = false,
                };
            });
        }
    }
}
