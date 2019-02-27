using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Tests.Persistence;
using MedicalExaminer.Common;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Models.V1.Locations;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace MedicalExaminer.API.Tests.Controllers
{
    public class LocationControllerTests : ControllerTestsBase<LocationsController>
    {
        public LocationControllerTests()
        {
            // Arrange
            ILocationPersistence locationPersistence = new LocationPersistenceFake();
            var mockLogger = new MELoggerMocker();
            Controller = new LocationsController(locationPersistence, mockLogger, Mapper);
        }

        [Fact]
        public void GetLocation_When_Called_With_Valid_Id_Returns_Expected_Type()
        {
            // Act
            var response = Controller.GetLocation("5");

            // Assert
            var taskResult = response.Should().BeOfType<Task<ActionResult<GetLocationResponse>>>().Subject;
            var okResult = taskResult.Result.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            var location = okResult.Value.Should().BeAssignableTo<GetLocationResponse>().Subject;
            Assert.Equal("5", location.Id);
        }

        [Fact]
        public void GetLocations_When_Called_Returns_Expected_Type()
        {
            // Act
            var response = Controller.GetLocations();

            // Assert
            var taskResult = response.Should().BeOfType<Task<ActionResult<GetLocationsResponse>>>().Subject;
            var okResult = taskResult.Result.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            var locationResponse = okResult.Value.Should().BeAssignableTo<GetLocationsResponse>().Subject;
            var locations = locationResponse.Locations;
            Assert.Equal(9, locations.Count());
        }
    }
}
