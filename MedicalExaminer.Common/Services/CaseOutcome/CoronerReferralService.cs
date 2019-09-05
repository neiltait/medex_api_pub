using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.CaseOutcome;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Settings;
using MedicalExaminer.Models;
using Microsoft.Extensions.Options;

namespace MedicalExaminer.Common.Services.CaseOutcome
{
    public class CoronerReferralService : IAsyncQueryHandler<CoronerReferralQuery, string>
    {
        private readonly IConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;
        private readonly UrgencySettings _urgencySettings;

        public CoronerReferralService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings,
            IOptions<UrgencySettings> urgencySettings)
        {
            _connectionSettings = connectionSettings;
            _databaseAccess = databaseAccess;
            _urgencySettings = urgencySettings.Value;
        }

        public async Task<string> Handle(CoronerReferralQuery param)
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
            examinationToUpdate.CoronerReferralSent = true;
            examinationToUpdate.CaseOutcome.CoronerReferralSent = true;

            examinationToUpdate = examinationToUpdate.UpdateCaseUrgencySort(_urgencySettings.DaysToPreCalculateUrgencySort);
            examinationToUpdate = examinationToUpdate.UpdateCaseStatus();

            var result = await _databaseAccess.UpdateItemAsync(_connectionSettings, examinationToUpdate);
            return result.ExaminationId;
        }
    }
}
