using System;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.Examination
{
    /// <summary>
    /// Examinations Update Case Urgency Score Service.
    /// </summary>
    public class ExaminationsUpdateCaseUrgencyScoreService : QueryHandler<ExaminationsUpdateCaseUrgencyScoreQuery, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExaminationsUpdateCaseUrgencyScoreService"/> class.
        /// </summary>
        /// <param name="databaseAccess">The database access.</param>
        /// <param name="connectionSettings">The connection settings.</param>
        public ExaminationsUpdateCaseUrgencyScoreService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings)
            : base(databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override async Task<int> Handle(ExaminationsUpdateCaseUrgencyScoreQuery param)
        {
            var examinations = (await DatabaseAccess.GetItemsAsync<Models.Examination>(
                ConnectionSettings,
                examination => examination.CaseCompleted == false)).ToList();

            foreach (var examination in examinations)
            {
                examination.UpdateCaseUrgencyScore();
                examination.LastModifiedBy = param.UpdatedBy.UserId;
                examination.ModifiedAt = DateTimeOffset.UtcNow;
            }

            var returnedDocuments = await DatabaseAccess.UpdateItemAsync(ConnectionSettings, examinations);

            return returnedDocuments.Count;
        }
    }
}