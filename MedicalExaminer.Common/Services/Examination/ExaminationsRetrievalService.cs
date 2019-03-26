using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Services.Examination
{
    public class ExaminationsRetrievalService : IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Models.Examination>>
    {
        private readonly IConnectionSettings _connectionSettings;
        private readonly ExaminationsQueryExpressionBuilder _examinationQueryBuilder;
        private readonly IDatabaseAccess _databaseAccess;

        public ExaminationsRetrievalService(IDatabaseAccess databaseAccess, IExaminationConnectionSettings connectionSettings, ExaminationsQueryExpressionBuilder examinationQueryBuilder)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
            _examinationQueryBuilder = examinationQueryBuilder;
        }

        public Task<IEnumerable<Models.Examination>> Handle(ExaminationsRetrievalQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var predicate = _examinationQueryBuilder.GetPredicate(param);
            
            switch (param.FilterOrderBy)
            {
                case ExaminationsOrderBy.Urgency:
                    return _databaseAccess.GetItemsAsync(_connectionSettings, predicate, x => x.UrgencyScore);
                case ExaminationsOrderBy.CaseCreated:
                    return _databaseAccess.GetItemsAsync(_connectionSettings, predicate, x => x.CreatedAt);
                case null:
                    return _databaseAccess.GetItemsAsync(_connectionSettings, predicate);
                default:
                    throw new ArgumentOutOfRangeException(nameof(param.FilterOrderBy));
            }
        }
    }
}