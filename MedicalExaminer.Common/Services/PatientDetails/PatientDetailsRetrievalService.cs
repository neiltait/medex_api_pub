using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.PatientDetails;

namespace MedicalExaminer.Common.Services.PatientDetails
{
    public class PatientDetailsRetrievalService : IAsyncQueryHandler<PatientDetailsByCaseIdQuery, Models.Examination>
    {
        private readonly IConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;

        public PatientDetailsRetrievalService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }

        public async Task<Models.Examination> Handle(PatientDetailsByCaseIdQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }
            var result = await _databaseAccess.GetItemAsync<Models.Examination>(_connectionSettings,
                examination => examination.ExaminationId == param.ExaminationId);

            return result;
        }
    }
}