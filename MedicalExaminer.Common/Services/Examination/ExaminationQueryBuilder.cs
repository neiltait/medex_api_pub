using System;
using System.Linq.Expressions;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Services.Examination
{
    public class ExaminationQueryBuilder
    {
        
        public Expression<Func<Models.Examination, bool>> GetPredicate(ExaminationsRetrievalQuery queryObject)
        {
            var caseStatusFilter = GetCaseStatusPredicate(queryObject.FilterCaseStatus);
            var medicalExaminerOfficeFilter = GetCaseMEOfficePredicate(queryObject.FilterLocationId);
            var userIdFilter = GetUserIdPredicate(queryObject.FilterUserId);
            var openCases = GetOpenCasesPredicate(queryObject.FilterOpenCases);

            Func<Models.Examination, bool> query = examination =>
                (caseStatusFilter(examination) && medicalExaminerOfficeFilter(examination) && userIdFilter(examination) && openCases(examination));

            var expression = FuncToExpression(query);
            return expression;
        }

        private Func<Models.Examination, bool> GetOpenCasesPredicate(bool paramFilterOpenCases)
        {
            return examination => examination.Completed == paramFilterOpenCases;
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
