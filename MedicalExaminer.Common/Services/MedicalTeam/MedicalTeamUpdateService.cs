using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Models;
using Microsoft.Azure.Documents;

namespace MedicalExaminer.Common.Services.MedicalTeam 
{
    public class MedicalTeamUpdateService : IAsyncUpdateDocumentHandler
    {
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IConnectionSettings _connectionSettings;

        public MedicalTeamUpdateService(IDatabaseAccess databaseAccess, IExaminationConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }

        public Task<string> Handle(Document examination)
        {
            if (examination == null)
            {
                throw new ArgumentNullException(nameof(examination));
            }
            // can put whatever filters in the param, just empty for now
            return _databaseAccess.Update(_connectionSettings, examination);
        }
    }
}
