using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Location;

namespace MedicalExaminer.Common.Services.Location
{
    /// <summary>
    /// Location Id Service.
    /// </summary>
    /// <inheritdoc/>
    public class LocationIdService : QueryHandler<LocationRetrievalByIdQuery, Models.Location>
    {
        /// <summary>
        /// Initialise a new instance of the Location Id Service.
        /// </summary>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">Connection Settings.</param>
        public LocationIdService(IDatabaseAccess databaseAccess, ILocationConnectionSettings connectionSettings)
            : base(databaseAccess, connectionSettings)
        {

        }

        /// <inheritdoc/>
        public override Task<Models.Location> Handle(LocationRetrievalByIdQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            return GetItemAsync(location => location.LocationId == param.LocationId);
        }
    }
}