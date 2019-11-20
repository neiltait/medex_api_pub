using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Location;

namespace MedicalExaminer.Common.Services.Location
{
    /// <summary>
    /// Update Location Is ME Office Service.
    /// </summary>
    public class UpdateLocationIsMeOfficeService : QueryHandler<UpdateLocationIsMeOfficeQuery, Models.Location>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UpdateLocationIsMEOfficeService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database access.</param>
        /// <param name="connectionSettings">Connection settings.</param>
        /// <param name="urgencySettings">Urgency settings.</param>
        public UpdateLocationIsMeOfficeService(
            IDatabaseAccess databaseAccess,
            ILocationConnectionSettings connectionSettings)
            : base(databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override async Task<Models.Location> Handle(UpdateLocationIsMeOfficeQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var location = await DatabaseAccess.GetItemByIdAsync<Models.Location>(ConnectionSettings, param.LocationId);

            location.IsMeOffice = param.IsMeOffice;

            var result = await DatabaseAccess.UpdateItemAsync(ConnectionSettings, location);

            return result;
        }
    }
}
