using System;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Queries.PatientDetails;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.PatientDetails
{
    public class PatientDetailsUpdateService : IAsyncQueryHandler<PatientDetailsUpdateQuery, Models.Examination>
    {
        private readonly IExaminationConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IMapper _mapper;
        private readonly IAsyncQueryHandler<LocationRetrievalByIdQuery, Models.Location> _locationHandler;
        public PatientDetailsUpdateService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings,
            IMapper mapper,
            IAsyncQueryHandler<LocationRetrievalByIdQuery, Models.Location> locationHandler)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
            _mapper = mapper;
            _locationHandler = locationHandler;
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

            caseToReplace.MedicalExaminerOfficeResponsibleName = _locationHandler.Handle(new LocationRetrievalByIdQuery(caseToReplace.MedicalExaminerOfficeResponsible)).Result.Name;
            caseToReplace.LastModifiedBy = param.UserId;

            caseToReplace = caseToReplace.UpdateCaseUrgencyScore();
            caseToReplace = caseToReplace.UpdateCaseStatus();
            caseToReplace.CaseBreakdown.DeathEvent = _mapper.Map(caseToReplace, caseToReplace.CaseBreakdown.DeathEvent);

            var result = await _databaseAccess.UpdateItemAsync(_connectionSettings, caseToReplace);
            return result;
        }

        
    }
}