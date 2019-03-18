using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Services.Examination
{
    public class ExaminationsRetrievalService : IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Models.Examination>>
    {
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IConnectionSettings _connectionSettings;
        public ExaminationsRetrievalService(IDatabaseAccess databaseAccess, IExaminationConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }
        public Task<IEnumerable<Models.Examination>> Handle(ExaminationsRetrievalQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var caseStatusFilter = GetCaseStatusPredicate(param.FilterCaseStatus);
            var medicalExaminerOfficeFilter = GetCaseMEOfficePredicate(param.FilterLocationId);
            var userIdFilter = GetUserIdPredicate(param.)

            Func<Models.Examination, bool> query = examination =>
                (caseStatusFilter(examination) && medicalExaminerOfficeFilter(examination));

            var expression = FuncToExpression(query);
            
            // can put whatever filters in the param, just empty for now
            var results = _databaseAccess.GetItemsAsync<Models.Examination>(_connectionSettings, expression);

            return results;
        }

        private Func<Models.Examination, bool> GetCaseStatusPredicate(CaseStatus? paramFilterCaseStatus)
        {
            switch (paramFilterCaseStatus)
            {
                case CaseStatus.AdmissionNotesHaveBeenAdded:
                    return examination => examination.AdmissionNotesHaveBeenAdded = true;
                    break;
                case CaseStatus.ReadyForMEScrutiny:
                    return examination => examination.ReadyForMEScrutiny = true;
                    break;
                case CaseStatus.Assigned:
                    return examination => examination.Assigned = false;
                    break;
                case CaseStatus.HaveBeenScrutinisedByME:
                    return examination => examination.HaveBeenScrutinisedByME = true;
                    break;
                case CaseStatus.PendingAdmissionNotes:
                    return examination => examination.PendingAdmissionNotes = true;
                    break;
                case CaseStatus.PendingDiscussionWithQAP:
                    return examination => examination.PendingDiscussionWithQAP = true;
                    break;
                case CaseStatus.PendingDiscussionWithRepresentative:
                    return examination => examination.PendingDiscussionWithRepresentative = true;
                    break;
                case CaseStatus.HaveFinalCaseOutstandingOutcomes:
                    return examination => examination.HaveFinalCaseOutstandingOutcomes = true;
                    break;
                case null:
                    return x => true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(paramFilterCaseStatus), paramFilterCaseStatus, null);
            }
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
            return examination => examination. == meOffice;
        }



        private static Expression<Func<T, bool>> FuncToExpression<T>(Func<T, bool> f)
        {
            return x => f(x);
        }
    }
}
