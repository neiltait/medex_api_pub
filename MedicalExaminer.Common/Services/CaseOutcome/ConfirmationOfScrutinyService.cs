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
        public ConfirmationOfScrutinyService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings)
            : base(databaseAccess, connectionSettings)
        {
        }

        public override async Task<Models.Examination> Handle(ConfirmationOfScrutinyQuery param)
        {
            var examinationToUpdate = await
                DatabaseAccess
                    .GetItemByIdAsync<Models.Examination>(
                        ConnectionSettings,
                        param.ExaminationId);

            examinationToUpdate.ConfirmationOfScrutinyCompletedAt = DateTime.Now;
            examinationToUpdate.ConfirmationOfScrutinyCompletedBy = param.User.UserId;
            examinationToUpdate.ModifiedAt = DateTimeOffset.Now;
            examinationToUpdate.LastModifiedBy = param.User.UserId;
            examinationToUpdate.CaseOutcome.ScrutinyConfirmedOn = DateTime.Now;
            examinationToUpdate.ScrutinyConfirmed = true;

            examinationToUpdate.UpdateCaseStatus();
            examinationToUpdate.UpdateCaseUrgencyScore();

            var result = await DatabaseAccess.UpdateItemAsync(ConnectionSettings, examinationToUpdate);
            return result;
        }
    }
}
