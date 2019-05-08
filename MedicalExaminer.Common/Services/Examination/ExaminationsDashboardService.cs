using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Services.Examination
{
    public class ExaminationsDashboardService : QueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview>
    {
        private readonly ExaminationsQueryExpressionBuilder _baseQueryBuilder;

        public ExaminationsDashboardService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings,
            ExaminationsQueryExpressionBuilder baseQueryBuilder)
            : base(databaseAccess, connectionSettings)
        {
            _baseQueryBuilder = baseQueryBuilder;
        }

        public override Task<ExaminationsOverview> Handle(ExaminationsRetrievalQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var baseQuery = GetBaseQuery(param);

            var overView = new ExaminationsOverview
            {
                CountOfAdmissionNotesHaveBeenAdded = GetCount(baseQuery, CaseStatus.AdmissionNotesHaveBeenAdded).Result,
                CountOfUnassigned = GetCount(baseQuery, CaseStatus.Unassigned).Result,
                CountOfHaveBeenScrutinisedByME = GetCount(baseQuery, CaseStatus.HaveBeenScrutinisedByME).Result,
                CountOfHaveFinalCaseOutstandingOutcomes = GetCount(baseQuery, CaseStatus.HaveFinalCaseOutstandingOutcomes).Result,
                CountOfPendingAdmissionNotes = GetCount(baseQuery, CaseStatus.PendingAdmissionNotes).Result,
                CountOfPendingDiscussionWithQAP = GetCount(baseQuery, CaseStatus.PendingDiscussionWithQAP).Result,
                CountOfPendingDiscussionWithRepresentative = GetCount(baseQuery, CaseStatus.PendingDiscussionWithRepresentative).Result,
                CountOfReadyForMEScrutiny = GetCount(baseQuery, CaseStatus.ReadyForMEScrutiny).Result,
                TotalCases = GetCount(baseQuery).Result,
                CountOfUrgentCases = GetCount(baseQuery, x => ((x.UrgencyScore > 0) && (x.CaseCompleted == false))).Result
            };

            return Task.FromResult(overView);
        }

        private async Task<int> GetCount(Expression<Func<Models.Examination, bool>> baseQuery, CaseStatus caseStatus)
        {
            var caseStatusPredicate = GetCaseStatusPredicate(caseStatus);
            var query = baseQuery.And(caseStatusPredicate);

            var result = await DatabaseAccess.GetCountAsync(ConnectionSettings, query);
            return result;
        }

        private async Task<int> GetCount(Expression<Func<Models.Examination, bool>> query)
        {
            return await DatabaseAccess.GetCountAsync(ConnectionSettings, query);
        }

        private async Task<int> GetCount(Expression<Func<Models.Examination, bool>> baseQuery, Expression<Func<Models.Examination, bool>> query)
        {
            var combinedQuery = query.And(baseQuery);
            return await DatabaseAccess.GetCountAsync(ConnectionSettings, combinedQuery);
        }

        private Expression<Func<Models.Examination, bool>> GetCaseStatusPredicate(CaseStatus? paramFilterCaseStatus)
        {
            switch (paramFilterCaseStatus)
            {
                case CaseStatus.AdmissionNotesHaveBeenAdded:
                    return examination => examination.AdmissionNotesHaveBeenAdded;
                case CaseStatus.ReadyForMEScrutiny:
                    return examination => examination.ReadyForMEScrutiny;
                case CaseStatus.Unassigned:
                    return examination => examination.Unassigned;
                case CaseStatus.HaveBeenScrutinisedByME:
                    return examination => examination.HaveBeenScrutinisedByME;
                case CaseStatus.PendingAdmissionNotes:
                    return examination => examination.PendingAdmissionNotes;
                case CaseStatus.PendingDiscussionWithQAP:
                    return examination => examination.PendingDiscussionWithQAP;
                case CaseStatus.PendingDiscussionWithRepresentative:
                    return examination => examination.PendingDiscussionWithRepresentative;
                case CaseStatus.HaveFinalCaseOutstandingOutcomes:
                    return examination => examination.HaveFinalCaseOutcomesOutstanding && examination.ScrutinyConfirmed;
                case null:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(paramFilterCaseStatus), paramFilterCaseStatus, null);
            }
        }

        private Expression<Func<Models.Examination, bool>> GetBaseQuery(ExaminationsRetrievalQuery param)
        {
            return _baseQueryBuilder.GetPredicate(param);
        }
    }
}
