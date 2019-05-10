using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace MedicalExaminer.BackgroundServices.Tests
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Test method names are self documenting.")]
    public class ScheduledServiceTests
    {
        private readonly DateTime _lastExecuted = DateTime.Parse("02/01/2019 00:00:01");

        private readonly Mock<IScheduledServiceConfiguration> _configurationMock;

        private readonly Mock<IScheduler> _schedulerMock;

        private readonly TestScheduledService _sut;

        public ScheduledServiceTests()
        {
            _configurationMock = new Mock<IScheduledServiceConfiguration>(MockBehavior.Strict);
            _schedulerMock = new Mock<IScheduler>(MockBehavior.Strict);

            _schedulerMock
                .SetupGet(s => s.LastExecuted)
                .Returns(_lastExecuted);

            _sut = new TestScheduledService(_configurationMock.Object, _schedulerMock.Object);
        }

        [Fact]
        public void Dispose_DoesNothing()
        {
            // Act
            _sut.Dispose();
        }

        [Fact]
        public void StopAsync_WhenNotStartedReturns()
        {
            // Act
            var result = _sut.StopAsync(CancellationToken.None);

            // Assert
            result.Status.Should().Be(TaskStatus.RanToCompletion);
        }

        [Fact]
        public async void StrartAndStopAsync_VerifyCallCount()
        {
            var expectedDateTime1 = DateTime.Parse("02/01/2019 00:01:00");
            var expectedDateTime2 = DateTime.Parse("02/01/2019 00:02:00");
            var expectedDateTime3 = DateTime.Parse("02/01/2019 00:03:00");

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            // Arrange
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

            var start = _sut.StartAsync(cancellationToken);

            // Act
            await _sut.StopAsync(CancellationToken.None);

            // Assert
            _sut.CountExecute.Should().Be(2);
            start.Status.Should().Be(TaskStatus.RanToCompletion);
        }

        private class TestScheduledService : ScheduledService
        {
            public TestScheduledService(IScheduledServiceConfiguration configuration, IScheduler scheduler)
                : base(configuration, scheduler)
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
    }
}
