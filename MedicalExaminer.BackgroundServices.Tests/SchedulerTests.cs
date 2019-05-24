using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace MedicalExaminer.BackgroundServices.Tests
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Test method names are self documenting.")]
    public class SchedulerTests
    {
        [Fact]
        public void LastExecuted_ReturnsDateTimeMinValue()
        {
            // Arrange
            var lastExecuted = DateTime.MinValue;

            // Act
            var sut = new Scheduler();

            // Assert
            sut.LastExecuted.Should().Be(lastExecuted);
        }

        [Fact]
        public void UtcNow_ReturnsTimeAfterMin()
        {
            // Arrange
            var utcNow = DateTime.UtcNow;

            // Act
            var sut = new Scheduler();

            // Assert
            sut.UtcNow.Should().BeAfter(DateTime.MinValue);
        }

        [Fact]
        public void Delay_ReturnsTask()
        {
            // Arrange
            var sut = new Scheduler();

            // Act
            var result = sut.Delay(TimeSpan.FromMilliseconds(0), CancellationToken.None);

            // Assert
            result.Should().BeOfType<Task>();
        }

        [Fact]
        public void DelayInt_ReturnsTask()
        {
            // Arrange
            var sut = new Scheduler();

            // Act
            var result = sut.Delay(0, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Task>();
        }
    }
}
