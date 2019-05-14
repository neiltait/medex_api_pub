using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Castle.Core.Logging;
using FluentAssertions;
using MedicalExaminer.Common.Settings;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace MedicalExaminer.BackgroundServices.Tests
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Test method names are self documenting.")]
    public class BackgroundServicesExtensionsTests
    {
        [Fact]
        public void AddBackgroundServices_DoesNotSetup_IfSettingsIsNull()
        {
            // Arrange
            const BackgroundServicesSettings settings = null;
            var sut = new ServiceCollection();

            // Act
            sut.AddBackgroundServices(settings);

            // Assert
            var provider = sut.BuildServiceProvider();
            var scheduler = provider.GetService<IScheduler>();
            scheduler.Should().BeNull();

            var serviceConfiguration = provider.GetService<IScheduledServiceConfiguration>();
            var configuration = serviceConfiguration.Should().BeNull();
        }

        [Fact]
        public void AddBackgroundServices()
        {
            // Arrange
            var sampleRate = TimeSpan.FromSeconds(1);
            var timeAt = TimeSpan.FromSeconds(2);
            var settings = new BackgroundServicesSettings()
            {
                SampleRate = sampleRate,
                TimeToRunEachDay = timeAt,
            };
            var sut = new ServiceCollection();

            // Act
            sut.AddBackgroundServices(settings);

            // Assert
            var provider = sut.BuildServiceProvider();
            var scheduler = provider.GetService<IScheduler>();
            scheduler.Should().BeOfType<Scheduler>();

            var serviceConfiguration = provider.GetService<IScheduledServiceConfiguration>();
            var configuration = serviceConfiguration.Should().BeOfType<ScheduledServiceEveryDayAtSetTime>().Subject;

            configuration.SampleRate.Should().Be(sampleRate);
        }
    }
}
