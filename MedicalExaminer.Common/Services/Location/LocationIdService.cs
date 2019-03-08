using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Location;

namespace MedicalExaminer.Common.Services.Location
{
    public class LocationIdService : IAsyncQueryHandler<LocationRetrivalByIdQuery, Models.Location>
    {
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IConnectionSettings _connectionSettings;
        public LocationIdService(IDatabaseAccess databaseAccess, ILocationConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }
        public Task<Models.Location> Handle(LocationRetrivalByIdQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            try
            {
                return _databaseAccess.QuerySingleAsync<Models.Location>(_connectionSettings, param.Id);
            }
            catch (Exception e)
            {
                //_logger.Log("Failed to retrieve examination data", e);
                throw;
            }
        }
    }
}
