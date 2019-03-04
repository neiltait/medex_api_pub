using System;
using System.Collections.Generic;
using System.Text;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.ReferenceDataLoader.Loaders
{
    /// <summary>
    /// Check that locations are in concsistent state
    /// </summary>
    public class LocationsChecker
    {
        private List<Location> _locations;

        public LocationsChecker(List<Location> locations)
        {
            _locations = locations;
        }

        /// <summary>
        /// Check that every location has a parentId links to another location, except the national loaction where parentD should be null 
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
                        throw new Exception("National location has parent id");
                    }
                        
                }
                else
                {
                    var parentId = location.ParentId;
                    var parent = _locations.FindAll(l => l.LocationId == parentId);
                    if (parent == null || parent.Count != 1)
                    {
                        throw new Exception($"Location {location.Code} does not have valid parent id");
                    }
                }                  
            }

            return true;
        }
    }
}
