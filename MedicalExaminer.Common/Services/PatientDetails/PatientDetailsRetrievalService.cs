using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.PatientDetails;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.PatientDetails
{
    public class PatientDetailsRetrievalService : IAsyncQueryHandler<PatientDetailsByCaseIdQuery, Models.PatientDetails>
    {
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IConnectionSettings _connectionSettings;
        public PatientDetailsRetrievalService(IDatabaseAccess databaseAccess, IExaminationConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }
        public Task<Models.PatientDetails> Handle(PatientDetailsByCaseIdQuery param)
        {
            var result = _databaseAccess.GetItemAsync<IExamination>(_connectionSettings,
                examination => examination.Id == param.ExaminationId);

            return result.ContinueWith(x=>x.Result.PatientDetails);
        }
    }
}
