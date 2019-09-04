using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.CaseOutcome;
using MedicalExaminer.Common.Settings;
using MedicalExaminer.Models;
using Microsoft.Extensions.Options;

namespace MedicalExaminer.Common.Services.CaseOutcome
{
    public class SaveOutstandingCaseItemsService : IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string>
    {
        private readonly IConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;
        private readonly UrgencySettings _urgencySettings;

        public SaveOutstandingCaseItemsService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings,
            IOptions<UrgencySettings> urgencySettings)
        {
            _connectionSettings = connectionSettings;
            _databaseAccess = databaseAccess;
            _urgencySettings = urgencySettings.Value;
        }

        public async Task<string> Handle(SaveOutstandingCaseItemsQuery param)
        {
            if (string.IsNullOrEmpty(param.CaseOutcome.ToString()))
            {
                throw new ArgumentNullException(nameof(param.CaseOutcome.ToString));
            }

            if (param.User == null)
            {
                throw new ArgumentNullException(nameof(param.User));
            }

            var examinationToUpdate = await
                _databaseAccess
                    .GetItemByIdAsync<Models.Examination>(
                        _connectionSettings,
                        param.ExaminationId);

            if (!examinationToUpdate.ScrutinyConfirmed)
            {
                return null;
            }

            examinationToUpdate.LastModifiedBy = param.User.UserId;
            examinationToUpdate.ModifiedAt = DateTime.Now;

            examinationToUpdate.CaseOutcome.MccdIssued = param.CaseOutcome.MccdIssued;
            examinationToUpdate.CaseOutcome.CremationFormStatus = param.CaseOutcome.CremationFormStatus;
            examinationToUpdate.CaseOutcome.GpNotifiedStatus = param.CaseOutcome.GpNotifiedStatus;

            examinationToUpdate.OutstandingCaseItemsCompleted = examinationToUpdate.CalculateOutstandingCaseOutcomesCompleted();
            examinationToUpdate = examinationToUpdate.UpdateCaseUrgencySort(_urgencySettings.DaysToPreCalculateUrgencySort);
            examinationToUpdate = examinationToUpdate.UpdateCaseStatus();

            var result = await _databaseAccess.UpdateItemAsync(_connectionSettings, examinationToUpdate);
            return result.ExaminationId;
        }
    }
}
