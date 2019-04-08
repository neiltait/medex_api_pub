using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using MedicalExaminer.Common.Extensions.Examination;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Xunit;

namespace MedicalExaminer.Common.Tests.Extensions.Examination
{
    /// <summary>
    /// Examination Extension Tests.
    /// </summary>
    public class ExaminationExtensionTests
    {
        /// <summary>
        /// Update Location Cache Updates No Locations When None Provided.
        /// </summary>
        [Fact]
        public void UpdateLocationCache_UpdatesNoLocations_WhenNoneProvided()
        {
            // Arrange
            var expectedNationalLocationId = "expectedNationalLocationId";
            var expectedRegionLocationId = "expectedRegionLocationId";
            var expectedTrustLocationId = "expectedTrustLocationId";
            var expectedSiteLocationId = "expectedSiteLocationId";
            var examination = new Models.Examination()
            {
                NationalLocationId = expectedNationalLocationId,
                RegionLocationId = expectedRegionLocationId,
                TrustLocationId = expectedTrustLocationId,
                SiteLocationId = expectedSiteLocationId,
            };

            var locations = Enumerable.Empty<Location>();

            // Act
            examination.UpdateLocationCache(locations);

            // Assert
            examination.NationalLocationId.Should().Be(expectedNationalLocationId);
            examination.RegionLocationId.Should().Be(expectedRegionLocationId);
            examination.TrustLocationId.Should().Be(expectedTrustLocationId);
            examination.SiteLocationId.Should().Be(expectedSiteLocationId);
        }

        /// <summary>
        /// Update Location Cache Updates All Locations When Provided.
        /// </summary>
        [Fact]
        public void UpdateLocationCache_UpdatesAllLocations_WhenProvided()
        {
            // Arrange
            var examination = new Models.Examination();
            var expectedNationalLocationId = "expectedNationalLocationId";
            var expectedRegionLocationId = "expectedRegionLocationId";
            var expectedTrustLocationId = "expectedTrustLocationId";
            var expectedSiteLocationId = "expectedSiteLocationId";
            var locations = new List<Location>()
            {
                new Location()
                {
                    LocationId = expectedNationalLocationId,
                    Type = LocationType.National,
                },
                new Location()
                {
                    LocationId = expectedRegionLocationId,
                    Type = LocationType.Region,
                },
                new Location()
                {
                    LocationId = expectedTrustLocationId,
                    Type = LocationType.Trust,
                },
                new Location()
                {
                    LocationId = expectedSiteLocationId,
                    Type = LocationType.Site,
                },
            };

            // Act
            examination.UpdateLocationCache(locations);

            // Assert
            examination.NationalLocationId.Should().Be(expectedNationalLocationId);
            examination.RegionLocationId.Should().Be(expectedRegionLocationId);
            examination.TrustLocationId.Should().Be(expectedTrustLocationId);
            examination.SiteLocationId.Should().Be(expectedSiteLocationId);
        }
    }
}
