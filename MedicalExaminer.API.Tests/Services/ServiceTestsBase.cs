using System;
using AutoMapper;
using Cosmonaut;
using FluentAssertions;
using MedicalExaminer.API.Extensions.Data;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries;
using MedicalExaminer.Common.Services;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.DependencyInjection;
using Moq;
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
        where TType : class
    {
        /// <summary>
        /// The Service under test.
        /// </summary>
        protected TService Service { get; }

        protected IServiceProvider _serviceProvider;

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
            var dataAccess = new DatabaseAccess(clientFactory.Object);
            var connectionSettings = CosmosMocker.CreateConnectionSettings<TConnectionSettings>();

            serviceCollection.AddTransient<IMapper>(s => mapper);

            serviceCollection.AddTransient<IDatabaseAccess>(s => dataAccess);
            //serviceCollection.AddTransient<IConnectionSettings>(s => connectionSettings.Object);
            serviceCollection.AddTransient(s => (IUserConnectionSettings)connectionSettings.Object);
            serviceCollection.AddTransient(s => (IExaminationConnectionSettings)connectionSettings.Object);
            serviceCollection.AddTransient(s => (ILocationConnectionSettings)connectionSettings.Object);

            ConfigureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();

            Service = GetService();
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<TService>();
        }

        protected TService GetService()
        {
            return _serviceProvider.GetRequiredService<TService>();
        }

        /*
        /// <summary>
        /// Base method to construct simple services. Override if you need to pass in other defaults.
        /// </summary>
        /// <param name="databaseAccess"></param>
        /// <param name="connectionSettings"></param>
        /// <returns></returns>
        protected virtual TService GetService(IDatabaseAccess databaseAccess, TConnectionSettings connectionSettings, ICosmosStore<TType> cosmosStore = null)
        {
            if (cosmosStore != null)
            {
                return  (TService)Activator.CreateInstance(typeof(TService), databaseAccess, connectionSettings, cosmosStore);
            }

            return (TService)Activator.CreateInstance(typeof(TService), databaseAccess, connectionSettings);
        }*/

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

        /// <summary>
        /// Get Examples
        /// </summary>
        /// <remarks>Called from constructor so return only; do not interact with the sub class since it wont have been set up yet.</remarks>
        /// <returns></returns>
        protected abstract TType[] GetExamples();
    }
}
