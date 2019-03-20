using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Location;

namespace MedicalExaminer.Common.Services.Location
{
    public class LocationIdService : IAsyncQueryHandler<LocationRetrievalByIdQuery, Models.Location>
    {
        private readonly IConnectionSettings connectionSettings;
        private readonly IDatabaseAccess databaseAccess;

        public LocationIdService(IDatabaseAccess databaseAccess, ILocationConnectionSettings connectionSettings)
        {
            this.databaseAccess = databaseAccess;
            this.connectionSettings = connectionSettings;
        }

        public Task<Models.Location> Handle(LocationRetrievalByIdQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            return databaseAccess.GetItemAsync<Models.Location>(
                connectionSettings,
                location => location.LocationId == param.LocationId);
        }
    }
}