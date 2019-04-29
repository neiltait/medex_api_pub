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
            if (string.IsNullOrEmpty(param.CaseOutcome.ToString()))
            {
                throw new ArgumentNullException(nameof(param.CaseOutcome.ToString));
            }

            if (param.User == null)
            {
                throw new ArgumentNullException(nameof(param.User));
            }

            var examinationToUpdate = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(param.ExaminationId, param.User));
            examinationToUpdate.LastModifiedBy = param.User.UserId;
            examinationToUpdate.ModifiedAt = DateTime.Now;

            examinationToUpdate.CaseOutcome = param.CaseOutcome;
            examinationToUpdate.OutstandingCaseItemsCompleted = true;

            examinationToUpdate = examinationToUpdate.UpdateCaseUrgencyScore();
            examinationToUpdate = examinationToUpdate.UpdateCaseStatus();

            examinationToUpdate.ScrutinyConfirmed = true; // just for testing purposes

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
