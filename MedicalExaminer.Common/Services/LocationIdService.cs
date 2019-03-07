using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Queries;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services
{
    public class LocationIdService : IAsyncQueryHandler<LocationRetrivalByIdQuery, Location>
    {
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IConnectionSettings _connectionSettings;
        public LocationIdService(IDatabaseAccess databaseAccess, ILocationConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }
        public Task<Location> Handle(LocationRetrivalByIdQuery param)
        {
            using (var conn = _databaseAccess.CreateClient(_connectionSettings))
            {
                try
                {
                    return _databaseAccess.QuerySingleAsync<Location>(_connectionSettings, param.Id);

                }
                catch (Exception e)
                {
                    //_logger.Log("Failed to retrieve examination data", e);
                    throw;
                }
            }
        }
    }
}
