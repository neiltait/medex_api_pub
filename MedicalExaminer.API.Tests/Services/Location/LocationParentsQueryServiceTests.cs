using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Services.Location;
using MedicalExaminer.Models.Enums;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Location
{
    public class LocationParentsQueryServiceTests : ServiceTestsBase<
        LocationParentsQuery,
        LocationConnectionSettings,
        IEnumerable<MedicalExaminer.Models.Location>,
        MedicalExaminer.Models.Location,
        LocationParentsQueryService>
    {
        [Fact]
        public async Task Handle_ReturnsAllParents_FromSite()
        {
            // Arrange
            var query = new LocationParentsQuery("site");

            // Act
            var results = (await Service.Handle(query)).ToList();

            // Assert
            results.Should().NotBeNull();
            results.Count.Should().Be(4);
            results.Count(l => l.Type == LocationType.National).Should().Be(1);
            results.Count(l => l.Type == LocationType.Region).Should().Be(1);
            results.Count(l => l.Type == LocationType.Trust).Should().Be(1);
            results.Count(l => l.Type == LocationType.Site).Should().Be(1);
        }

        [Fact]
        public async Task Handle_ReturnsAllParents_FromTrust()
        {
            // Arrange
            var query = new LocationParentsQuery("trust");

            // Act
            var results = (await Service.Handle(query)).ToList();

            // Assert
            results.Should().NotBeNull();
            results.Count.Should().Be(3);
            results.Count(l => l.Type == LocationType.National).Should().Be(1);
            results.Count(l => l.Type == LocationType.Region).Should().Be(1);
            results.Count(l => l.Type == LocationType.Trust).Should().Be(1);
        }

        [Fact]
        public async Task Handle_ReturnsAllParents_FromRegion()
        {
            // Arrange
            var query = new LocationParentsQuery("region");

            // Act
            var results = (await Service.Handle(query)).ToList();

            // Assert
            results.Should().NotBeNull();
            results.Count.Should().Be(2);
            results.Count(l => l.Type == LocationType.National).Should().Be(1);
            results.Count(l => l.Type == LocationType.Region).Should().Be(1);
        }

        [Fact]
        public async Task Handle_ReturnsAllParents_FromNational()
        {
            // Arrange
            var locationId = "national";
            var location = new Mock<MedicalExaminer.Models.Location>();
            var query = new LocationParentsQuery(locationId);

            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var dbAccess = new Mock<IDatabaseAccess>();

            dbAccess.Setup(db => db.GetItemByIdAsync<MedicalExaminer.Models.Location>(
                    connectionSettings.Object,
                    It.IsAny<string>()))
                .Returns(Task.FromResult(location.Object)).Verifiable();

            // Act
            var results = (await Service.Handle(query)).ToList();

            // Assert
            results.Should().NotBeNull();
            results.Count.Should().Be(1);
            results.Count(l => l.Type == LocationType.National).Should().Be(1);
        }

        [Fact]
        public async Task Handle_ReturnsEmpty_WhenInvalidLocation()
        {
            // Arrange
            var query = new LocationParentsQuery("invalid");

            // Act
            var results = (await Service.Handle(query)).ToList();

            // Assert
            results.Should().NotBeNull();
            results.Count.Should().Be(0);
        }

        protected override MedicalExaminer.Models.Location[] GetExamples()
        {
            return new[]
            {
                new MedicalExaminer.Models.Location()
                {
                    LocationId = "national",
                    ParentId = null,
                    Type = LocationType.National,
                },
                new MedicalExaminer.Models.Location()
                {
                    LocationId = "region",
                    ParentId = "national",
                    Type = LocationType.Region,
                },
                new MedicalExaminer.Models.Location()
                {
                    LocationId = "trust",
                    ParentId = "region",
                    Type = LocationType.Trust,
                },
                new MedicalExaminer.Models.Location()
                {
                    LocationId = "site",
                    ParentId = "trust",
                    Type = LocationType.Site,
                },
            };

        }
    }
}