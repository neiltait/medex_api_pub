using System.Collections.Generic;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Models.v1.Examinations
{
    /// <inheritdoc />
    /// <summary>
    ///     Response object for a list of examinations.
    /// </summary>
    public class GetExaminationsResponse : ResponseBase
    {
        /// <summary>
        ///     List of Examinations.
        /// </summary>
        public IEnumerable<Examination> Examinations { get; set; }
    }
}