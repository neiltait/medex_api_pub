using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalExaminer.Common;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Tests.Persistence
{
    public class LocationPersistenceFake : ILocationPersistence
    {
        private readonly List<Location> _locations;

        public LocationPersistenceFake()
        {
            _locations = new List<Location>();

            var national = new Location();
            var region1 = new Location();
            var region2 = new Location();
            var trust1 = new Location();
            var trust2 = new Location();
            var site1 = new Location();
            var site2 = new Location();
            var site3 = new Location();
            var site4 = new Location();

            national.LocationId = "1";
            national.Code = "National";
            national.Name = "Medical Examiners National";
            national.ParentId = null;
            national.Type = LocationType.National;

            region1.LocationId = "2";
            region1.Code = "R1";
            region1.Name = "Region 1";
            region1.ParentId = "1";
            region1.Type = LocationType.Region;

            region2.LocationId = "3";
            region2.Code = "R2";
            region2.Name = "Region 2";
            region2.ParentId = "1";
            region2.Type = LocationType.Region;

            trust1.LocationId = "4";
            trust1.Code = "R0A";
            trust1.Name = "Manchester University Foundation Trust";
            trust1.ParentId = "2";
            trust1.Type = LocationType.Trust;

            trust2.LocationId = "5";
            trust2.Code = "RKK";
            trust2.Name = "Grimsby Foundation Trust";
            trust2.ParentId = "3";
            trust2.Type = LocationType.Trust;

            site1.LocationId = "6";
            site1.Code = "R0A01";
            site1.Name = "Old Mill Hospital";
            site1.ParentId = "4";
            site1.Type = LocationType.Site;

            site2.LocationId = "7";
            site2.Code = "R0A02";
            site2.Name = "St Agnes Hospital";
            site2.ParentId = "4";
            site2.Type = LocationType.Site;

            site3.LocationId = "8";
            site3.Code = "RKK01";
            site3.Name = "Leeds Road Hospital";
            site3.ParentId = "5";
            site3.Type = LocationType.Site;

            site4.LocationId = "9";
            site4.Code = "RKK02";
            site4.Name = "North Hospital";
            site4.ParentId = "6";
            site4.Type = LocationType.Site;

            _locations.Add(national);
            _locations.Add(region1);
            _locations.Add(region2);
            _locations.Add(trust1);
            _locations.Add(trust2);
            _locations.Add(site1);
            _locations.Add(site2);
            _locations.Add(site3);
            _locations.Add(site4);
        }

        public async Task<Location> GetLocationAsync(string locationId)
        {
            foreach (var location in _locations)
            {
                if (location.LocationId == locationId)
                {
                    return await Task.FromResult(location);
                }
            }

            throw new ArgumentException("Invalid Argument");
        }

        public async Task<IEnumerable<Location>> GetLocationsAsync()
        {
            return await Task.FromResult(_locations);
        }

        public async Task<IEnumerable<Location>> GetLocationsByNameAsync(string locationName)
        {
            return await Task.FromResult(_locations.FindAll(location =>
                location.Name.ToUpper().Contains(locationName.ToUpper())));
        }

        /// <summary>
        ///     Gets either a list of all locations minus national or empty list depending if parentID matches locationId of
        ///     national location in test data
        /// </summary>
        /// <param name="parentId">LocationID of location whose children to return</param>
        /// <returns>Either full or empty list</returns>
        public async Task<IEnumerable<Location>> GetLocationsByParentIdAsync(string parentId)
        {
            var results = new List<Location>();
            var parentLocation = _locations.Find(l => l.LocationId == parentId);

            if (parentLocation != null && parentLocation.Type == LocationType.National)
            {
                results.AddRange(_locations.FindAll(l => l.Type != LocationType.National));
            }

            return await Task.FromResult(results);
        }
    }
}