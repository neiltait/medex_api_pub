using System;
using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Extensions.Data;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries;
using MedicalExaminer.Common.Reporting;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Common.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;
// ReSharper disable VirtualMemberCallInConstructor

namespace MedicalExaminer.API.Tests.Services
{
    /// <summary>
    /// Service Test Base.
    /// </summary>
    /// <typeparam name="TQuery">The Query being Tested.</typeparam>
    /// <typeparam name="TConnectionSettings">The Connection String class.</typeparam>
    /// <typeparam name="TItem">The Item being returned.</typeparam>
    /// <typeparam name="TType">The Type of item being returned. May be same as TItem if not a collection.</typeparam>
    /// <typeparam name="TService">Finally the Service.</typeparam>
    public abstract class ServiceTestsBase<TQuery, TConnectionSettings, TItem, TType, TService>
        where TQuery : class, IQuery<TItem>
        where TConnectionSettings : class, IConnectionSettings
        where TService : QueryHandler<TQuery, TItem>
        where TType : class, new()
    {
        /// <summary>
        /// Initialise a new instance of <see cref="ServiceTestsBase{TQuery,TConnectionSettings,TItem,TType,TService}"/>.
        /// </summary>
        protected ServiceTestsBase()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            // Init mapper
            var mapperConfiguration = new MapperConfiguration(config => { config.AddMedicalExaminerProfiles(); });
            mapperConfiguration.AssertConfigurationIsValid();
            var mapper = mapperConfiguration.CreateMapper();

            // Make sure you don't access the sub class inside this method since its being called in constructor.
            var client = CosmosMocker.CreateDocumentClient(GetExamples());
            var clientFactory = CosmosMocker.CreateClientFactory(client);
            var dataAccess = new DatabaseAccess(clientFactory.Object, new RequestChargeService(), new OptionsWrapper<CosmosDbSettings>(new CosmosDbSettings()));
            var connectionSettings = CosmosMocker.CreateConnectionSettings<TConnectionSettings>();

            serviceCollection.AddTransient(s => mapper);

            serviceCollection.AddTransient<IDatabaseAccess>(s => dataAccess);
            serviceCollection.AddTransient(s => (IUserConnectionSettings)connectionSettings.Object);
            serviceCollection.AddTransient(s => (IExaminationConnectionSettings)connectionSettings.Object);
            serviceCollection.AddTransient(s => (ILocationConnectionSettings)connectionSettings.Object);

            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            Service = GetService();
        }

        /// <summary>
        /// Service Provider.
        /// </summary>
        protected IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// The Service under test.
        /// </summary>
        protected TService Service { get; }

        /// <summary>
        /// Query is null throws an exception.
        /// </summary>
        [Fact]
        public void QueryIsNullThrowsException()
        {
            // Arrange
            var sut = GetService();

            // Act
            Action act = () => sut.Handle(null).GetAwaiter().GetResult();

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<TService>();
        }

        protected TService GetService()
        {
            return ServiceProvider.GetRequiredService<TService>();
        }

        /// <summary>
        /// Get Examples
        /// </summary>
        /// <remarks>Called from constructor so return only; do not interact with the sub class since it wont have been set up yet.</remarks>
        /// <returns>Array of examples.</returns>
        protected abstract TType[] GetExamples();
    }
}
