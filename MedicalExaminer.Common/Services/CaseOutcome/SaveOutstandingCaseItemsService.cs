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
        private readonly IAsyncQueryHandler<ExaminationRetrievalQuery, Models.Examination> _examinationRetrievalService;
        private readonly IConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;

        public SaveOutstandingCaseItemsService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings,
            IAsyncQueryHandler<ExaminationRetrievalQuery, Models.Examination> examinationRetrievalService)
        {
            _connectionSettings = connectionSettings;
            _databaseAccess = databaseAccess;
            _examinationRetrievalService = examinationRetrievalService;
        }

        public async Task<string> Handle(SaveOutstandingCaseItemsQuery param)
        {
            if (string.IsNullOrEmpty(param.Examination.ExaminationId))
            {
                throw new ArgumentNullException(nameof(param.Examination.ExaminationId));
            }

            if (param.User == null)
            {
                throw new ArgumentNullException(nameof(param.User));
            }

            var examinationToUpdate = param.Examination;
            examinationToUpdate.LastModifiedBy = param.User.UserId;
            examinationToUpdate.ModifiedAt = DateTime.Now;

            examinationToUpdate.OutstandingCaseItemsCompleted = true;

            examinationToUpdate = examinationToUpdate.UpdateCaseUrgencyScore();
            examinationToUpdate = examinationToUpdate.UpdateCaseStatus();

            if (examinationToUpdate.ScrutinyConfirmed)
            {
                var result = await _databaseAccess.UpdateItemAsync(_connectionSettings, examinationToUpdate);
                return result.ExaminationId;
            }
            else
            {
                return null; // for now
            }
        }
    }
}
