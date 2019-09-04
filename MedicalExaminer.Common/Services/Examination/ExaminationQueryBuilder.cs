using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MedicalExaminer.Common.Queries;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Services.Examination
{
    public class ExaminationsQueryExpressionBuilder
    {
        public Expression<Func<Models.Examination, bool>> GetPredicate(ExaminationsRetrievalQuery queryObject)
        {
            var permissedLocationFilter = GetPermissedLocationPredicate(queryObject.PermissedLocations);
            var locationFilter = GetLocationPredicate(queryObject.FilterLocationId);
            var caseStatusFilter = GetCaseStatusPredicate(queryObject.FilterCaseStatus);
            var userIdFilter = GetUserIdPredicate(queryObject.FilterUserId);
            var openCases = GetOpenCasesPredicate(queryObject.FilterOpenCases);

            var predicate = permissedLocationFilter
                .And(locationFilter)
                .And(caseStatusFilter)
                .And(userIdFilter)
                .And(openCases);

            return predicate;
        }

        private Expression<Func<Models.Examination, bool>> GetLocationPredicate(string locationId)
        {
            if (string.IsNullOrEmpty(locationId))
            {
                return null;
            }

            return examination =>
                examination.NationalLocationId == locationId
                || examination.RegionLocationId == locationId
                || examination.TrustLocationId == locationId
                || examination.SiteLocationId == locationId;
        }

        private Expression<Func<Models.Examination, bool>> GetPermissedLocationPredicate(IEnumerable<string> permissedLocations)
        {
            var locationList = permissedLocations.ToList();

            return examination =>
                locationList.Contains(examination.NationalLocationId)
                || locationList.Contains(examination.RegionLocationId)
                || locationList.Contains(examination.TrustLocationId)
                || locationList.Contains(examination.SiteLocationId);
        }

        private Expression<Func<Models.Examination, bool>> GetOpenCasesPredicate(bool paramFilterOpenCases)
        {
            return examination => examination.CaseCompleted == !paramFilterOpenCases;
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
                    return examination => examination.HaveFinalCaseOutcomesOutstanding;
                case null:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(paramFilterCaseStatus), paramFilterCaseStatus, null);
            }
        }

        private Expression<Func<Models.Examination, bool>> GetUserIdPredicate(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            Expression<Func<Models.Examination, bool>> mePredicate = examination => examination.MedicalTeam.MedicalExaminerUserId == userId;
            Expression<Func<Models.Examination, bool>> meoPredicate = examination => examination.MedicalTeam.MedicalExaminerOfficerUserId == userId;

            var predicate = mePredicate.Or(meoPredicate);
            return predicate;
        }
    }
}