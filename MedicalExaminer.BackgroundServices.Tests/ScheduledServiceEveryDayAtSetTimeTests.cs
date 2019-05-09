using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
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
        [Fact]
        public void CanExecute_ShouldReturnFalse_WhenTimeLessThanAtTime()
        {
            // Arrange
            var sut = new ScheduledServiceEveryDayAtSetTime(TimeSpan.Parse("02:00"));
            var now = DateTime.Parse("05/01/2019 01:00:00");
            var last = DateTime.Parse("01/01/2019 01:00:00");

            // Act
            var result = sut.CanExecute(now, last);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CanExecute_ShouldReturnTrue_WhenTimeGreaterThanAtTime()
        {
            // Arrange
            var sut = new ScheduledServiceEveryDayAtSetTime(TimeSpan.Parse("02:00"));
            var now = DateTime.Parse("05/01/2019 02:00:01");
            var last = DateTime.Parse("01/01/2019 01:00:00");

            // Act
            var result = sut.CanExecute(now, last);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void CanExecute_ShouldReturnFalse_WhenTimeGreaterThanAtTimeAndLastRunToday()
        {
            // Arrange
            var sut = new ScheduledServiceEveryDayAtSetTime(TimeSpan.Parse("02:00"));
            var now = DateTime.Parse("05/01/2019 02:00:01");
            var last = DateTime.Parse("05/01/2019 00:00:01");

            // Act
            var result = sut.CanExecute(now, last);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CanExecute_ShouldReturnTrue_WhenTimeGreaterThanAtTimeAndLastRunYesterday()
        {
            // Arrange
            var sut = new ScheduledServiceEveryDayAtSetTime(TimeSpan.Parse("02:00"));
            var now = DateTime.Parse("05/01/2019 02:00:01");
            var last = DateTime.Parse("04/01/2019 23:59:59");

            // Act
            var result = sut.CanExecute(now, last);

            // Assert
            result.Should().BeTrue();
        }
    }
}
