using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cosmonaut;
using Cosmonaut.Response;
using FluentAssertions;
using MedicalExaminer.API.Tests.Services;
using MedicalExaminer.BackgroundServices.Services;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Models;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Moq;
using Xunit;

namespace MedicalExaminer.BackgroundServices.Tests.Services
{
    /// <summary>
    /// Update Examinations Service Tests.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Test method names are self documenting.")]
    public class UpdateExaminationsServiceTests
    {
        private readonly Mock<ILogger<UpdateExaminationsService>> _logger;

        private readonly Mock<IScheduledServiceConfiguration> _configurationMock;

        private readonly Mock<IScheduler> _schedulerMock;

        private readonly Mock<IServiceProvider> _serviceProviderMock;

        private readonly UpdateExaminationsService _sut;

        public UpdateExaminationsServiceTests()
        {
            _logger = new Mock<ILogger<UpdateExaminationsService>>(MockBehavior.Strict);
            _configurationMock = new Mock<IScheduledServiceConfiguration>(MockBehavior.Strict);
            _schedulerMock = new Mock<IScheduler>(MockBehavior.Strict);
            _serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            _logger
                .Setup(s => s.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()))
                .Verifiable();

            _schedulerMock
                .SetupGet(s => s.LastExecuted)
                .Returns(DateTime.Parse("01/01/2019 00:00:00"));

            _sut = new UpdateExaminationsService(
                _logger.Object,
                _configurationMock.Object,
                _schedulerMock.Object,
                _serviceProviderMock.Object);
        }

        [Fact]
        public void ExecuteAsync_UpdatesExaminationsAndCreatesAudits()
        {
            // Arrange
            var expectedDateTime1 = DateTime.Parse("02/01/2019 00:01:00");
            var expectedDateTime2 = DateTime.Parse("02/01/2019 00:02:00");
            var startingModified = DateTime.MinValue;

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var startingExaminations = new List<Examination>()
            {
                new Examination()
                {
                    ModifiedAt = startingModified,
                }
            };

            List<Examination> updatedExaminations = null;
            List<AuditEntry<Examination>> createdAudits = null;

            // Capture the update so we can test it at the end.
            var store = CosmosMocker.CreateCosmosStore(startingExaminations.ToArray());
            store
                .Setup(s => s.UpdateRangeAsync(
                    It.IsAny<IEnumerable<Examination>>(),
                    null,
                    It.IsAny<CancellationToken>()))
                .Returns((IEnumerable<Examination> examinations, RequestOptions ro, CancellationToken ct) =>
                {
                    updatedExaminations = examinations.ToList();
                    return Task.FromResult(new CosmosMultipleResponse<Examination>());
                });

            // Capture any created audits.
            var auditStore = CosmosMocker.CreateCosmosStore(new AuditEntry<Examination>[] { });
            auditStore
                .Setup(s => s.AddRangeAsync(
                    It.IsAny<IEnumerable<AuditEntry<Examination>>>(),
                    null,
                    It.IsAny<CancellationToken>()))
                .Returns((IEnumerable<AuditEntry<Examination>> audits, RequestOptions ro, CancellationToken ct) =>
                {
                    createdAudits = audits.ToList();
                    return Task.FromResult(new CosmosMultipleResponse<AuditEntry<Examination>>());
                });

            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>(MockBehavior.Strict);
            var serviceScopeMock = new Mock<IServiceScope>(MockBehavior.Strict);

            serviceScopeFactoryMock
                .Setup(ssf => ssf.CreateScope())
                .Returns(serviceScopeMock.Object);

            serviceScopeMock
                .SetupGet(ss => ss.ServiceProvider)
                .Returns(_serviceProviderMock.Object);

            serviceScopeMock
                .Setup(ss => ss.Dispose())
                .Verifiable();

            _serviceProviderMock
                .Setup(sp => sp.GetService(typeof(IServiceScopeFactory)))
                .Returns(serviceScopeFactoryMock.Object);

            _serviceProviderMock
                .Setup(sp => sp.GetService(typeof(ICosmosStore<Examination>)))
                .Returns(store.Object);
            _serviceProviderMock
                .Setup(sp => sp.GetService(typeof(ICosmosStore<AuditEntry<Examination>>)))
                .Returns(auditStore.Object);

            _schedulerMock
                .Setup(s => s.Delay(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _schedulerMock
                .Setup(s => s.Delay(It.IsAny<TimeSpan>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _configurationMock
                .SetupGet(c => c.SampleRate)
                .Returns(TimeSpan.FromMilliseconds(1));

            _configurationMock
                .Setup(c => c.CanExecute(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns((DateTime dateTime, DateTime last) =>
                {
                    if (dateTime == expectedDateTime1)
                    {
                        // We only care about a single run.
                        return true;
                    }

                    cancellationTokenSource.Cancel();
                    return false;
                });

            _schedulerMock
                .SetupSequence(s => s.UtcNow)
                .Returns(expectedDateTime1)
                .Returns(expectedDateTime2);

            // Act
            var start = _sut.StartAsync(cancellationToken);

            // Assert
            start.Status.Should().Be(TaskStatus.RanToCompletion);

            foreach (var updatedExamination in updatedExaminations)
            {
                updatedExamination.GetCaseUrgencyScore().Should().NotBe(-1);
                updatedExamination.LastModifiedBy.Should().Be("UpdateExaminationService");
                updatedExamination.ModifiedAt.Should().NotBe(startingModified);
            }

            createdAudits.Count().Should().Be(updatedExaminations.Count());
        }
    }
}
