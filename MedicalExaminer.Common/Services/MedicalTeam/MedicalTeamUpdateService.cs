using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.MedicalTeam
{
    public class MedicalTeamUpdateService : IAsyncUpdateDocumentHandler
    {
        private readonly IConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;

        public MedicalTeamUpdateService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }

        public async Task<Models.Examination> Handle(Models.Examination examination)
        {
            if (examination == null)
            {
                throw new ArgumentNullException(nameof(examination));
            }

            examination = examination.UpdateCaseUrgencyScore();

            var returnedDocument = await _databaseAccess.UpdateItemAsync(_connectionSettings, examination);

            return returnedDocument;
        }
    }
}