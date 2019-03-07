using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.Queries;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services
{
    public class ExaminationRetrivalService : IAsyncQueryHandler<ExaminationRetrivalQuery, Examination>
    {
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IConnectionSettings _connectionSettings;
        public ExaminationRetrivalService(IDatabaseAccess databaseAccess, IConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }

        public async Task<Examination> Handle(ExaminationRetrivalQuery param)
        {
            using (var conn = _databaseAccess.CreateClient(_connectionSettings))
            {
                try
                {
                    return await _databaseAccess.QuerySingleAsync<Examination>(_connectionSettings, param.ExaminationId.ToString());

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
