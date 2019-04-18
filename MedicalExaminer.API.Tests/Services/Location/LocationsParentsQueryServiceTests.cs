using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Services.Location;
using MedicalExaminer.Models.Enums;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Location
{
    public class LocationsParentsQueryServiceTests : ServiceTestsBase<
        LocationsParentsQuery,
        LocationConnectionSettings,
        IDictionary<string,IEnumerable<MedicalExaminer.Models.Location>>,
        MedicalExaminer.Models.Location,
        LocationsParentsQueryService>
    {
        [Fact]
        public async Task Handle_ReturnsAllParents_FromMultipleSites()
        {
            // Arrange
            var query = new LocationsParentsQuery(new[] { "site1", "site2" });

            // Act
            var results = await Service.Handle(query);

            // Assert
            results.Should().NotBeNull();
            results.Count.Should().Be(2);

            results["site1"].Count().Should().Be(4);
            results["site1"].Count(l => l.Type == LocationType.National).Should().Be(1);
            results["site1"].Count(l => l.Type == LocationType.Region).Should().Be(1);
            results["site1"].Count(l => l.Type == LocationType.Trust).Should().Be(1);
            results["site1"].Count(l => l.Type == LocationType.Site).Should().Be(1);

            results["site2"].Count().Should().Be(4);
            results["site2"].Count(l => l.Type == LocationType.National).Should().Be(1);
            results["site2"].Count(l => l.Type == LocationType.Region).Should().Be(1);
            results["site2"].Count(l => l.Type == LocationType.Trust).Should().Be(1);
            results["site2"].Count(l => l.Type == LocationType.Site).Should().Be(1);
        }

        [Fact]
        public async Task Handle_ReturnsAllParents_FromSitesSharingPath()
        {
            // Arrange
            var query = new LocationsParentsQuery(new[] { "site1", "trust1" });

            // Act
            var results = await Service.Handle(query);

            // Assert
            results.Should().NotBeNull();
            results.Count.Should().Be(2);

            results["site1"].Count().Should().Be(4);
            results["site1"].Count(l => l.Type == LocationType.National).Should().Be(1);
            results["site1"].Count(l => l.Type == LocationType.Region).Should().Be(1);
            results["site1"].Count(l => l.Type == LocationType.Trust).Should().Be(1);
            results["site1"].Count(l => l.Type == LocationType.Site).Should().Be(1);

            results["trust1"].Count().Should().Be(3);
            results["trust1"].Count(l => l.Type == LocationType.National).Should().Be(1);
            results["trust1"].Count(l => l.Type == LocationType.Region).Should().Be(1);
            results["trust1"].Count(l => l.Type == LocationType.Trust).Should().Be(1);
        }

        [Fact]
        public async Task Handle_ReturnsAllParents_FromSitesWithSameLocation()
        {
            // Arrange
            var query = new LocationsParentsQuery(new[] { "site1", "site1" });

            // Act
            var results = await Service.Handle(query);

            // Assert
            results.Should().NotBeNull();
            results.Count.Should().Be(1);

            results["site1"].Count().Should().Be(4);
            results["site1"].Count(l => l.Type == LocationType.National).Should().Be(1);
            results["site1"].Count(l => l.Type == LocationType.Region).Should().Be(1);
            results["site1"].Count(l => l.Type == LocationType.Trust).Should().Be(1);
            results["site1"].Count(l => l.Type == LocationType.Site).Should().Be(1);
        }

        [Fact]
        public async Task Handle_ReturnsAllParents_FromSitesSharingEntirePath()
        {
            // Arrange
            var query = new LocationsParentsQuery(new[] { "site1", "trust1", "region1", "national1" });

            // Act
            var results = await Service.Handle(query);

            // Assert
            results.Should().NotBeNull();
            results.Count.Should().Be(4);

            results["site1"].Count().Should().Be(4);
            results["site1"].Count(l => l.Type == LocationType.National).Should().Be(1);
            results["site1"].Count(l => l.Type == LocationType.Region).Should().Be(1);
            results["site1"].Count(l => l.Type == LocationType.Trust).Should().Be(1);
            results["site1"].Count(l => l.Type == LocationType.Site).Should().Be(1);

            results["trust1"].Count().Should().Be(3);
            results["trust1"].Count(l => l.Type == LocationType.National).Should().Be(1);
            results["trust1"].Count(l => l.Type == LocationType.Region).Should().Be(1);
            results["trust1"].Count(l => l.Type == LocationType.Trust).Should().Be(1);

            results["region1"].Count().Should().Be(2);
            results["region1"].Count(l => l.Type == LocationType.National).Should().Be(1);
            results["region1"].Count(l => l.Type == LocationType.Region).Should().Be(1);

            results["national1"].Count().Should().Be(1);
            results["national1"].Count(l => l.Type == LocationType.National).Should().Be(1);
        }

        [Fact]
        public async Task Handle_ReturnsAllParents_WithoutSharing()
        {
            // Arrange
            var query = new LocationsParentsQuery(new[] { "site1", "trust2", "region3", "national4" });

            // Act
            var results = await Service.Handle(query);

            // Assert
            results.Should().NotBeNull();
            results.Count.Should().Be(4);

            results["site1"].Count().Should().Be(4);
            results["site1"].Count(l => l.Type == LocationType.National).Should().Be(1);
            results["site1"].Count(l => l.Type == LocationType.Region).Should().Be(1);
            results["site1"].Count(l => l.Type == LocationType.Trust).Should().Be(1);
            results["site1"].Count(l => l.Type == LocationType.Site).Should().Be(1);

            results["trust2"].Count().Should().Be(3);
            results["trust2"].Count(l => l.Type == LocationType.National).Should().Be(1);
            results["trust2"].Count(l => l.Type == LocationType.Region).Should().Be(1);
            results["trust2"].Count(l => l.Type == LocationType.Trust).Should().Be(1);

            results["region3"].Count().Should().Be(2);
            results["region3"].Count(l => l.Type == LocationType.National).Should().Be(1);
            results["region3"].Count(l => l.Type == LocationType.Region).Should().Be(1);

            results["national4"].Count().Should().Be(1);
            results["national4"].Count(l => l.Type == LocationType.National).Should().Be(1);
        }

        protected override MedicalExaminer.Models.Location[] GetExamples()
        {
            return Enumerable.Range(1, 4).SelectMany(i => new[]
            {
                new MedicalExaminer.Models.Location()
                {
                    LocationId = $"national{i}",
                    ParentId = null,
                    Type = LocationType.National,
                },
                new MedicalExaminer.Models.Location()
                {
                    LocationId = $"region{i}",
                    ParentId = $"national{i}",
                    Type = LocationType.Region,
                },
                new MedicalExaminer.Models.Location()
                {
                    LocationId = $"trust{i}",
                    ParentId = $"region{i}",
                    Type = LocationType.Trust,
                },
                new MedicalExaminer.Models.Location()
                {
                    LocationId = $"site{i}",
                    ParentId = $"trust{i}",
                    Type = LocationType.Site,
                },
            }).ToArray();
        }
    }
}