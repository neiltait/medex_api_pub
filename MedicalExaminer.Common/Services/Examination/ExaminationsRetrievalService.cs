using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cosmonaut;
using Cosmonaut.Extensions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Services.Examination
{
    /// <summary>
    /// Examinations Retrieval Service.
    /// </summary>
    public class ExaminationsRetrievalService : QueryHandler<ExaminationsRetrievalQuery, IEnumerable<Models.Examination>>
    {
        private readonly ExaminationsQueryExpressionBuilder _examinationQueryBuilder;
        private readonly ICosmosStore<Models.Examination> _store;

        /// <summary>
        /// Initialise a new instance of <see cref="ExaminationsRetrievalService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">Connection Settings.</param>
        /// <param name="examinationQueryBuilder">Examination Query Builder.</param>
        /// <param name="store">Cosmos Store for paging.</param>
        public ExaminationsRetrievalService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings,
            ExaminationsQueryExpressionBuilder examinationQueryBuilder,
            ICosmosStore<Models.Examination> store)
            : base(databaseAccess, connectionSettings)
        {
            _examinationQueryBuilder = examinationQueryBuilder;
            _store = store;
        }

        /// <inheritdoc/>
        public async override Task<IEnumerable<Models.Examination>> Handle(ExaminationsRetrievalQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var predicate = _examinationQueryBuilder.GetPredicate(param, false);
            switch (param.FilterOrderBy)
            {
                case ExaminationsOrderBy.Urgency:
                    var key = DateTime.Now.Date.UrgencyKey();
                    return _store
                        .Query()
                        .WithPagination(param.FilterPageNumber, param.FilterPageSize)
                        .Where(predicate)
                        .OrderByDescending(x => x.UrgencySort[key])
                        .ToListAsync()
                        .Result;

                case ExaminationsOrderBy.CaseCreated:
                    return _store.Query().WithPagination(param.FilterPageNumber, param.FilterPageSize).Where(predicate).OrderBy(x => x.CreatedAt).ToListAsync().Result;
                case null:
                    return _store.Query().WithPagination(param.FilterPageNumber, param.FilterPageSize).Where(predicate).ToListAsync().Result;
                default:
                    throw new ArgumentOutOfRangeException(nameof(param.FilterOrderBy));
            }
        }
    }
}