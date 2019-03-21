using System;
using System.Linq.Expressions;
using MedicalExaminer.Common.Queries;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Services.Examination
{
    public class ExaminationQueryBuilder
    {

        public Expression<Func<Models.Examination, bool>> GetPredicate(ExaminationsRetrievalQuery queryObject)
        {
            Expression<Func<Models.Examination, bool>> caseStatusFilter = GetCaseStatusPredicate(queryObject.FilterCaseStatus);
            Expression<Func<Models.Examination, bool>> medicalExaminerOfficeFilter = GetCaseMEOfficePredicate(queryObject.FilterLocationId);
            Expression<Func<Models.Examination, bool>> userIdFilter = GetUserIdPredicate(queryObject.FilterUserId);
            Expression<Func<Models.Examination, bool>> openCases = GetOpenCasesPredicate(queryObject.FilterOpenCases);

            var predicate = caseStatusFilter.And(medicalExaminerOfficeFilter)
                .And(userIdFilter).And(openCases);

            return predicate;
        }

        private Expression<Func<Models.Examination, bool>> GetOpenCasesPredicate(bool paramFilterOpenCases)
        {
            return examination => examination.Completed == !paramFilterOpenCases;
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
                    return examination => examination.Assigned == false;
                case CaseStatus.HaveBeenScrutinisedByME:
                    return examination => examination.HaveBeenScrutinisedByME;
                case CaseStatus.PendingAdmissionNotes:
                    return examination => examination.PendingAdmissionNotes;
                case CaseStatus.PendingDiscussionWithQAP:
                    return examination => examination.PendingDiscussionWithQAP;
                case CaseStatus.PendingDiscussionWithRepresentative:
                    return examination => examination.PendingDiscussionWithRepresentative;
                case CaseStatus.HaveFinalCaseOutstandingOutcomes:
                    return examination => examination.HaveFinalCaseOutstandingOutcomes;
                case null:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(paramFilterCaseStatus), paramFilterCaseStatus, null);
            }
        }

        private Expression<Func<Models.Examination, bool>> GetCaseMEOfficePredicate(string meOffice)
        {
            if (string.IsNullOrEmpty(meOffice))
            {
                return null;
            }
            return examination => examination.MedicalExaminerOfficeResponsible == meOffice;
        }

        private Expression<Func<Models.Examination, bool>> GetUserIdPredicate(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }
            return examination => examination.CaseOfficer == userId;
        }
    }
}
