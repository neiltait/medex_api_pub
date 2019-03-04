using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.API.Models.Validators;
using MedicalExaminer.Common;
using MedicalExaminer.Models;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Validators
{
    public class LocationIdValidatorTests
    {
        [Fact]
        public async void NoLocationIsFoundReturnsError()
        {
            // Arrange
            var locationId = new Mock<LocationIdString>("bad location");
            var locationPersistence = new Mock<ILocationPersistence>();
            locationPersistence.Setup(persistence =>
                persistence.GetLocationAsync("bad location")).Returns(Task.FromResult<Location>(null));

            var sut = new LocationIdValidator(locationPersistence.Object);
            // Act
            var result = await sut.ValidateAsync(locationId.Object);
            // Assert
            result.Count().Should().Be(1);
        }

        [Fact]
        public async void NoLocationIdSuppliedReturnsError()
        {
            // Arrange
            var locationId = new Mock<LocationIdString>(string.Empty);
            var locationPersistence = new Mock<ILocationPersistence>();
            locationPersistence.Setup(persistence =>
                persistence.GetLocationAsync(string.Empty)).Returns(Task.FromResult<Location>(null));

            var sut = new LocationIdValidator(locationPersistence.Object);
            // Act
            var result = await sut.ValidateAsync(locationId.Object);
            // Assert
            result.Count().Should().Be(1);
        }

        [Fact]
        public async void OneLocationIsFoundReturnsNoError()
        {
            // Arrange
            var locationResult = new Mock<Location>();
            var locationId = new Mock<LocationIdString>("good location");
            var locationPersistence = new Mock<ILocationPersistence>();
            locationPersistence.Setup(persistence =>
                persistence.GetLocationAsync("good location")).Returns(Task.FromResult<Location>(locationResult.Object));

            var sut = new LocationIdValidator(locationPersistence.Object);
            // Act
            var result = await sut.ValidateAsync(locationId.Object);
            // Assert
            result.Count().Should().Be(0);
        }
    }
}
