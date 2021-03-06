﻿using System;
using System.Linq.Expressions;
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
        public void LocationIdFoundReturnsResult()
        {
            // Arrange
            var locationId = "a";
            var location = new Mock<MedicalExaminer.Models.Location>();
            var connectionSettings = new Mock<ILocationConnectionSettings>();
            var query = new Mock<LocationRetrievalByIdQuery>(locationId);
            var dbAccess = new Mock<IDatabaseAccess>();
            dbAccess
                .Setup(db => db.GetItemByIdAsync<MedicalExaminer.Models.Location>(connectionSettings.Object,It.IsAny<string>()))
                .Returns(Task.FromResult(location.Object))
                .Verifiable();
            var sut = new LocationIdService(dbAccess.Object, connectionSettings.Object);

            // Act
            var result = sut.Handle(query.Object);

            // Assert
            dbAccess.Verify(
                db => db.GetItemByIdAsync<MedicalExaminer.Models.Location>(
                    connectionSettings.Object,
                    It.IsAny<string>()), Times.Once);

            Assert.Equal(location.Object, result.Result);
        }

        [Fact]
        public void LocationIdIsNullThrowsException()
        {
            // Arrange
            var connectionSettings = new Mock<ILocationConnectionSettings>();
            LocationRetrievalByIdQuery query = null;
            var dbAccess = new Mock<IDatabaseAccess>();
            var sut = new LocationIdService(dbAccess.Object, connectionSettings.Object);

            // Act
            Action act = () => sut.Handle(query).GetAwaiter().GetResult();
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void LocationIdNotFoundReturnsNull()
        {
            // Arrange
            var locationId = "a";
            var connectionSettings = new Mock<ILocationConnectionSettings>();
            var query = new Mock<LocationRetrievalByIdQuery>(locationId);
            var dbAccess = new Mock<IDatabaseAccess>();
            dbAccess
                .Setup(db => db.GetItemByIdAsync<MedicalExaminer.Models.Location>(connectionSettings.Object, It.IsAny<string>()))
                .Returns(Task.FromResult<MedicalExaminer.Models.Location>(null))
                .Verifiable();

            var sut = new LocationIdService(dbAccess.Object, connectionSettings.Object);
            var expected = default(MedicalExaminer.Models.Location);

            // Act
            var result = sut.Handle(query.Object);

            // Assert
            dbAccess.Verify(
                db => db.GetItemByIdAsync<MedicalExaminer.Models.Location>(
                    connectionSettings.Object,
                    It.IsAny<string>()), Times.Once);

            Assert.Equal(expected, result.Result);
        }
    }
}