using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;

namespace MedicalExaminer.BackgroundServices.Tests
{
    /// <summary>
    /// Scheduled Service Every Day At Set Time Tests.
    /// </summary>
    public class ScheduledServiceEveryDayAtSetTimeTests
    {
        /// <summary>
        /// Test.
        /// </summary>
        [Fact]
        public void Test()
        {
            // Arrange
            var sut = new ScheduledServiceEveryDayAtSetTime(TimeSpan.Parse("02:00"));
            var now = DateTime.Parse("05/01/2019 01:00:00");
            var last = DateTime.Parse("04/01/2019 01:00:00");

            // Act
            var result = sut.CanExecute(now, last);

            // Assert
            result.Should().BeTrue();
        }
    }
}
