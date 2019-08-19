using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;
using AutoMapper;
using Cosmonaut;
using Cosmonaut.Extensions.Microsoft.DependencyInjection;
using MedicalExaminer.API.Authorization;
using MedicalExaminer.API.Authorization.ExaminationContext;
using MedicalExaminer.API.Extensions.ApplicationBuilder;
using MedicalExaminer.API.Extensions.Data;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models;
using MedicalExaminer.API.Services;
using MedicalExaminer.API.Services.Implementations;
using MedicalExaminer.BackgroundServices;
using MedicalExaminer.Common;
using MedicalExaminer.Common.Authorization;
using MedicalExaminer.Common.Authorization.Roles;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Extensions;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries;
using MedicalExaminer.Common.Queries.CaseBreakdown;
using MedicalExaminer.Common.Queries.CaseOutcome;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Queries.PatientDetails;
using MedicalExaminer.Common.Queries.Permissions;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Reporting;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Common.Services.CaseOutcome;
using MedicalExaminer.Common.Services.Examination;
using MedicalExaminer.Common.Services.Location;
using MedicalExaminer.Common.Services.MedicalTeam;
using MedicalExaminer.Common.Services.PatientDetails;
using MedicalExaminer.Common.Services.Permissions;
using MedicalExaminer.Common.Services.User;
using MedicalExaminer.Common.Settings;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Okta.Sdk;
using Okta.Sdk.Configuration;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace MedicalExaminer.API
{
    /// <summary>
    ///     Startup
    /// </summary>
    public class Startup
    {
        private const string ApiTitle = "Medical Examiner API";
        private const string ApiDescription = "The API for the Medical Examiner Service.";

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The Configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Add services to the container.
        /// </summary>
        /// <param name="services">Service Collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            var oktaSettings = services.ConfigureSettings<OktaSettings>(Configuration, "Okta");

            var cosmosDbSettings = services.ConfigureSettings<CosmosDbSettings>(Configuration, "CosmosDB");

            var backgroundServicesSettings =
                services.ConfigureSettings<BackgroundServicesSettings>(Configuration, "BackgroundServices");

            var locationMigrationSettings =
                services.ConfigureSettings<LocationMigrationSettings>(Configuration, "LocationMigrationSettings");


            if(locationMigrationSettings == null)
            {
                throw new ArgumentNullException(@"there is no location migration settings
example:
  LocationMigrationSettings: {
  Version: 1
  }
            ");
            }

            services.ConfigureSettings<AuthorizationSettings>(Configuration, "Authorization");

            services.ConfigureSettings<RequestChargeSettings>(Configuration, "RequestCharge");

            ConfigureOktaClient(services);

            services.AddSingleton<ITokenService, OktaTokenService>();

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddScoped<RequestChargeService>();

            // Database connections.
            services.AddSingleton<IDocumentClientFactory, DocumentClientFactory>();
            services.AddScoped<IDatabaseAccess, DatabaseAccess>();

            ConfigureQueries(services, cosmosDbSettings);

            Mapper.Initialize(config => { config.AddMedicalExaminerProfiles(); });
            Mapper.AssertConfigurationIsValid();
            services.AddAutoMapper();

            ConfigureAuthentication(services, oktaSettings, cosmosDbSettings);

            ConfigureAuthorization(services);

            services.AddMvcCore()
                .AddVersionedApiExplorer(options =>
                {
                    // The format of the version added to the route URL
                    options.GroupNameFormat = "'v'VVV";

                    // Tells swagger to replace the version in the controller route
                    options.SubstituteApiVersionInUrl = true;
                });

            services.AddMvc(options =>
            {
                options.UseExaminationContextModelBindingProvider();
                options.Filters.Add<GlobalExceptionFilter>();
                options.Filters.Add<ControllerActionFilter>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddExaminationValidation();
            services.AddApiVersioning(config => { config.ReportApiVersions = true; });



            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(options =>
            {
                // c.SwaggerDoc("v1", new Info { Title = "Medical Examiner Data API", Version = "v1" });

                // note: need a temporary service provider here because one has not been created yet
                var provider = services.BuildServiceProvider()
                    .GetRequiredService<IApiVersionDescriptionProvider>();

                // add a swagger document for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName,
                        CreateInfoForApiVersion(description, ApiTitle, ApiDescription));
                }

                // add a custom operation filter which sets default values
                options.OperationFilter<SwaggerDefaultValues>();

                // Make all enums appear as strings
                options.DescribeAllEnumsAsStrings();

                // Locate the XML file being generated by ASP.NET.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                // Swagger to use those XML comments.
                options.IncludeXmlComments(xmlPath);

                // Make swagger do authentication
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", Array.Empty<string>() },
                };

                options.AddSecurityDefinition("Okta", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "implicit",
                    AuthorizationUrl = oktaSettings.AuthorizationUrl,
                    Scopes = new Dictionary<string, string>
                    {
                        { "profile", "Profile" },
                        { "openid", "OpenID" },
                    },
                });

                options.AddSecurityDefinition("Bearer",
                    new ApiKeyScheme
                    {
                        Description =
                            "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = "header",
                        Type = "apiKey"
                    });

                options.AddSecurityRequirement(security);
            });

            // Logger.
            services.AddScoped<IMELogger, MELogger>();

 
            services.AddScoped<ControllerActionFilter>();

            var cosmosSettings = new CosmosStoreSettings(
                cosmosDbSettings.DatabaseId,
                new Uri(cosmosDbSettings.URL),
                cosmosDbSettings.PrimaryKey);

            const string examinationsCollection = "Examinations";
            services.AddCosmosStore<Examination>(cosmosSettings, examinationsCollection);
            services.AddCosmosStore<AuditEntry<Examination>>(cosmosSettings, examinationsCollection.AuditCollection());

            services.AddScoped<IMeLoggerPersistence>(s => new MeLoggerPersistence(
                new Uri(cosmosDbSettings.URL),
                cosmosDbSettings.PrimaryKey,
                cosmosDbSettings.DatabaseId));

            services.AddBackgroundServices(backgroundServicesSettings);

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            UpdateInvalidOrNullUserPermissionIds(serviceProvider);
            UpdateLocations(serviceProvider, locationMigrationSettings);
        }

        /// <summary>
        ///     Configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The App.</param>
        /// <param name="env">The Environment.</param>
        /// <param name="loggerFactory">The Logger Factory.</param>
        /// <param name="provider">API Version Description Privider.</param>
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IApiVersionDescriptionProvider provider)
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

            // Ensure collections available
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var databaseAccess = scope.ServiceProvider.GetRequiredService<IDatabaseAccess>();

                databaseAccess.EnsureCollectionAvailable(app.ApplicationServices.GetRequiredService<ILocationConnectionSettings>());
                databaseAccess.EnsureCollectionAvailable(app.ApplicationServices.GetRequiredService<IExaminationConnectionSettings>());
                databaseAccess.EnsureCollectionAvailable(app.ApplicationServices.GetRequiredService<IUserConnectionSettings>());
                databaseAccess.EnsureCollectionAvailable(app.ApplicationServices.GetRequiredService<IUserSessionConnectionSettings>());
            }

            app.UseMiddleware<ResponseTimeMiddleware>();
            app.UseMiddleware<ReportRUMiddleware>();

            // TODO: Not using HTTPS while we join front to back end
            //app.UseHttpsRedirection();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();


            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                // Use a bespoke Index that has OpenID/CustomJS to handle OKTA
                c.IndexStream = () =>
                    GetType().GetTypeInfo().Assembly
                        .GetManifestResourceStream("MedicalExaminer.API.SwaggerIndex.html");

                var oktaSettings = app.ApplicationServices.GetRequiredService<IOptions<OktaSettings>>();

                c.OAuthConfigObject = new OAuthConfigObject
                {
                    ClientId = oktaSettings.Value.ClientId,
                    ClientSecret = oktaSettings.Value.ClientSecret
                };
                c.OAuthAdditionalQueryStringParams(new Dictionary<string, string>
                    { { "nonce", Guid.NewGuid().ToString() } });

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }
            });


            // Must be above UseMvc
            app.UseProductVersionInAllResponseHeaders();

            // Must be above UseMvc
            app.UseAuthentication();

            app.UseCors("MyPolicy");

            app.UseMvc();
        }

        /// <summary>
        /// Configure Queries.
        /// </summary>
        /// <param name="services">Services.</param>
        /// <param name="cosmosDbSettings">Cosmos Database Settings.</param>
        private void ConfigureQueries(IServiceCollection services, CosmosDbSettings cosmosDbSettings)
        {
            services.AddSingleton<ILocationConnectionSettings>(s => new LocationConnectionSettings(
                new Uri(cosmosDbSettings.URL),
                cosmosDbSettings.PrimaryKey,
                cosmosDbSettings.DatabaseId));

            services.AddSingleton<IExaminationConnectionSettings>(s => new ExaminationConnectionSettings(
                new Uri(cosmosDbSettings.URL),
                cosmosDbSettings.PrimaryKey,
                cosmosDbSettings.DatabaseId));

            services.AddSingleton<IUserConnectionSettings>(s => new UserConnectionSettings(
                new Uri(cosmosDbSettings.URL),
                cosmosDbSettings.PrimaryKey,
                cosmosDbSettings.DatabaseId));

            services.AddSingleton<IUserSessionConnectionSettings>(s => new UserSessionConnectionSettings(
                new Uri(cosmosDbSettings.URL),
                cosmosDbSettings.PrimaryKey,
                cosmosDbSettings.DatabaseId));

            // Examination services
            services.AddScoped(s => new ExaminationsQueryExpressionBuilder());
            services
                .AddScoped<IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview>,
                    ExaminationsDashboardService>();
            services.AddScoped<IAsyncQueryHandler<CreateExaminationQuery, Examination>, CreateExaminationService>();
            services
                .AddScoped<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>, ExaminationRetrievalService>();
            services
                .AddScoped<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>,
                    ExaminationsRetrievalService>();

            services.AddScoped<LocationMigrationService, LocationMigrationService>();

            services
                .AddScoped<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>, ExaminationRetrievalService>();
            services
                .AddScoped<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>,
                    ExaminationsRetrievalService>();
            services.AddScoped<IAsyncQueryHandler<CreateEventQuery, EventCreationResult>, CreateEventService>();

            // Medical team services
            services.AddScoped<IAsyncUpdateDocumentHandler, MedicalTeamUpdateService>();

            // Case Outcome Services
            services.AddScoped<IAsyncQueryHandler<CloseCaseQuery, string>, CloseCaseService>();
            services.AddScoped<IAsyncQueryHandler<CoronerReferralQuery, string>, CoronerReferralService>();
            services
                .AddScoped<IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string>, SaveOutstandingCaseItemsService
                >();
            services
                .AddScoped<IAsyncQueryHandler<ConfirmationOfScrutinyQuery, Examination>, ConfirmationOfScrutinyService
                >();

            // Patient details services
            services
                .AddScoped<IAsyncQueryHandler<PatientDetailsUpdateQuery, Examination>, PatientDetailsUpdateService>();

            // User services
            services.AddScoped<IAsyncQueryHandler<CreateUserQuery, MeUser>, CreateUserService>();
            services.AddScoped<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>, UserRetrievalByEmailService>();
            services.AddScoped<IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser>, UserRetrievalByOktaIdService>();
            services.AddScoped<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>, UserRetrievalByIdService>();
            services.AddScoped<IAsyncQueryHandler<UserUpdateQuery, MeUser>, UserUpdateService>();
            services.AddScoped<IAsyncQueryHandler<UserUpdateOktaQuery, MeUser>, UserUpdateOktaService>();
            services.AddScoped<IAsyncQueryHandler<InvalidUserPermissionQuery, bool>, InvalidUserPermissionUpdateService>();
            services.AddScoped<IOktaClient, OktaClient>();

            // User session services
            services.AddScoped<IAsyncQueryHandler<UserSessionUpdateOktaTokenQuery, MeUserSession>, UserSessionUpdateOktaTokenService>();
            services.AddScoped<IAsyncQueryHandler<UserSessionRetrievalByOktaIdQuery, MeUserSession>, UserSessionRetrievalByOktaIdService>();

            services.AddScoped<IAsyncQueryHandler<ExaminationByNhsNumberRetrievalQuery, Examination>, ExaminationByNhsNumberRetrievalService>();
            // Used for roles; but is being abused to pass null and get all users.
            services.AddScoped<IAsyncQueryHandler<UsersRetrievalQuery, IEnumerable<MeUser>>, UsersRetrievalService>();
            services
                .AddScoped<IAsyncQueryHandler<UsersRetrievalByRoleLocationQuery, IEnumerable<MeUser>>,
                    UsersRetrievalByRoleLocationQueryService>();

            // Location Services
            services.AddScoped<IAsyncQueryHandler<LocationRetrievalByIdQuery, Location>, LocationIdService>();
            services
                .AddScoped<IAsyncQueryHandler<LocationsRetrievalByQuery, IEnumerable<Location>>, LocationsQueryService
                >();
            services
                .AddScoped<IAsyncQueryHandler<LocationParentsQuery, IEnumerable<Location>>, LocationParentsQueryService
                >();
            services
                .AddScoped<IAsyncQueryHandler<LocationsParentsQuery, IDictionary<string, IEnumerable<Location>>>,
                    LocationsParentsQueryService>();
        }

        /// <summary>
        ///     Configure basic authentication so we can use tokens.
        /// Configure basic authentication so we can use tokens.
        /// </summary>
        /// <param name="services">Services.</param>
        /// <param name="oktaSettings">Okta Settings.</param>
        /// <param name="cosmosDbSettings">Cosmos Database Settings.</param>
        private void ConfigureAuthentication(
            IServiceCollection services,
            OktaSettings oktaSettings,
            CosmosDbSettings cosmosDbSettings)
        {
            var oktaTokenExpiry = int.Parse(oktaSettings.LocalTokenExpiryTimeMinutes);
            var provider = services.BuildServiceProvider();
            var tokenService = provider.GetRequiredService<ITokenService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = oktaSettings.Authority;
                    options.Audience = oktaSettings.Audience;
                    options.SecurityTokenValidators.Clear();
                    options.SecurityTokenValidators.Add(
                        new OktaJwtSecurityTokenHandler(
                            tokenService,
                            new JwtSecurityTokenHandler(),
                            provider.GetRequiredService<IAsyncQueryHandler<UserSessionUpdateOktaTokenQuery, MeUserSession>>(),
                            provider.GetRequiredService<IAsyncQueryHandler<UserSessionRetrievalByOktaIdQuery, MeUserSession>>(),
                            provider.GetRequiredService<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>>(),
                            provider.GetRequiredService<IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser>>(),
                            provider.GetRequiredService<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>(),
                            provider.GetRequiredService<IAsyncQueryHandler<UserUpdateQuery, MeUser>>(),
                            provider.GetRequiredService<IAsyncQueryHandler<CreateUserQuery, MeUser>>(),
                            provider.GetRequiredService<OktaClient>(),
                            oktaTokenExpiry));
                });
        }

        /// <summary>
        /// Configure Okta Client.
        /// </summary>
        /// <param name="services">Services.</param>
        private void ConfigureOktaClient(IServiceCollection services)
        {
            // Configure okta client
            services.AddScoped(context =>
            {
                var settings = context.GetRequiredService<IOptions<OktaSettings>>();
                return new OktaClientConfiguration
                {
                    OktaDomain = settings.Value.Domain,
                    Token = settings.Value.SdkToken,
                };
            });
            services.AddScoped<OktaClient, OktaClient>();
        }

        /// <summary>
        /// Creates the information for API versions.
        /// </summary>
        /// <param name="description">The Description.</param>
        /// <param name="apiTitle">The API Title</param>
        /// <param name="apiDescription">The API Description</param>
        /// <returns>Info for Swagger</returns>
        private static Info CreateInfoForApiVersion(
            ApiVersionDescription description,
            string apiTitle,
            string apiDescription)
        {
            var info = new Info
            {
                Title = $"{apiTitle} {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = apiDescription
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }

        /// <summary>
        /// Configure Authorization.
        /// </summary>
        /// <param name="services">Services.</param>
        private void ConfigureAuthorization(IServiceCollection services)
        {
            services.AddSingleton<IRolePermissions, RolePermissions>();

            services.AddSingleton<IEnumerable<Common.Authorization.Role>>(er => new List<Common.Authorization.Role>()
            {
                new MedicalExaminerOfficerRole(),
                new MedicalExaminerRole(),
                new ServiceAdministratorRole(),
                new ServiceOwnerRole(),
            });

            services.AddAuthorization(options =>
            {
                foreach (var permission in(Permission[]) Enum.GetValues(typeof(Permission)))
                {
                    options.AddPolicy($"HasPermission={permission}",
                        policy => { policy.Requirements.Add(new PermissionRequirement(permission)); });
                }
            });

            // Needs to be scoped since it takes scoped parameters.
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();
            services.AddScoped<IAuthorizationHandler, DocumentPermissionHandler>();
            services.AddScoped<IPermissionService, PermissionService>();
        }

        private void UpdateLocations(IServiceProvider serviceProvider, LocationMigrationSettings locationMigrationSettings)
        {
            LocationMigrationService instance = serviceProvider.GetService<LocationMigrationService>();
            var result = instance.Handle(_locationMigrationQueryLookup[locationMigrationSettings.Version]);
        }

        private void UpdateInvalidOrNullUserPermissionIds(IServiceProvider serviceProvider)
        {
            IAsyncQueryHandler<InvalidUserPermissionQuery, bool> instance = serviceProvider.GetService<IAsyncQueryHandler<InvalidUserPermissionQuery, bool>>();

            instance.Handle(new InvalidUserPermissionQuery());
        }

        private Dictionary<int, IMigrationQuery> _locationMigrationQueryLookup = new Dictionary<int, IMigrationQuery>
        {
            {1, new LocationMigrationQueryV1() }
        };
    }
}