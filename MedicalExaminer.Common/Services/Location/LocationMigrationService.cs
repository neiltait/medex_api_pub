using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Location;

namespace MedicalExaminer.Common.Services.Location
{
    public class LocationMigrationService : QueryHandler<LocationMigrationQuery, bool>
    {
        public LocationMigrationService(IDatabaseAccess databaseAccess, ILocationConnectionSettings connectionSettings)
                : base(databaseAccess, connectionSettings)
        {

        }

        public override Task<bool> Handle(LocationMigrationQuery param)
        {
            var locations = DatabaseAccess.GetItemsAsync<Models.Location>(ConnectionSettings, x => true).Result;

            var migratedLocations = locations.Select(x => Mapper.Map<Models.Location>(x));

            foreach(var location in migratedLocations)
            {
                DatabaseAccess.UpdateItemAsync(ConnectionSettings, location);
            }
            return Task.FromResult(true);
        }
    }
}
