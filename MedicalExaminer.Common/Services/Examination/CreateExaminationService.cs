using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Extensions.MeUser;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Services.Examination
{
    public class CreateExaminationService : IAsyncQueryHandler<CreateExaminationQuery, Models.Examination>
    {
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IConnectionSettings _connectionSettings;
        private readonly IAsyncQueryHandler<LocationRetrievalByIdQuery, Models.Location> _locationHandler;
        //private readonly IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> _usersRetrievalByEmailService;


        public CreateExaminationService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings,
            IAsyncQueryHandler<LocationRetrievalByIdQuery, Models.Location> locationHandler)
      //      IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> usersRetrievalByEmailService)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
            _locationHandler = locationHandler;
        //    _usersRetrievalByEmailService = usersRetrievalByEmailService;
        }

        public async Task<Models.Examination> Handle(CreateExaminationQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            param.Examination.ExaminationId = Guid.NewGuid().ToString();
            param.Examination.MedicalExaminerOfficeResponsibleName = _locationHandler.Handle(new LocationRetrievalByIdQuery(param.Examination.MedicalExaminerOfficeResponsible)).Result.Name;
            param.Examination.Unassigned = true;
            param.Examination.CaseBreakdown = new CaseBreakDown();

            param.Examination.CaseBreakdown.DeathEvent = new DeathEvent()
            {
                Created = param.Examination.CreatedAt.Date,
                DateOfDeath = param.Examination.DateOfDeath,
                TimeOfDeath = param.Examination.TimeOfDeath,
                UserId = param.Examination.CreatedBy,
                UsersRole = param.User.UsersExaminationRole(new[] { UserRoles.MedicalExaminer, UserRoles.MedicalExaminerOfficer }).ToString(),
                UserFullName = param.User.FullName(),
                EventId = Guid.NewGuid().ToString()
            };
            param.Examination.UpdateCaseUrgencyScore();
            param.Examination.LastModifiedBy = param.User.UserId;
            return await _databaseAccess.CreateItemAsync(_connectionSettings, param.Examination);
        }
    }
}