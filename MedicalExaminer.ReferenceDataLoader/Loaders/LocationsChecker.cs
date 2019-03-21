namespace MedicalExaminer.ReferenceDataLoader.Loaders
{
    using System;
    using System.Collections.Generic;
    using Models;
    using Models.Enums;

    /// <summary>
    ///     Check that locations are in concsistent state
    /// </summary>
    public class LocationsChecker
    {
        private readonly List<Location> _locations;

        public LocationsChecker(List<Location> locations)
        {
            _locations = locations;
        }

        /// <summary>
        ///     Run all check methods
        /// </summary>
        /// <returns>Boolean representing all checks have passed</returns>
        public bool RunAllChecks()
        {
            return CheckLocationIdsNotNull() && CheckAllLocationIdsAreUnique() && CheckParentIdsValid();
        }

        /// <summary>
        ///     Check that every location has a parentId links to another location, except the national loaction where parentD
        ///     should be null
        /// </summary>
        /// <returns>bool</returns>
        public bool CheckParentIdsValid()
        {
            foreach (var location in _locations)
            {
                if (location.Type == LocationType.National)
                {
                    if (location.ParentId != null)
                    {
                        throw new Exception("National location should not have parent id");
                    }
                }
                else
                {
                    var parentId = location.ParentId;
                    var parent = _locations.FindAll(l => l.LocationId == parentId);
                    if (parent.Count != 1)
                    {
                        throw new Exception($"Location {location.Code} does not have valid parent id");
                    }
                }
            }

            return true;
        }

        /// <summary>
        ///     Check LocationId is unique for each location
        /// </summary>
        /// <returns>Boolean Check all location IDs are unique</returns>
        public bool CheckAllLocationIdsAreUnique()
        {
            var locationIdsOnly = new HashSet<string>();
            _locations.ForEach(l => locationIdsOnly.Add(l.LocationId));

            if (locationIdsOnly.Count != _locations.Count)
            {
                throw new Exception("Duplicate locationIds detected");
            }

            return true;
        }

        /// <summary>
        ///     Check that no locationId value is null
        /// </summary>
        /// <returns>Boolean Location IDs are not null</returns>
        public bool CheckLocationIdsNotNull()
        {
            if (_locations.FindAll(l => l.LocationId == null).Count > 0)
            {
                throw new Exception("Locations exist with null for LocationId");
            }

            return true;
        }
    }
}