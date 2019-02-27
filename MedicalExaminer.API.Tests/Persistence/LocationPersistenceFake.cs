using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalExaminer.Common;
using MedicalExaminer.Models;


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

            national.Id = "1";
            national.Code = "National";
            national.Name = "Medical Examiners National";
            national.Parent = null;

            region1.Id = "2";
            region1.Code = "R1";
            region1.Name = "Region 1";
            region1.Parent = "National";

            region2.Id = "3";
            region2.Code = "R2";
            region2.Name = "Region 2";
            region2.Parent = "National";

            trust1.Id = "4";
            trust1.Code = "R0A";
            trust1.Name = "Manchester University Foundation Trust";
            trust1.Parent = "R1";

            trust2.Id = "5";
            trust2.Code = "RKK";
            trust1.Name = "Grimsby Foundation Trust";
            trust1.Parent = "R2";

            site1.Id = "6";
            site1.Code = "R0A01";
            site1.Name = "Old Mill Hospital";
            site1.Parent = "T1";

            site2.Id = "7";
            site2.Code = "R0A02";
            site2.Name = "St Agnes Hospital";
            site2.Parent = "T1";

            site3.Id = "8";
            site3.Code = "RKK01";
            site3.Name = "Leeds Road Hospital";
            site3.Parent = "T2";

            site4.Id = "9";
            site4.Code = "RKK02";
            site4.Name = "North Hospital";
            site4.Parent = "T2";

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
                if (location.Id == locationId)
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
    }
}
