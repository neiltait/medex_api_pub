using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.Examination
{
    /// <summary>
    /// summary text
    /// </summary>
    public class FinanceService : QueryHandler<FinanceQuery, IEnumerable<Models.Examination>>
    {
        private ExaminationsQueryExpressionBuilder _examinationQueryBuilder;
        public FinanceService(IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings,
            ExaminationsQueryExpressionBuilder examinationQueryBuilder)
            : base(databaseAccess, connectionSettings)
        {
            _examinationQueryBuilder = examinationQueryBuilder;
        }

        public override Task<IEnumerable<Models.Examination>> Handle(FinanceQuery param)
        {
            var queryExpression = _examinationQueryBuilder.GetFinancePredicate(param);

            return DatabaseAccess.GetItemsAsync(ConnectionSettings, queryExpression);
        }
    }
}
