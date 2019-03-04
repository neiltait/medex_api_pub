using System;
using System.Collections.Generic;
using System.Text;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using MedicalExaminer.ReferenceDataLoader.Loaders;
using Xunit;

namespace MedicalExaminers.ReferenceDataLoader.Tests
{
    public class LocationsCheckerTests
    {
        [Fact]
        public void CheckParentIdsValid_AllAreValid()
        {
            ///Arrange
            var national = new Location();
            national.LocationId = "N1";
            national.ParentId = null;
            national.Type = LocationType.National;

            var region1 = new Location();
            region1.LocationId = "R1";
            region1.ParentId = "N1";
            region1.Type = LocationType.Region;

            var trust1 = new Location();
            trust1.LocationId = "T1";
            trust1.ParentId = "R1";
            trust1.Type = LocationType.Trust;

            var site1 = new Location();
            site1.LocationId = "S1";
            site1.ParentId = "T1";
            site1.Type = LocationType.Site;

            var locations = new List<Location>();
            locations.Add(national);
            locations.Add(region1);
            locations.Add(trust1);
            locations.Add(site1);

            var locationsChecker = new LocationsChecker(locations);


            //Act
            var result = locationsChecker.CheckParentIdsValid();

            //Assert
            Assert.True(result);
        }
    }
}
