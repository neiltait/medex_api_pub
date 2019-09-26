using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    /// <summary>
    /// Examinations Dashboard Service.
    /// </summary>
    public class ExaminationsDashboardService : QueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview>
    {
        private readonly ExaminationsQueryExpressionBuilder _baseQueryBuilder;

        /// <summary>
        /// Initialise a new instance of <see cref="ExaminationsDashboardService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">Connection Settings.</param>
        /// <param name="baseQueryBuilder">Base Query Builder.</param>
        public ExaminationsDashboardService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings,
            ExaminationsQueryExpressionBuilder baseQueryBuilder)
            : base(databaseAccess, connectionSettings)
        {
            _baseQueryBuilder = baseQueryBuilder;
        }

        /// <inheritdoc/>
        public override async Task<ExaminationsOverview> Handle(ExaminationsRetrievalQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var examinationCountQuery = GetBaseQuery(param, true); // this is all examinations

            var examinations = (await GetItemsAsync(examinationCountQuery)).ToList().AsQueryable();

            // Now do all the counting our side.
            var overView = new ExaminationsOverview
            {
                CountOfHaveUnknownBasicDetails = GetCount(examinations, CaseStatus.HaveUnknownBasicDetails),
                CountOfReadyForMEScrutiny = GetCount(examinations, CaseStatus.ReadyForMEScrutiny),
                CountOfUnassigned = GetCount(examinations, CaseStatus.Unassigned),
                CountOfHaveBeenScrutinisedByME = GetCount(examinations, CaseStatus.HaveBeenScrutinisedByME),
                CountOfPendingAdditionalDetails = GetCount(examinations, CaseStatus.PendingAdditionalDetails),
                CountOfPendingDiscussionWithQAP = GetCount(examinations, CaseStatus.PendingDiscussionWithQAP),
                CountOfPendingDiscussionWithRepresentative = GetCount(examinations, CaseStatus.PendingDiscussionWithRepresentative),
                CountOfHaveFinalCaseOutstandingOutcomes = GetCount(examinations, CaseStatus.HaveFinalCaseOutstandingOutcomes),
                TotalCases = examinations.Count(),
                CountOfUrgentCases = GetCount(examinations, x => x.IsUrgent() && x.CaseCompleted == false)
            };

            return overView;
        }

        private int GetCount(IQueryable<Models.Examination> examinations, CaseStatus caseStatus)
        {
            var caseStatusPredicate = GetCaseStatusPredicate(caseStatus);
            var result = examinations.Count(caseStatusPredicate);

            return result;
        }

        private int GetCount(IQueryable<Models.Examination> examinations, Expression<Func<Models.Examination, bool>> query)
        {
            return examinations.Count(query);
        }

        private Expression<Func<Models.Examination, bool>> GetCaseStatusPredicate(CaseStatus? paramFilterCaseStatus)
        {
            switch (paramFilterCaseStatus)
            {
                case CaseStatus.HaveUnknownBasicDetails:
                    return examination => examination.HaveUnknownBasicDetails;
                case CaseStatus.ReadyForMEScrutiny:
                    return examination => examination.ReadyForMEScrutiny;
                case CaseStatus.Unassigned:
                    return examination => examination.Unassigned;
                case CaseStatus.HaveBeenScrutinisedByME:
                    return examination => examination.HaveBeenScrutinisedByME;
                case CaseStatus.PendingAdditionalDetails:
                    return examination => examination.PendingAdditionalDetails;
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

        private Expression<Func<Models.Examination, bool>> GetBaseQuery(ExaminationsRetrievalQuery param, bool isDashboardCount)
        {
            return _baseQueryBuilder.GetPredicate(param, isDashboardCount);
        }
    }
}