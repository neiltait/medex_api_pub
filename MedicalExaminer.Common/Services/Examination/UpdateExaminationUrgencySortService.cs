using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Settings;
using MedicalExaminer.Models;
using Microsoft.Extensions.Options;

namespace MedicalExaminer.Common.Services.Examination
{
    /// <summary>
    /// Service update examination urgency sort.
    /// </summary>
    public class UpdateExaminationUrgencySortService : QueryHandler<UpdateExaminationUrgencySortQuery, bool>
    {
        private readonly UrgencySettings _urgencySettings;

        /// <summary>
        /// Initialise a new instance of <see cref="UpdateExaminationUrgencySortService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database access.</param>
        /// <param name="connectionSettings">Connection settings.</param>
        /// <param name="urgencySettings">Urgency settings.</param>
        public UpdateExaminationUrgencySortService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings,
            IOptions<UrgencySettings> urgencySettings)
            : base(databaseAccess, connectionSettings)
        {
            _urgencySettings = urgencySettings.Value;
        }

        /// <inheritdoc/>
        public override async Task<bool> Handle(UpdateExaminationUrgencySortQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var examinations = DatabaseAccess
                .GetItemsAsync<Models.Examination>(ConnectionSettings, e => e.CaseCompleted == false)
                .Result;

            foreach (var examination in examinations)
            {
                examination.UpdateCaseUrgencySort(_urgencySettings.DaysToPreCalculateUrgencySort);

                examination.LastModifiedBy = "UpdateExaminationUrgencySortService";
                examination.ModifiedAt = DateTimeOffset.UtcNow;

                await DatabaseAccess.UpdateItemAsync(ConnectionSettings, examination);
            }

            return true;
        }
    }
}
