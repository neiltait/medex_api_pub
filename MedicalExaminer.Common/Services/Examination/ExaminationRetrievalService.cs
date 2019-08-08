using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.Examination
{
    /// <summary>
    /// Examination Retrieval Service.
    /// </summary>
    public class ExaminationRetrievalService : QueryHandler<ExaminationRetrievalQuery, Models.Examination>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="ExaminationRetrievalService"/>.
        /// </summary>
        /// <param name="databaseAccess"></param>
        /// <param name="connectionSettings"></param>
        public ExaminationRetrievalService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings)
        : base(databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override Task<Models.Examination> Handle(ExaminationRetrievalQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var result = GetItemByIdAsync(param.ExaminationId);

            if (result.Result != null)
            {
                return Task.FromResult(result.Result);
            }

            return result;
        }
    }
}