﻿using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;

namespace MedicalExaminer.Common.Services.Examination
{
    public class ExaminationRetrivalService : IAsyncQueryHandler<ExaminationRetrivalQuery, Models.Examination>
    {
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IConnectionSettings _connectionSettings;
        public ExaminationRetrivalService(IDatabaseAccess databaseAccess, IExaminationConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }

        public Task<Models.Examination> Handle(ExaminationRetrivalQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }
                try
                {
                    var result =  _databaseAccess.QuerySingleAsync<Models.Examination>(_connectionSettings, param.ExaminationId);
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
