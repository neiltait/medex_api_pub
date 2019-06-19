using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MedicalExaminer.API.Attributes;
using MedicalExaminer.Common;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Attributes
{
    public class ValidMedicalExaminerOfficeTests
    {
        [Fact]
        public void NoLocationIdSuppliedReturnsError()
        {
            // Arrange
            var locationId = string.Empty;
            var expectedResult = "The location Id must be supplied";

            var locationPersistence = new Mock<IAsyncQueryHandler<LocationRetrievalByIdQuery, Location>>();
            locationPersistence
                .Setup(persistence => persistence.Handle(It.Is<LocationRetrievalByIdQuery>(o => o.LocationId == string.Empty)))
                .Returns(Task.FromResult<Location>(null));

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(context => context.GetService(It.IsAny<Type>()))
                .Returns(locationPersistence.Object);
            var sut = new ValidLocation();

            // Act
            var result = sut.GetValidationResult(
                locationId,
                new ValidationContext(new object(), serviceProvider.Object, new Dictionary<object, object>()));

            // Assert
            Assert.Equal(expectedResult, result.ErrorMessage);
        }

        [Fact]
        public void NoLocationIsFoundReturnsError()
        {
            // Arrange
            var locationId = "bad location";
            var locationPersistence = new Mock<IAsyncQueryHandler<LocationRetrievalByIdQuery, Location>>();
            var expectedResult = "The location Id has not been found";
            locationPersistence
                .Setup(persistence => persistence.Handle(It.Is<LocationRetrievalByIdQuery>(o => o.LocationId == "bad location")))
                .Returns(Task.FromResult<Location>(null));

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(context => context.GetService(It.IsAny<Type>()))
                .Returns(locationPersistence.Object);
            var sut = new ValidLocation();

            // Act
            var result = sut.GetValidationResult(
                locationId,
                new ValidationContext(new object(), serviceProvider.Object, new Dictionary<object, object>()));

            Assert.Equal(expectedResult, result.ErrorMessage);
        }

        [Fact]
        public void NullLocationIdSuppliedReturnsError()
        {
            // Arrange
            object locationId = null;
            var expectedResult = "The location Id must be supplied";

            var locationPersistence = new Mock<IAsyncQueryHandler<LocationRetrievalByIdQuery, Location>>();
            locationPersistence
                .Setup(persistence => persistence.Handle(It.Is<LocationRetrievalByIdQuery>(o => o.LocationId == string.Empty)))
                .Returns(Task.FromResult<Location>(null));

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(context => context.GetService(It.IsAny<Type>()))
                .Returns(locationPersistence.Object);
            var sut = new ValidLocation();

            // Act
            var result = sut.GetValidationResult(locationId,
                new ValidationContext(new object(), serviceProvider.Object, new Dictionary<object, object>()));

            // Assert
            Assert.Equal(expectedResult, result.ErrorMessage);
        }

        [Fact]
        public void OneLocationIsFoundReturnsNoError()
        {
            // Arrange
            var locationResult = new Mock<Location>();
            var expectedResult = ValidationResult.Success;
            var locationId = "good location";
            var locationPersistence = new Mock<IAsyncQueryHandler<LocationRetrievalByIdQuery, Location>>();
            locationPersistence
                .Setup(persistence => persistence.Handle(It.Is<LocationRetrievalByIdQuery>( o => o.LocationId == "good location")))
                .Returns(Task.FromResult(locationResult.Object));

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(context => context.GetService(It.IsAny<Type>()))
                .Returns(locationPersistence.Object);
            var sut = new ValidLocation();

            // Act
            var result = sut.GetValidationResult(
                locationId,
                new ValidationContext(new object(), serviceProvider.Object, new Dictionary<object, object>()));

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}