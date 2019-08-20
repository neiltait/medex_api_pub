using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.CaseBreakdown;
using MedicalExaminer.Common.Settings;
using MedicalExaminer.Models;
using Microsoft.Extensions.Options;

namespace MedicalExaminer.Common.Services.Examination
{
    public class CreateEventService : IAsyncQueryHandler<CreateEventQuery, EventCreationResult>
    {
        private readonly IConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;
        private readonly UrgencySettings _urgencySettings;

        public CreateEventService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings,
            IOptions<UrgencySettings> urgencySettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
            _urgencySettings = urgencySettings.Value;
        }

        public async Task<EventCreationResult> Handle(CreateEventQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var examinationToUpdate = await
                            _databaseAccess
                                .GetItemByIdAsync<Models.Examination>(
                                    _connectionSettings,
                                    param.CaseId);

            examinationToUpdate = examinationToUpdate
                                            .AddEvent(param.Event)
                                            .UpdateCaseUrgencySort(_urgencySettings.DaysToPreCalculateUrgencySort)
                                            .UpdateCaseStatus();

            examinationToUpdate.LastModifiedBy = param.Event.UserId;
            examinationToUpdate.ModifiedAt = DateTime.Now;
            var result = await _databaseAccess.UpdateItemAsync(_connectionSettings, examinationToUpdate);
            return new EventCreationResult(param.Event.EventId, result);
        }
    }
}