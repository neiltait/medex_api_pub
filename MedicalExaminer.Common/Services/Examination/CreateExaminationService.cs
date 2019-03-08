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

        public Task<string> Handle(CreateExaminationQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }
                try
                {
                    param.Examination.Id = Guid.NewGuid().ToString();
                    param.Examination.PatientDetails = new Models.PatientDetails()
                    {
                        GivenNames = param.Examination.GivenNames,
                        Surname = param.Examination.Surname
                    };
                    var result = _databaseAccess.Create(_connectionSettings, param.Examination);
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
