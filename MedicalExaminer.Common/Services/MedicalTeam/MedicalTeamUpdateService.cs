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

        public async Task<string> Handle(Models.Examination examination)
        {
            if (examination == null)
            {
                throw new ArgumentNullException(nameof(examination));
            }

            var returnedDocument = await _databaseAccess.UpdateItemAsync(_connectionSettings, examination);

            return returnedDocument.Id;
        }
    }
}
