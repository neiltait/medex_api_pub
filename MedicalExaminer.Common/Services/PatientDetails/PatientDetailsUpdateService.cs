using System;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.PatientDetails;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.PatientDetails
{
    public class PatientDetailsUpdateService : IAsyncQueryHandler<PatientDetailsUpdateQuery, Models.Examination>
    {
        private readonly IExaminationConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;

        private readonly IMapper _mapper;

        public PatientDetailsUpdateService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings,
            IMapper mapper)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
            _mapper = mapper;
        }

        public async Task<Models.Examination> Handle(PatientDetailsUpdateQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var caseToReplace = await
                _databaseAccess
                    .GetItemAsync<Models.Examination>(
                        _connectionSettings,
                        examination => examination.ExaminationId == param.CaseId);

            _mapper.Map(param.PatientDetails, caseToReplace);

            caseToReplace.UpdateCaseUrgencyScore();

            var result = await _databaseAccess.UpdateItemAsync(_connectionSettings, caseToReplace);
            return result;
        }
    }
}