using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;

namespace MedicalExaminer.Common.Services.Examination
{
    public class ExaminationRetrievalService : IAsyncQueryHandler<ExaminationRetrievalQuery, Models.Examination>
    {
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IConnectionSettings _connectionSettings;
        public ExaminationRetrievalService(IDatabaseAccess databaseAccess, IExaminationConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }

        public Task<Models.Examination> Handle(ExaminationRetrievalQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }
                try
                {
                    var result =  _databaseAccess.GetItemAsync<Models.Examination>(_connectionSettings, 
                        x=> x.ExaminationId == param.ExaminationId);
                    return result;
                }
                catch (Exception e)
                {
                    //_logger.Log("Failed to retrieve examination data", e);
                    throw;
                }
        }
    }
}
