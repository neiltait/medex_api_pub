using System;
using System.Collections.Generic;
using System.Linq;
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
    public class LocationsQueryServiceTests : ServiceTestsBase<
        LocationsRetrievalByQuery,
        LocationConnectionSettings,
        IEnumerable<MedicalExaminer.Models.Location>,
        MedicalExaminer.Models.Location,
        LocationsQueryService>
    {
        [Fact]
        public async Task Handle_ReturnsAllResults_WhenNoFilterApplied()
        {
            //Arrange
            var query = new LocationsRetrievalByQuery(null, null);

            //Act
            var results = (await Service.Handle(query)).ToList();

            //Assert
            results.Should().NotBeNull();
            results.Count.Should().Be(50);
        }

        [Fact]
        public async Task Handle_ReturnsFiltered_WhenFilteredByName()
        {
            //Arrange
            var query = new LocationsRetrievalByQuery("Name2", null);

            //Act
            var results = (await Service.Handle(query)).ToList();

            //Assert
            results.Should().NotBeNull();
            results.Count.Should().Be(1);
        }

        [Fact]
        public async Task Handle_ReturnsFiltered_WhenFilteredByParentId()
        {
            //Arrange
            var query = new LocationsRetrievalByQuery(null, "Name1");

            //Act
            var results = (await Service.Handle(query)).ToList();

            //Assert
            results.Should().NotBeNull();
            results.Count.Should().Be(1);
        }

        [Fact]
        public async Task Handle_ReturnsFiltered_WhenFilteredByNameAndParentId()
        {
            //Arrange
            var query = new LocationsRetrievalByQuery("Name2", "Name1");

            //Act
            var results = (await Service.Handle(query)).ToList();

            //Assert
            results.Should().NotBeNull();
            results.Count.Should().Be(1);
        }

        [Fact]
        public async Task Handle_ReturnsFiltered_WhenFilteredByPermissedLocations()
        {
            // Arrange
            var permissedLocations = new[]
            {
                "Name2",
                "Name3",
            };

            var query = new LocationsRetrievalByQuery(null, null, permissedLocations);

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
                ParentId = i > start ? $"Name{(i-1)}" : null
            }).ToArray();
        }
    }
}