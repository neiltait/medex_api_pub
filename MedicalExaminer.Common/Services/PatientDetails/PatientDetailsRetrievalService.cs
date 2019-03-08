using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.PatientDetails;

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
            var result = _databaseAccess.QueryAsyncOne<Models.PatientDetails>(_connectionSettings,
                $"SELECT e.patient_details.given_names as GivenNames, e.patient_details.surname as Surname FROM Examinations e WHERE e.id = '{param.ExaminationId}'");

            return result;
        }
    }
}
