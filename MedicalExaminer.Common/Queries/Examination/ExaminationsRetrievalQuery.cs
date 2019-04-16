using System.Collections.Generic;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Queries.Examination
{
    public class ExaminationsRetrievalQuery : IQuery<IEnumerable<Models.Examination>>, IQuery<ExaminationsOverview>
    {
        public readonly string PagingToken;
        public readonly CaseStatus? FilterCaseStatus;
        public readonly string FilterLocationId;
        public readonly ExaminationsOrderBy? FilterOrderBy;
        public readonly int FilterPageNumber;
        public readonly int FilterPageSize;
        public readonly string FilterUserId;
        public readonly bool FilterOpenCases;
        public readonly string UserId;

        public ExaminationsRetrievalQuery(CaseStatus? filterCaseStatus, string filterLocationId, 
            ExaminationsOrderBy? filterOrderBy, int filterPageNumber, int filterPageSize, 
            string filterUserId, bool filterOpenCases, string pagingToken, string userId)
        {
            FilterCaseStatus = filterCaseStatus;
            FilterLocationId = filterLocationId;
            FilterOrderBy = filterOrderBy;
            FilterPageNumber = filterPageNumber;
            FilterPageSize = filterPageSize;
            FilterUserId = filterUserId;
            FilterOpenCases = filterOpenCases;
            PagingToken = pagingToken;
            UserId = userId;
        }
    }
}