using System;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Extensions.MeUser;
using MedicalExaminer.Common.Extensions.Models;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Queries.PatientDetails;
using MedicalExaminer.Common.Settings;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Microsoft.Extensions.Options;

namespace MedicalExaminer.Common.Services.PatientDetails
{
    /// <summary>
    /// Patient Details Update Service.
    /// </summary>
    public class PatientDetailsUpdateService : IAsyncQueryHandler<PatientDetailsUpdateQuery, Models.Examination>
    {
        private readonly IExaminationConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IMapper _mapper;
        private readonly IAsyncQueryHandler<LocationRetrievalByIdQuery, Models.Location> _locationHandler;
        private readonly UrgencySettings _urgencySettings;

        /// <summary>
        /// Initialise a new instance of <see cref="PatientDetailsUpdateService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">Connection Settings.</param>
        /// <param name="mapper">Mapper.</param>
        /// <param name="locationHandler">Location Handler.</param>
        public PatientDetailsUpdateService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings,
            IMapper mapper,
            IAsyncQueryHandler<LocationRetrievalByIdQuery, Models.Location> locationHandler,
            IOptions<UrgencySettings> urgencySettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
            _mapper = mapper;
            _locationHandler = locationHandler;
            _urgencySettings = urgencySettings.Value;
        }

        /// <summary>
        /// Handle.
        /// </summary>
        /// <param name="param">Patient Details Update Query.</param>
        /// <returns>An Examination.</returns>
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

            caseToReplace.UpdateLocationPath(param.Locations);
            caseToReplace.MedicalExaminerOfficeResponsibleName = _locationHandler.Handle(
                new LocationRetrievalByIdQuery(caseToReplace.MedicalExaminerOfficeResponsible)).Result.Name;
            caseToReplace.LastModifiedBy = param.User.UserId;
            caseToReplace.ModifiedAt = DateTime.Now;

            caseToReplace = caseToReplace.UpdateCaseUrgencySort(_urgencySettings.DaysToPreCalculateUrgencySort);
            caseToReplace = caseToReplace.UpdateCaseStatus();
            caseToReplace.CaseBreakdown.DeathEvent = _mapper.Map(caseToReplace, caseToReplace.CaseBreakdown.DeathEvent);
            caseToReplace.CaseBreakdown.DeathEvent.UserId = param.User.UserId;
            caseToReplace.CaseBreakdown.DeathEvent.UserFullName = param.User.FullName();
            caseToReplace.CaseBreakdown.DeathEvent.UsersRole = param.User.UsersRoleIn(new[] { UserRoles.MedicalExaminer, UserRoles.MedicalExaminerOfficer }).ToString();
            var result = await _databaseAccess.UpdateItemAsync(_connectionSettings, caseToReplace);
            return result;
        }
    }
}