using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
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
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }
                try
                {
                    param.Examination.ExaminationId = Guid.NewGuid().ToString();
                    var result = await _databaseAccess.CreateItemAsync(_connectionSettings, param.Examination, false);
                    return result.ExaminationId;
                }
                catch (Exception e)
                {
                    //_logger.Log("Failed to retrieve examination data", e);
                    throw;
                }
        }
    }
}
