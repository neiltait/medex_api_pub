using System;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries;
using MedicalExaminer.Common.Services;
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
    /// <typeparam name="TService">Finalyl the Service.</typeparam>
    public abstract class ServiceTestsBase<TQuery, TConnectionSettings, TItem, TType, TService>
        where TQuery : class, IQuery<TItem>
        where TConnectionSettings : class, IConnectionSettings
        where TService : QueryHandler<TQuery, TItem>
    {
        /// <summary>
        /// The Service under test.
        /// </summary>
        protected TService Service { get; }

        /// <summary>
        /// Initialise a new instance of <see cref="ServiceTestsBase{TQuery,TConnectionSettings,TItem,TType,TService}"/>.
        /// </summary>
        protected ServiceTestsBase()
        {
            // Make sure you don't access the sub class inside this method since its being called in constructor.
            var client = CosmosMocker.CreateDocumentClient(GetExamples());
            var clientFactory = CosmosMocker.CreateClientFactory(client);
            var dataAccess = new DatabaseAccess(clientFactory.Object);
            var connectionSettings = CosmosMocker.CreateConnectionSettings<TConnectionSettings>();

            Service = GetService(dataAccess, connectionSettings.Object);
        }

        /// <summary>
        /// Base method to construct simple services. Override if you need to pass in other defaults.
        /// </summary>
        /// <param name="databaseAccess"></param>
        /// <param name="connectionSettings"></param>
        /// <returns></returns>
        protected virtual TService GetService(IDatabaseAccess databaseAccess, TConnectionSettings connectionSettings)
        {
            var service = (TService)Activator.CreateInstance(typeof(TService), databaseAccess, connectionSettings);

            return service;
        }

        /// <summary>
        /// Query is null throws an exception.
        /// </summary>
        [Fact]
        public void QueryIsNullThrowsException()
        {
            // Arrange
            var connectionSettings = CosmosMocker.CreateConnectionSettings<TConnectionSettings>();
            var dbAccess = new Mock<IDatabaseAccess>();

            var sut = GetService(dbAccess.Object, connectionSettings.Object);

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
