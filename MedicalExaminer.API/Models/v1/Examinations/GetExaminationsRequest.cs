using MedicalExaminer.API.Attributes;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.Examinations
{
    /// <summary>
    /// A filter object to request examinations
    /// </summary>
    public class GetExaminationsRequest
    {
        /// <summary>
        /// The location id to get the examinations for
        /// </summary>
        [ValidMedicalExaminerOffice]
        public string LocationId { get; set; }

        /// <summary>
        /// The users id to retrieve examinations for
        /// </summary>
        [ValidUserId]
        public string UserId { get; set; }

        /// <summary>
        /// Case Status to filter on, mutually exclusive
        /// </summary>
        public CaseStatus? CaseStatus { get; set; }

        /// <summary>
        /// What to order the results by
        /// </summary>
        public ExaminationsOrderBy OrderBy { get; set; }

        /// <summary>
        /// Return only open cases = true, else false for closed cases
        /// </summary>
        public bool OpenCases { get; set; }

        /// <summary>
        /// What size page should be returned
        /// </summary>
        public int PageSize { get; set; }
        
        /// <summary>
        /// Page number to begin the results on
        /// </summary>
        public int PageNumber { get; set; }
    }
}
