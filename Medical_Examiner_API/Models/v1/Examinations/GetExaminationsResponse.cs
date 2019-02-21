using System.Collections.Generic;

namespace Medical_Examiner_API.Models.V1.Examinations
{
    /// <inheritdoc />
    /// <summary>
    /// Response object for a list of examinations.
    /// </summary>
    public class GetExaminationsResponse : ResponseBase
    {
        /// <summary>
        /// List of Examinations.
        /// </summary>
        public IEnumerable<ExaminationItem> Examinations { get; set; }
    }
}
