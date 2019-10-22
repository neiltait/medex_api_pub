using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Services.Examination;
using MedicalExaminer.Common.Settings;
using MedicalExaminer.Models;
using Microsoft.Extensions.Options;

namespace MedicalExaminer.Common.Services.Location
{
    /// <summary>
    /// Update Location Is ME Office Service.
    /// </summary>
    public class UpdateLocationIsMeOfficeService : QueryHandler<UpdateLocationIsMeOfficeQuery, bool>
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
        public override async Task<bool> Handle(UpdateLocationIsMeOfficeQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var location = await DatabaseAccess.GetItemByIdAsync<Models.Location>(ConnectionSettings, param.LocationId);

            location.IsMeOffice = param.IsMeOffice;

            await DatabaseAccess.UpdateItemAsync(ConnectionSettings, location);

            return true;
        }
    }
}
