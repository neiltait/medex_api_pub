using System;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Services.Location;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Location
{
    public class LocationIdServiceTests
    {
        [Fact]
        public void LocationIdNotFoundReturnsNull()
        {
            // Arrange
            var locationId = "a";
            var connectionSettings = new Mock<ILocationConnectionSettings>();
            var query = new Mock<LocationRetrivalByIdQuery>(locationId);
            var dbAccess = new Mock<IDatabaseAccess>();
            dbAccess.Setup(db => db.QuerySingleAsync<MedicalExaminer.Models.Location>(connectionSettings.Object, "a"))
                .Returns(Task.FromResult<MedicalExaminer.Models.Location>(null)).Verifiable();
            var sut = new LocationIdService(dbAccess.Object, connectionSettings.Object);
            var expected = default(MedicalExaminer.Models.Location);

            // Act
            var result = sut.Handle(query.Object);
            
            // Assert
            dbAccess.Verify(db => db.QuerySingleAsync<MedicalExaminer.Models.Location>(connectionSettings.Object, "a"), Times.Once);
            
            Assert.Equal(expected, result.Result);

        }

        [Fact]
        public void LocationIdIsNullThrowsException()
        {
            // Arrange
            var connectionSettings = new Mock<ILocationConnectionSettings>();
            LocationRetrivalByIdQuery query = null;
            var dbAccess = new Mock<IDatabaseAccess>();
            var sut = new LocationIdService(dbAccess.Object, connectionSettings.Object);

            // Act
            Action act = () => sut.Handle(query);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void LocationIdFoundReturnsResult()
        {
            // Arrange
            var locationId = "a";
            var location = new Mock<MedicalExaminer.Models.Location>();
            var connectionSettings = new Mock<ILocationConnectionSettings>();
            var query = new Mock<LocationRetrivalByIdQuery>(locationId);
            var dbAccess = new Mock<IDatabaseAccess>();
            dbAccess.Setup(db => db.QuerySingleAsync<MedicalExaminer.Models.Location>(connectionSettings.Object, "a"))
                .Returns(Task.FromResult(location.Object)).Verifiable();
            var sut = new LocationIdService(dbAccess.Object, connectionSettings.Object);
            var expected = location;

            // Act
            var result = sut.Handle(query.Object);

            // Assert
            dbAccess.Verify(db => db.QuerySingleAsync<MedicalExaminer.Models.Location>(connectionSettings.Object, "a"), Times.Once);
            Assert.Equal(location.Object, result.Result);
        }
    }
}
