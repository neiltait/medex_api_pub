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

        [Fact]
        public void CheckParentIdsValid_ParentIdDoesNotExist()
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
            site1.Code = "Site1";
            site1.ParentId = "T2"; //does not exist!
            site1.Type = LocationType.Site;

            var locations = new List<Location>();
            locations.Add(national);
            locations.Add(region1);
            locations.Add(trust1);
            locations.Add(site1);

            var locationsChecker = new LocationsChecker(locations);

            //Act
            Exception ex = Assert.Throws<Exception>(() => locationsChecker.CheckParentIdsValid());

            //Assert
            Assert.Equal("Location Site1 does not have valid parent id", ex.Message);
        }

        [Fact]
        public void CheckParentIdsValid_NationalHasParentId()
        {
            ///Arrange
            var national = new Location();
            national.LocationId = "N1";
            national.ParentId = "X1"; //should be null!
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
            Exception ex = Assert.Throws<Exception>(() => locationsChecker.CheckParentIdsValid());

            //Assert
            Assert.Equal("National location should not have parent id", ex.Message);
        }

        [Fact]
        public void CheckAllLocationIdsAreUnique_AllAreUnique()
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
            site1.Code = "Site1";
            site1.ParentId = "T1";
            site1.Type = LocationType.Site;

            var locations = new List<Location>();
            locations.Add(national);
            locations.Add(region1);
            locations.Add(trust1);
            locations.Add(site1);

            var locationsChecker = new LocationsChecker(locations);

            //Act
            var result = locationsChecker.CheckAllLocationIdsAreUnique();

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void CheckAllLocationIdsAreUnique_DupolicateDetected()
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
            site1.Code = "Site1";
            site1.ParentId = "T1";
            site1.Type = LocationType.Site;

            var site2 = new Location();
            site2.LocationId = "S1"; //Duplicate!
            site2.Code = "Site2";
            site2.ParentId = "T1";
            site2.Type = LocationType.Site;

            var locations = new List<Location>();
            locations.Add(national);
            locations.Add(region1);
            locations.Add(trust1);
            locations.Add(site1);
            locations.Add(site2);

            var locationsChecker = new LocationsChecker(locations);

            //Act
            Exception ex = Assert.Throws<Exception>(() => locationsChecker.CheckAllLocationIdsAreUnique());

            //Assert
            Assert.Equal("Duplicate locationIds detected", ex.Message);
        }

        [Fact]
        public void CheckLocationIdsNotNull_NoNulls()
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
            site1.Code = "Site1";
            site1.ParentId = "T1";
            site1.Type = LocationType.Site;

            var locations = new List<Location>();
            locations.Add(national);
            locations.Add(region1);
            locations.Add(trust1);
            locations.Add(site1);

            var locationsChecker = new LocationsChecker(locations);

            //Act
            var result = locationsChecker.CheckAllLocationIdsAreUnique();

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void CheckLocationIdsNotNull_Nulls()
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
            site1.LocationId = null;
            site1.Code = "Site1";
            site1.ParentId = "T1";
            site1.Type = LocationType.Site;

            var locations = new List<Location>();
            locations.Add(national);
            locations.Add(region1);
            locations.Add(trust1);
            locations.Add(site1);

            var locationsChecker = new LocationsChecker(locations);

            //Act
            Exception ex = Assert.Throws<Exception>(() => locationsChecker.CheckLocationIdsNotNull());

            //Assert
            Assert.Equal("Locations exist with null for LocationId", ex.Message);
        }
    }
}
