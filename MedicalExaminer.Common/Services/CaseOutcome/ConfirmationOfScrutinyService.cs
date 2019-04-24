using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.CaseOutcome
{
    public class ConfirmationOfScrutinyService : QueryHandler<ExaminationRetrievalQuery, Models.Examination>
    {
        private IAsyncQueryHandler<ExaminationRetrievalQuery, Models.Examination> _examinationRetrievalService;


        public ConfirmationOfScrutinyService(
            IAsyncQueryHandler<ExaminationRetrievalQuery, Models.Examination> examinationRetrievalService,
        IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings)
        : base(databaseAccess, connectionSettings)
        {
            _examinationRetrievalService = examinationRetrievalService;
        }

        public async Task<Models.Examination> Handle(ExaminationRetrievalQuery param)
        {
            var examinationToUpdate = await
                _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(param.ExaminationId, param.User));

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
