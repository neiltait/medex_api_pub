using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Services.Examination
{
    /// <summary>
    /// Examinations Retrieval Service.
    /// </summary>
    public class ExaminationsRetrievalService : QueryHandler<ExaminationsRetrievalQuery, IEnumerable<Models.Examination>>
    {
        private readonly ExaminationsQueryExpressionBuilder _examinationQueryBuilder;

        /// <summary>
        /// Initialise a new instance of <see cref="ExaminationsRetrievalService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">Connection Settings.</param>
        /// <param name="examinationQueryBuilder">Examination Query Builder.</param>
        public ExaminationsRetrievalService(IDatabaseAccess databaseAccess, IExaminationConnectionSettings connectionSettings, ExaminationsQueryExpressionBuilder examinationQueryBuilder)
            : base(databaseAccess, connectionSettings)
        {
            _examinationQueryBuilder = examinationQueryBuilder;
        }

        /// <inheritdoc/>
        public override Task<IEnumerable<Models.Examination>> Handle(ExaminationsRetrievalQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var predicate = _examinationQueryBuilder.GetPredicate(param);
            
            switch (param.FilterOrderBy)
            {
                case ExaminationsOrderBy.Urgency:
                    return GetItemsAsync(predicate, x => x.UrgencyScore);
                case ExaminationsOrderBy.CaseCreated:
                    return GetItemsAsync(predicate, x => x.CreatedAt);
                case null:
                    return GetItemsAsync(predicate);
                default:
                    throw new ArgumentOutOfRangeException(nameof(param.FilterOrderBy));
            }
        }
    }
}