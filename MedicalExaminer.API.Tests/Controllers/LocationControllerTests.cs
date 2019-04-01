using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Models.v1.Locations;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Controllers
{
    public class LocationControllerTests : ControllerTestsBase<LocationsController>
    {
        private Mock<IAsyncQueryHandler<LocationRetrievalByIdQuery, Location>> _locationRetrievalByIdMock;

        private Mock<IAsyncQueryHandler<LocationsRetrievalByQuery, IEnumerable<Location>>> _locationRetrievalMock;

        public LocationControllerTests()
        {
            _locationRetrievalByIdMock = new Mock<IAsyncQueryHandler<LocationRetrievalByIdQuery, Location>>();

            _locationRetrievalMock = new Mock<IAsyncQueryHandler<LocationsRetrievalByQuery, IEnumerable<Location>>>();

            Controller = new LocationsController(
                _locationRetrievalByIdMock.Object,
                _locationRetrievalMock.Object,
                LoggerMock.Object,
                Mapper);
        }

        [Fact]
        public void GetLocations_ShouldReturnLocations()
        {
            // Arrange
            var expectedName = "name";
            var expectedLocation1 = new Location
            {
                Name = expectedName,
            };
            var expectedLocations = new List<Location>
            {
                expectedLocation1,
            };
            _locationRetrievalMock
                .Setup(lr => lr.Handle(It.IsAny<LocationsRetrievalByQuery>()))
                .Returns(Task.FromResult((IEnumerable<Location>)expectedLocations));

            var request = new GetLocationsRequest
            {
                Name = expectedName,
            };

            // Act
            var response = Controller.GetLocations(request);

            // Assert
            var taskResult = response.Should().BeOfType<Task<ActionResult<GetLocationsResponse>>>().Subject;
            var okResult = taskResult.Result.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            var location = okResult.Value.Should().BeAssignableTo<GetLocationsResponse>().Subject;
            location.Locations.Count().Should().Be(1);
            location.Locations.First().Name.Should().Be(expectedName);
        }

        [Fact]
        public void GetLocation_When_Called_With_Invalid_Id_Returns_Expected_Type()
        {
            // Arrange
            var expectedLocation = "expectedLocation";
            _locationRetrievalByIdMock
                .Setup(lr => lr.Handle(It.IsAny<LocationRetrievalByIdQuery>()))
                .Throws(CreateDocumentClientExceptionForTesting());

            // Act
            var response = Controller.GetLocation(expectedLocation);

            // Assert
            var taskResult = response.Should().BeOfType<Task<ActionResult<GetLocationResponse>>>().Subject;
            taskResult.Result.Value.Should().BeNull();
            Assert.Equal(TaskStatus.RanToCompletion, taskResult.Status);
        }

        [Fact]
        public void GetLocation_When_Called_With_Valid_Id_Returns_Expected_Type()
        {
            // Arrange
            var expectedLocationId = "expectedLocationId";
            var expectedLocationName = "expectedLocationName";
            var expectedLocation = new Location()
            {
                LocationId = expectedLocationId,
                Name = expectedLocationName,
            };

            _locationRetrievalByIdMock
                .Setup(lr => lr.Handle(It.IsAny<LocationRetrievalByIdQuery>()))
                .Returns(Task.FromResult(expectedLocation));

            // Act
            var response = Controller.GetLocation(expectedLocationId);

            // Assert
            var taskResult = response.Should().BeOfType<Task<ActionResult<GetLocationResponse>>>().Subject;
            var okResult = taskResult.Result.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            var location = okResult.Value.Should().BeAssignableTo<GetLocationResponse>().Subject;
            Assert.Equal(TaskStatus.RanToCompletion, taskResult.Status);
            Assert.Equal(expectedLocationId, location.LocationId);
        }
    }
}