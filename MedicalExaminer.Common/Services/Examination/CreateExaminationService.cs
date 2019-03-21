﻿using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;

namespace MedicalExaminer.Common.Services.Examination
{
    public class CreateExaminationService : IAsyncQueryHandler<CreateExaminationQuery, Models.Examination>
    {

        private readonly IDatabaseAccess _databaseAccess;
        private readonly IConnectionSettings _connectionSettings;
        public CreateExaminationService(IDatabaseAccess databaseAccess, IExaminationConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }
        
        public async Task<Models.Examination> Handle(CreateExaminationQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }
            try
            {
                param.Examination.id = Guid.NewGuid().ToString();
                param.Examination.Assigned = true;
                param.Examination.UrgencyScore = Calculator.CalculateUrgencyScore(param.Examination);
                return await _databaseAccess.CreateItemAsync(_connectionSettings, 
                    param.Examination, false);
            }
            catch (Exception e)
            {
                //_logger.Log("Failed to retrieve examination data", e);
                throw;
            }
            

        }
    }
}
