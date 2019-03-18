using System.Collections.Generic;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Queries.Examination
{
    public class ExaminationsRetrievalQuery : IQuery<IEnumerable<Models.Examination>>
    {
        public readonly CaseStatus? FilterCaseStatus;
        public readonly string FilterLocationId;
        public readonly ExaminationsOrderBy? FilterOrderBy;
        public readonly int FilterPageNumber;
        public readonly int FilterPageSize;

        public ExaminationsRetrievalQuery(CaseStatus? filterCaseStatus, string filterLocationId, ExaminationsOrderBy? filterOrderBy, int filterPageNumber, int filterPageSize, string filterUserId)
        {
            FilterCaseStatus = filterCaseStatus;
            FilterLocationId = filterLocationId;
            FilterOrderBy = filterOrderBy;
            FilterPageNumber = filterPageNumber;
            FilterPageSize = filterPageSize;
        }
    }
}
