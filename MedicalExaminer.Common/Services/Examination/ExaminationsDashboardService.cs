using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Services.Examination
{
    public class ExaminationsDashboardService : IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview>
    {
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IConnectionSettings _connectionSettings;
        public ExaminationsDashboardService(IDatabaseAccess databaseAccess, IExaminationConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }

        public Task<ExaminationsOverview> Handle(ExaminationsRetrievalQuery param)
        {
            var baseQuery = GetBaseQuery(param);

            var countOfAdmissionNotesHaveBeenAdded = GetCount(baseQuery, CaseStatus.AdmissionNotesHaveBeenAdded);
            var countOfAssigned = GetCount(baseQuery, CaseStatus.Assigned);
            var countOfHaveBeenScrutinisedByME = GetCount(baseQuery, CaseStatus.HaveBeenScrutinisedByME);
            var countOfHaveFinalCaseOutstandingOutcomes = GetCount(baseQuery, CaseStatus.HaveFinalCaseOutstandingOutcomes);
            var countOfPendingAdmissionNotes = GetCount(baseQuery, CaseStatus.PendingAdmissionNotes);
            var countOfPendingDiscussionWithQAP = GetCount(baseQuery, CaseStatus.PendingDiscussionWithQAP);
            var countOfPendingDiscussionWithRepresentative = GetCount(baseQuery, CaseStatus.PendingDiscussionWithRepresentative);
            var countOfReadyForMEScrutiny = GetCount(baseQuery, CaseStatus.ReadyForMEScrutiny);

            var overView = new ExaminationsOverview
            {
                AdmissionNotesHaveBeenAdded = countOfAdmissionNotesHaveBeenAdded,
                Assigned = countOfAssigned,
                HaveBeenScrutinisedByME = countOfHaveBeenScrutinisedByME,
                HaveFinalCaseOutstandingOutcomes = countOfHaveFinalCaseOutstandingOutcomes,
                PendingAdmissionNotes = countOfPendingAdmissionNotes,
                PendingDiscussionWithQAP = countOfPendingDiscussionWithQAP,
                PendingDiscussionWithRepresentative = countOfPendingDiscussionWithRepresentative,
                ReadyForMEScrutiny = countOfReadyForMEScrutiny
            };

            return Task.FromResult(overView);
        }

        private int GetCount(Func<Models.Examination, bool> baseQuery, CaseStatus caseStatus)
        {
            var caseStatusPredicate = GetCaseStatusPredicate(caseStatus);
            Func<Models.Examination, bool> query = examination =>
                (baseQuery(examination) && caseStatusPredicate(examination));
            var expression = FuncToExpression(query);
            var result = _databaseAccess.GetCountAsync(_connectionSettings, expression);
            return result;
        }

        private Func<Models.Examination, bool> GetCaseStatusPredicate(CaseStatus? paramFilterCaseStatus)
        {
            switch (paramFilterCaseStatus)
            {
                case CaseStatus.AdmissionNotesHaveBeenAdded:
                    return examination => examination.AdmissionNotesHaveBeenAdded = true;
                case CaseStatus.ReadyForMEScrutiny:
                    return examination => examination.ReadyForMEScrutiny = true;
                case CaseStatus.Assigned:
                    return examination => examination.Assigned = false;
                case CaseStatus.HaveBeenScrutinisedByME:
                    return examination => examination.HaveBeenScrutinisedByME = true;
                case CaseStatus.PendingAdmissionNotes:
                    return examination => examination.PendingAdmissionNotes = true;
                case CaseStatus.PendingDiscussionWithQAP:
                    return examination => examination.PendingDiscussionWithQAP = true;
                case CaseStatus.PendingDiscussionWithRepresentative:
                    return examination => examination.PendingDiscussionWithRepresentative = true;
                case CaseStatus.HaveFinalCaseOutstandingOutcomes:
                    return examination => examination.HaveFinalCaseOutstandingOutcomes = true;
                case null:
                    return x => true;
                default:
                    throw new ArgumentOutOfRangeException(nameof(paramFilterCaseStatus), paramFilterCaseStatus, null);
            }
        }

        private Func<Models.Examination, bool> GetBaseQuery(ExaminationsRetrievalQuery param)
        {
            var openCasePredicate = GetOpenCasesPredicate(param.FilterOpenCases);
            var meOfficePredicate = GetCaseMEOfficePredicate(param.FilterLocationId);
            var userIdPredicate = GetUserIdPredicate(param.FilterUserId);

            return examination => (userIdPredicate(examination) && 
                                   meOfficePredicate(examination) && 
                                   openCasePredicate(examination));
        }

        private Func<Models.Examination, bool> GetOpenCasesPredicate(bool paramFilterOpenCases)
        {
            return examination => examination.Completed == paramFilterOpenCases;
        }
        
        private Func<Models.Examination, bool> GetCaseMEOfficePredicate(string meOffice)
        {
            if (string.IsNullOrEmpty(meOffice))
            {
                return examination => true;
            }
            return examination => examination.MedicalExaminerOfficeResponsible == meOffice;
        }

        private Func<Models.Examination, bool> GetUserIdPredicate(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return examination => true;
            }
            return examination => examination.CaseOfficer == userId;
        }

        private static Expression<Func<T, bool>> FuncToExpression<T>(Func<T, bool> f)
        {
            return x => f(x);
        }
    }
}
