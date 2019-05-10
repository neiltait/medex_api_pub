using System;
using System.Collections.Generic;
using System.Linq;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Queries.Examination
{
    /// <summary>
    /// Examinations Update Case Urgency Score Query.
    /// </summary>
    /// <seealso cref="IQuery{Int32}" />
    public class ExaminationsUpdateCaseUrgencyScoreQuery : IQuery<int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExaminationsUpdateCaseUrgencyScoreQuery"/> class.
        /// </summary>
        /// <param name="updatedBy">The user performing the update.</param>
        public ExaminationsUpdateCaseUrgencyScoreQuery(MeUser updatedBy)
        {
            UpdatedBy = updatedBy;
        }

        /// <summary>
        /// Gets or sets the updated by.
        /// </summary>
        public MeUser UpdatedBy { get; }
    }
}