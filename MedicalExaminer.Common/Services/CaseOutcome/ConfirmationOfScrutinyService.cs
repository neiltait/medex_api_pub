using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.CaseOutcome;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.CaseOutcome
{
    public class ConfirmationOfScrutinyService : QueryHandler<ConfirmationOfScrutinyQuery, Models.Examination>
    {
        private readonly IConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;

        public ConfirmationOfScrutinyService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings)
        : base(databaseAccess, connectionSettings)
        {
            _connectionSettings = connectionSettings;
            _databaseAccess = databaseAccess;
        }

        public override async Task<Models.Examination> Handle(ConfirmationOfScrutinyQuery param)
        {
            var examinationToUpdate = await
                _databaseAccess
                    .GetItemAsync<Models.Examination>(
                        _connectionSettings,
                        examination => examination.ExaminationId == param.ExaminationId);

            examinationToUpdate.ConfirmationOfScrutinyCompletedAt = DateTime.Now;
            examinationToUpdate.ConfirmationOfScrutinyCompletedBy = param.User.UserId;
            examinationToUpdate.ModifiedAt = DateTimeOffset.Now;
            examinationToUpdate.LastModifiedBy = param.User.UserId;

            // todo: Set Confirmation of scrutiny to true (after merging Feature/coroner referral branch)

            examinationToUpdate.UpdateCaseStatus();
            examinationToUpdate.UpdateCaseUrgencyScore();

            var result = await _databaseAccess.UpdateItemAsync(_connectionSettings, examinationToUpdate);
            return result;
        }
    }
}
