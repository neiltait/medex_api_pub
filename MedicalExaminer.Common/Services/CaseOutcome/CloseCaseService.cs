using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.CaseOutcome;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.CaseOutcome
{
    public class CloseCaseService : IAsyncQueryHandler<CloseCaseQuery, string>
    {
        private readonly IAsyncQueryHandler<ExaminationRetrievalQuery, Models.Examination> _examinationRetrievalService;
        private readonly IConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;

        public CloseCaseService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings,
            IAsyncQueryHandler<ExaminationRetrievalQuery, Models.Examination> examinationRetrievalService)
        {
            _connectionSettings = connectionSettings;
            _databaseAccess = databaseAccess;
            _examinationRetrievalService = examinationRetrievalService;
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

            var examinationToUpdate = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(param.ExaminationId, param.User));
            examinationToUpdate.LastModifiedBy = param.User.UserId;
            examinationToUpdate.ModifiedAt = DateTime.Now;

            examinationToUpdate.Completed = true;

            examinationToUpdate = examinationToUpdate.UpdateCaseUrgencyScore();
            examinationToUpdate = examinationToUpdate.UpdateCaseStatus();

            var result = await _databaseAccess.UpdateItemAsync(_connectionSettings, examinationToUpdate);
            return result.ExaminationId;
        }
    }
}
