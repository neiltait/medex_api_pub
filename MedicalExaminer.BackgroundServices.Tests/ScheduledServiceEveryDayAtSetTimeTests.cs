using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Xunit;

namespace MedicalExaminer.BackgroundServices.Tests
{
    /// <summary>
    /// Scheduled Service Every Day At Set Time Tests.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Test method names are self documenting.")]
    public class ScheduledServiceEveryDayAtSetTimeTests
    {
        private readonly TimeSpan _sampleRate;

        public ScheduledServiceEveryDayAtSetTimeTests()
        {
            _sampleRate = TimeSpan.FromHours(1);
        }

        [Fact]
        public void CanExecute_ShouldReturnFalse_WhenTimeLessThanAtTimeAndLastRunBeforeToday()
        {
            // Arrange
            var sut = new ScheduledServiceEveryDayAtSetTime(TimeSpan.Parse("02:00"), _sampleRate);
            var now = DateTime.Parse("05/01/2019 01:00:00");
            var last = DateTime.Parse("01/01/2019 01:00:00");

            // Act
            var result = sut.CanExecute(now, last);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CanExecute_ShouldReturnTrue_WhenTimeGreaterOrEqualThanAtTimeAndTimeLastRunBeforeToday()
        {
            // Arrange
            var sut = new ScheduledServiceEveryDayAtSetTime(TimeSpan.Parse("02:00"), _sampleRate);
            var now = DateTime.Parse("05/01/2019 02:00:00");
            var last = DateTime.Parse("01/01/2019 01:00:00");

            // Act
            var result = sut.CanExecute(now, last);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void CanExecute_ShouldReturnFalse_WhenTimeGreaterOrEqualThanAtTimeAndLastRunToday()
        {
            // Arrange
            var sut = new ScheduledServiceEveryDayAtSetTime(TimeSpan.Parse("02:00"), _sampleRate);
            var now = DateTime.Parse("05/01/2019 02:00:00");
            var last = DateTime.Parse("05/01/2019 00:00:01");

            // Act
            var result = sut.CanExecute(now, last);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CanExecute_ShouldReturnTrue_WhenTimeLessThanAtTimeAndLastRunToday()
        {
            // Arrange
            var sut = new ScheduledServiceEveryDayAtSetTime(TimeSpan.Parse("02:00"), _sampleRate);
            var now = DateTime.Parse("05/01/2019 02:00:02");
            var last = DateTime.Parse("05/01/2019 02:00:01");

            // Act
            var result = sut.CanExecute(now, last);

            // Assert
            result.Should().BeFalse();
        }
    }
}
