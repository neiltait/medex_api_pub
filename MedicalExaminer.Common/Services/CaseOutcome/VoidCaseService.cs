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
    public class VoidCaseService : IAsyncQueryHandler<VoidCaseQuery, Models.Examination>
    {
        private readonly IConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;
        private readonly UrgencySettings _urgencySettings;

        public VoidCaseService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings,
            IOptions<UrgencySettings> urgencySettings)
        {
            _connectionSettings = connectionSettings;
            _databaseAccess = databaseAccess;
            _urgencySettings = urgencySettings.Value;
        }

        public async Task<Models.Examination> Handle(VoidCaseQuery param)
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
            examinationToUpdate.VoidReason = param.VoidReason;
            examinationToUpdate.IsVoid = true;
            examinationToUpdate.VoidedDate = DateTime.Now;

            examinationToUpdate.CaseBreakdown.VoidEvent = new VoidEvent
            {
                VoidReason = examinationToUpdate.VoidReason,
                Created = DateTime.Now,
                EventId = Guid.NewGuid().ToString(),
                UserFullName = param.User.FullName(),
                GmcNumber = param.User.GmcNumber,
                UserId = param.User.UserId,
                UsersRole = param.User.Role()?.ToString()
            };

            examinationToUpdate =
                examinationToUpdate.UpdateCaseUrgencySort(_urgencySettings.DaysToPreCalculateUrgencySort);
            examinationToUpdate = examinationToUpdate.UpdateCaseStatus();

            var result = await _databaseAccess.UpdateItemAsync(_connectionSettings, examinationToUpdate);
            return result;
        }
    }
}
