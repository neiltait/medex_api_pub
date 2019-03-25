using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;

namespace MedicalExaminer.Common.Services.Examination
{
    public class ExaminationRetrievalService : IAsyncQueryHandler<ExaminationRetrievalQuery, Models.Examination>
    {
        private readonly IConnectionSettings connectionSettings;
        private readonly IDatabaseAccess databaseAccess;

        public ExaminationRetrievalService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings)
        {
            this.databaseAccess = databaseAccess;
            this.connectionSettings = connectionSettings;
        }

        public Task<Models.Examination> Handle(ExaminationRetrievalQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var result = databaseAccess.GetItemAsync<Models.Examination>(
                connectionSettings,
                x => x.ExaminationId == param.ExaminationId);
            return result;
        }
    }
}