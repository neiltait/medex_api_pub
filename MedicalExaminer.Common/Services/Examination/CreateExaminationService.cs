using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries;
using MedicalExaminer.Common.Queries.Examination;

namespace MedicalExaminer.Common.Services.Examination
{
    public class CreateExaminationService : IAsyncQueryHandler<CreateExaminationQuery, string>
    {
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IConnectionSettings _connectionSettings;
        public CreateExaminationService(IDatabaseAccess databaseAccess, IExaminationConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }

        public async Task<string> Handle(CreateExaminationQuery param)
        {
            using (var conn = _databaseAccess.CreateClient(_connectionSettings))
            {
                try
                {
                    param.Examination.Id = Guid.NewGuid().ToString();
                    return await _databaseAccess.Create<Models.Examination>(_connectionSettings, param.Examination);

                }
                catch (Exception e)
                {
                    //_logger.Log("Failed to retrieve examination data", e);
                    throw;
                }
            }
        }
    }
}
