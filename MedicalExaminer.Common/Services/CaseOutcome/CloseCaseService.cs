using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Extensions.MeUser;
using MedicalExaminer.Common.Queries.CaseOutcome;
using MedicalExaminer.Common.Settings;
using MedicalExaminer.Models;
using Microsoft.Extensions.Options;

namespace MedicalExaminer.Common.Services.CaseOutcome
{
    public class CloseCaseService : IAsyncQueryHandler<CloseCaseQuery, string>
    {
        private readonly IConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;
        private readonly UrgencySettings _urgencySettings;

        public CloseCaseService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings,
            IOptions<UrgencySettings> urgencySettings)
        {
            _connectionSettings = connectionSettings;
            _databaseAccess = databaseAccess;
            _urgencySettings = urgencySettings.Value;
        }

        public async Task<string> Handle(CloseCaseQuery param)
        {
            if (string.IsNullOrEmpty(param.ExaminationId))
            {
                throw new ArgumentNullException(nameof(param.ExaminationId));
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

            examinationToUpdate.LastModifiedBy = param.User.UserId;
            examinationToUpdate.ModifiedAt = DateTime.Now;
            examinationToUpdate.CaseCompleted = true;
            examinationToUpdate.CaseOutcome.CaseCompleted = true;

            examinationToUpdate.CaseBreakdown.CaseClosedEvent = new CaseClosedEvent()
            {
                CaseOutcome = examinationToUpdate.CaseOutcome.CaseOutcomeSummary.Value,
                Created = DateTime.Now,
                DateCaseClosed = DateTime.Now,
                EventId = Guid.NewGuid().ToString(),
                UserFullName = param.User.FullName(),
                UserId = param.User.UserId,
                UsersRole = param.User.Role()?.ToString()
            };

            examinationToUpdate = examinationToUpdate.UpdateCaseUrgencySort(_urgencySettings.DaysToPreCalculateUrgencySort);
            examinationToUpdate = examinationToUpdate.UpdateCaseStatus();

            var result = await _databaseAccess.UpdateItemAsync(_connectionSettings, examinationToUpdate);
            return result.ExaminationId;
        }
    }
}
