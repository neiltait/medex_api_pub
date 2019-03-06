using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MedicalExaminer.API.Attributes;
using MedicalExaminer.Common;
using MedicalExaminer.Models;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Attributes
{
    public class ValidMedicalExaminerOfficeTests
    {
        [Fact]
        public void NoLocationIsFoundReturnsError()
        {
            // Arrange
            var locationId = "bad location";
            var locationPersistence = new Mock<ILocationPersistence>();
            var expectedResult = "The location id has not been found";
            locationPersistence.Setup(persistence =>
                persistence.GetLocationAsync("bad location")).Returns(Task.FromResult<Location>(null));

            
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(context => context.GetService(It.IsAny<Type>()))
                .Returns(locationPersistence.Object);
            var sut = new ValidMedicalExaminerOffice();

            // Act
            var result = sut.GetValidationResult(locationId, new ValidationContext(new object(), serviceProvider.Object, new Dictionary<object, object>()));
            
            Assert.Equal(expectedResult, result.ErrorMessage);
        }

        [Fact]
        public async void NoLocationIdSuppliedReturnsError()
        {
            // Arrange
            var locationId = string.Empty;
            var expectedResult = "The location id must be supplied";

            var locationPersistence = new Mock<ILocationPersistence>();
            locationPersistence.Setup(persistence =>
                persistence.GetLocationAsync(string.Empty)).Returns(Task.FromResult<Location>(null));

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(context => context.GetService(It.IsAny<Type>()))
                .Returns(locationPersistence.Object);
            var sut = new ValidMedicalExaminerOffice();

            // Act
            var result = sut.GetValidationResult(locationId, new ValidationContext(new object(), serviceProvider.Object, new Dictionary<object, object>()));
            
            // Assert
            Assert.Equal(expectedResult, result.ErrorMessage);
        }

        [Fact]
        public async void NullLocationIdSuppliedReturnsError()
        {
            // Arrange
            object locationId = null;
            var expectedResult = "The location id must be supplied";

            var locationPersistence = new Mock<ILocationPersistence>();
            locationPersistence.Setup(persistence =>
                persistence.GetLocationAsync(string.Empty)).Returns(Task.FromResult<Location>(null));

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(context => context.GetService(It.IsAny<Type>()))
                .Returns(locationPersistence.Object);
            var sut = new ValidMedicalExaminerOffice();

            // Act
            var result = sut.GetValidationResult(locationId, new ValidationContext(new object(), serviceProvider.Object, new Dictionary<object, object>()));

            // Assert
            Assert.Equal(expectedResult, result.ErrorMessage);
        }

        [Fact]
        public async void OneLocationIsFoundReturnsNoError()
        {
            // Arrange
            var locationResult = new Mock<Location>();
            var expectedResult = ValidationResult.Success;
            var locationId = "good location";
            var locationPersistence = new Mock<ILocationPersistence>();
            locationPersistence.Setup(persistence =>
                persistence.GetLocationAsync("good location")).Returns(Task.FromResult<Location>(locationResult.Object));

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(context => context.GetService(It.IsAny<Type>()))
                .Returns(locationPersistence.Object);
            var sut = new ValidMedicalExaminerOffice();
            // Act
            var result = sut.GetValidationResult(locationId, new ValidationContext(new object(), serviceProvider.Object, new Dictionary<object, object>()));
            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
