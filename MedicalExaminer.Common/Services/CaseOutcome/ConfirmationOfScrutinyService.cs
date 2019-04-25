using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.CaseOutcome;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.CaseOutcome
{
    public class ConfirmationOfScrutinyService : QueryHandler<ConfirmationOfScrutinyQuery, Models.Examination>
    {
        IAsyncQueryHandler<ExaminationRetrievalQuery, Models.Examination> _examinationService;

        public ConfirmationOfScrutinyService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings,
            IAsyncQueryHandler<ExaminationRetrievalQuery, Models.Examination> examinationService)
        : base(databaseAccess, connectionSettings)
        {
            _examinationService = examinationService;
        }

        public override async Task<Models.Examination> Handle(ConfirmationOfScrutinyQuery param)
        {
            var examinationToUpdate = await
                _examinationService.Handle(new ExaminationRetrievalQuery(param.ExaminationId, param.User));

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
