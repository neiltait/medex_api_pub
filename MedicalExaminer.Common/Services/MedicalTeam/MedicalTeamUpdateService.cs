using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Extensions.MeUser;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.MedicalTeam
{
    /// <summary>
    /// Medical Team Update Service.
    /// </summary>
    public class MedicalTeamUpdateService : IAsyncUpdateDocumentHandler
    {
        private readonly IConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser> _userRetrievalByIdService;

        /// <summary>
        /// Initialise a new instance of <see cref="MedicalTeamUpdateService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database access.</param>
        /// <param name="connectionSettings">Connection settings.</param>
        /// <param name="userRetrievalByIdService">User retrieval by Id service.</param>
        public MedicalTeamUpdateService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings,
            IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser> userRetrievalByIdService)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
            _userRetrievalByIdService = userRetrievalByIdService;
        }

        /// <inheritdoc/>
        public async Task<Models.Examination> Handle(Models.Examination examination, string userId)
        {
            if (examination == null)
            {
                throw new ArgumentNullException(nameof(examination));
            }

            examination.MedicalTeam.MedicalExaminerFullName = await GetFullName(examination.MedicalTeam.MedicalExaminerUserId);
            examination.MedicalTeam.MedicalExaminerOfficerFullName = await GetFullName(examination.MedicalTeam.MedicalExaminerOfficerUserId);

            examination = examination.UpdateCaseUrgencyScore();
            examination = examination.UpdateCaseStatus();
            examination.LastModifiedBy = userId;
            examination.ModifiedAt = DateTime.Now;

            var returnedDocument = await _databaseAccess.UpdateItemAsync(_connectionSettings, examination);

            return returnedDocument;
        }

        /// <summary>
        /// Get Full Name.
        /// Helper method to get the full name of a user.
        /// </summary>
        /// <remarks>The user id can be null and in this case don't query and just return.</remarks>
        /// <param name="userId">The id of the user.</param>
        /// <returns>The full name or null</returns>
        private async Task<string> GetFullName(string userId)
        {
            if (userId == null)
            {
                return null;
            }

            var me = await _userRetrievalByIdService.Handle(new UserRetrievalByIdQuery(userId));
            return me.FullName();
        }
    }
}