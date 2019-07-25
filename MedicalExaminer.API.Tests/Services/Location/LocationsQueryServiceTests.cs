using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Services.Location;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Location
{
    public class LocationsQueryServiceTests : ServiceTestsBase<
        LocationsRetrievalByQuery,
        LocationConnectionSettings,
        IEnumerable<MedicalExaminer.Models.Location>,
        MedicalExaminer.Models.Location,
        LocationsQueryService>
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Handle_ReturnsAllResults_WhenNoFilterApplied(bool forLookup)
        {
            // Arrange
            var query = new LocationsRetrievalByQuery(null, null, forLookup, false);

            // Act
            var results = (await Service.Handle(query)).ToList();

            // Assert
            results.Should().NotBeNull();
            results.Count.Should().Be(50);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Handle_ReturnsFiltered_WhenFilteredByName(bool forLookup)
        {
            // Arrange
            var query = new LocationsRetrievalByQuery("Name2", null, forLookup, false);

            // Act
            var results = (await Service.Handle(query)).ToList();

            // Assert
            results.Should().NotBeNull();
            results.Count.Should().Be(1);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Handle_ReturnsFiltered_WhenFilteredByParentId(bool forLookup)
        {
            // Arrange
            var query = new LocationsRetrievalByQuery(null, "Name1", forLookup, false);

            // Act
            var results = (await Service.Handle(query)).ToList();

            // Assert
            results.Should().NotBeNull();
            results.Count.Should().Be(1);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Handle_ReturnsFiltered_WhenFilteredByNameAndParentId(bool forLookup)
        {
            // Arrange
            var query = new LocationsRetrievalByQuery("Name2", "Name1", forLookup, false);

            // Act
            var results = (await Service.Handle(query)).ToList();

            // Assert
            results.Should().NotBeNull();
            results.Count.Should().Be(1);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Handle_ReturnsFiltered_WhenFilteredByPermissedLocations(bool forLookup)
        {
            // Arrange
            var permissedLocations = new[]
            {
                "Name2",
                "Name3",
            };

            var query = new LocationsRetrievalByQuery(null, null, forLookup, false, permissedLocations);

            // Act
            var results = (await Service.Handle(query)).ToList();

            // Assert
            results.Should().NotBeNull();
            results.Count.Should().Be(2);
            results.ElementAt(0).Name.Should().Be("Name2");
            results.ElementAt(1).Name.Should().Be("Name3");
        }

        protected override MedicalExaminer.Models.Location[] GetExamples()
        {
            const int start = 1;
            return Enumerable.Range(start, 50).Select(i => new MedicalExaminer.Models.Location
            {
                LocationId = $"Name{i}",
                Name = $"Name{i}",
                ParentId = i > start ? $"Name{(i-1)}" : null,
                IsMeOffice = false
            }).ToArray();
        }
    }
}