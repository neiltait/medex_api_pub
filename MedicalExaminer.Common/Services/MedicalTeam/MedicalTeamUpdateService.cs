using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Extensions.MeUser;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.MedicalTeam
{
    public class MedicalTeamUpdateService : IAsyncUpdateDocumentHandler
    {
        private readonly IConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser> _userService;

        public MedicalTeamUpdateService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings,
            IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser> userService)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
            _userService = userService;
        }

        public async Task<Models.Examination> Handle(Models.Examination examination, string userId)
        {
            if (examination == null)
            {
                throw new ArgumentNullException(nameof(examination));
            }

            var me = await _userService.Handle(new UserRetrievalByIdQuery(examination.MedicalTeam.MedicalExaminerUserId));
            var meo = await _userService.Handle(new UserRetrievalByIdQuery(examination.MedicalTeam.MedicalExaminerOfficerUserId));

            examination.MedicalTeam.MedicalExaminerFullName = me.FullName();
            examination.MedicalTeam.MedicalExaminerOfficerFullName = meo.FullName();

            examination.UpdateCaseUrgencyScore();
            examination.LastModifiedBy = userId;
            var returnedDocument = await _databaseAccess.UpdateItemAsync(_connectionSettings, examination);

            return returnedDocument;
        }
    }
}