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
        private IAsyncQueryHandler<ConfirmationOfScrutinyQuery, Models.Examination> _confirmationOfScrutinyService;

        public ConfirmationOfScrutinyService(
            IAsyncQueryHandler<ConfirmationOfScrutinyQuery, Models.Examination> confirmationOfScrutinyService,
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings)
        : base(databaseAccess, connectionSettings)
        {
            _confirmationOfScrutinyService = confirmationOfScrutinyService;
        }

        public override async Task<Models.Examination> Handle(ConfirmationOfScrutinyQuery param)
        {
            var examinationToUpdate = await
                _confirmationOfScrutinyService.Handle(new ConfirmationOfScrutinyQuery(param.ExaminationId, param.User));

            examinationToUpdate.ConfirmationOfScrutinyCompletedAt = DateTime.Now;
            examinationToUpdate.ConfirmationOfScrutinyCompletedBy = param.User.UserId;
            examinationToUpdate.ModifiedAt = DateTimeOffset.Now;
            examinationToUpdate.LastModifiedBy = param.User.UserId;

            examinationToUpdate.UpdateCaseStatus();
            examinationToUpdate.UpdateCaseUrgencyScore();

            return await UpdateItemAsync(examinationToUpdate);
        }
    }
}
