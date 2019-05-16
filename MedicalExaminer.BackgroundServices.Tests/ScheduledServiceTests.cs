using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Moq;
using Xunit;

namespace MedicalExaminer.BackgroundServices.Tests
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Test method names are self documenting.")]
    public class ScheduledServiceTests
    {
        private readonly DateTime _lastExecuted = DateTime.Parse("02/01/2019 00:00:01");

        private readonly Mock<ILogger<TestScheduledService>> _loggerMock;

        private readonly Mock<IScheduledServiceConfiguration> _configurationMock;

        private readonly Mock<IScheduler> _schedulerMock;

        public ScheduledServiceTests()
        {
            _loggerMock = new Mock<ILogger<TestScheduledService>>(MockBehavior.Strict);
            _configurationMock = new Mock<IScheduledServiceConfiguration>(MockBehavior.Strict);
            _schedulerMock = new Mock<IScheduler>(MockBehavior.Strict);

            _schedulerMock
                .SetupGet(s => s.LastExecuted)
                .Returns(_lastExecuted);
        }

        [Fact]
        public void Dispose_DoesNothing()
        {
            var sut = new TestScheduledService(
                _loggerMock.Object,
                _configurationMock.Object,
                _schedulerMock.Object);

            // Act
            sut.Dispose();
        }

        [Fact]
        public void StopAsync_WhenNotStartedReturns()
        {
            var sut = new TestScheduledService(
                _loggerMock.Object,
                _configurationMock.Object,
                _schedulerMock.Object);

            // Act
            var result = sut.StopAsync(CancellationToken.None);

            // Assert
            result.Status.Should().Be(TaskStatus.RanToCompletion);
        }

        [Fact]
        public async void StrartAndStopAsync_VerifyCallCount()
        {
            // Arrange
            var expectedDateTime1 = DateTime.Parse("02/01/2019 00:01:00");
            var expectedDateTime2 = DateTime.Parse("02/01/2019 00:02:00");
            var expectedDateTime3 = DateTime.Parse("02/01/2019 00:03:00");

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var sut = new TestScheduledService(
                _loggerMock.Object,
                _configurationMock.Object,
                _schedulerMock.Object);

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
                        return true;
                    }

                    if (dateTime == expectedDateTime2)
                    {
                        return true;
                    }

                    cancellationTokenSource.Cancel();
                    return false;
                });

            _schedulerMock.SetupSequence(s => s.UtcNow)
                .Returns(expectedDateTime1)
                .Returns(expectedDateTime2)
                .Returns(expectedDateTime3);

            var start = sut.StartAsync(cancellationToken);

            // Act
            await sut.StopAsync(CancellationToken.None);

            // Assert
            sut.CountExecute.Should().Be(2);
            start.Status.Should().Be(TaskStatus.RanToCompletion);
        }

        [Fact]
        public async void VerifyExceptionsRaisedDuringExecuteAreLogged()
        {
            // Arrange
            var expectedDateTime1 = DateTime.Parse("02/01/2019 00:01:00");
            var expectedDateTime2 = DateTime.Parse("02/01/2019 00:02:00");
            var expectedDateTime3 = DateTime.Parse("02/01/2019 00:03:00");

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var loggerMock = new Mock<ILogger<TestScheduledServiceThatThrowsException>>(MockBehavior.Strict);

            var sut = new TestScheduledServiceThatThrowsException(
                loggerMock.Object,
                _configurationMock.Object,
                _schedulerMock.Object);

            loggerMock.Setup(l => l.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.IsAny<FormattedLogValues>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()))
                .Verifiable();

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
                        return true;
                    }

                    if (dateTime == expectedDateTime2)
                    {
                        return true;
                    }

                    cancellationTokenSource.Cancel();
                    return false;
                });

            _schedulerMock.SetupSequence(s => s.UtcNow)
                .Returns(expectedDateTime1)
                .Returns(expectedDateTime2)
                .Returns(expectedDateTime3);

            var start = sut.StartAsync(cancellationToken);

            // Act
            await sut.StopAsync(CancellationToken.None);

            // Assert
            start.Status.Should().Be(TaskStatus.RanToCompletion);

            loggerMock.Verify(l => l.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.IsAny<FormattedLogValues>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()));
        }

        public class TestScheduledService : ScheduledService
        {
            public TestScheduledService(ILogger<TestScheduledService> logger, IScheduledServiceConfiguration configuration, IScheduler scheduler)
                : base(logger, configuration, scheduler)
            {
                CountExecute = 0;
            }

            public int CountExecute { get; private set; }

            protected override Task ExecuteAsync(CancellationToken cancellationToken)
            {
                CountExecute++;
                return Task.CompletedTask;
            }
        }

        public class TestScheduledServiceThatThrowsException : ScheduledService
        {
            public TestScheduledServiceThatThrowsException(ILogger<TestScheduledServiceThatThrowsException> logger, IScheduledServiceConfiguration configuration, IScheduler scheduler)
                : base(logger, configuration, scheduler)
            {
            }

            protected override Task ExecuteAsync(CancellationToken cancellationToken)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
