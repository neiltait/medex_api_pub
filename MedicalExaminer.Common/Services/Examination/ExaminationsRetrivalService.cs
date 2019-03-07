using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries;
using MedicalExaminer.Common.Queries.Examination;

namespace MedicalExaminer.Common.Services.Examination
{
    public class ExaminationsRetrivalService : IAsyncQueryHandler<ExaminationsRetrivalQuery, IEnumerable<Models.Examination>>
    {
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IConnectionSettings _connectionSettings;
        public ExaminationsRetrivalService(IDatabaseAccess databaseAccess, IExaminationConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }
        public Task<IEnumerable<Models.Examination>> Handle(ExaminationsRetrivalQuery param)
        {
            // can put whatever filters in the param, just empty for now
            return _databaseAccess.QueryAsync<Models.Examination>(_connectionSettings, "SELECT * FROM Examinations");
        }
    }
}
