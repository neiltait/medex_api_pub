using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.CaseOutcome;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.CaseOutcome
{
    public class SaveOutstandingCaseItemsService : IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string>
    {
        private readonly IConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;

        public SaveOutstandingCaseItemsService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings)
        {
            _connectionSettings = connectionSettings;
            _databaseAccess = databaseAccess;
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
                    .GetItemAsync<Models.Examination>(
                        _connectionSettings,
                        examination => examination.ExaminationId == param.ExaminationId);

            examinationToUpdate.LastModifiedBy = param.User.UserId;
            examinationToUpdate.ModifiedAt = DateTime.Now;

            examinationToUpdate.CaseOutcome = param.CaseOutcome;
            examinationToUpdate.OutstandingCaseItemsCompleted = true;

            examinationToUpdate = examinationToUpdate.UpdateCaseUrgencyScore();
            examinationToUpdate = examinationToUpdate.UpdateCaseStatus();

            var result = await _databaseAccess.UpdateItemAsync(_connectionSettings, examinationToUpdate);
            return result.ExaminationId;
        }
    }
}
