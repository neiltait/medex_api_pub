using System;
using System.Collections.Generic;

namespace MedicalExaminer.Common.Queries.Examination
{
    /// <summary>
    /// Query used for generating the finance report
    /// </summary>
    public class FinanceQuery : IQuery<IEnumerable<Models.Examination>>
    {
        /// <summary>
        /// Finance query constructor
        /// </summary>
        /// <param name="dateFrom">the examination created date to run the report from</param>
        /// <param name="dateTo">the examination created date to run the report to</param>
        /// <param name="locationId">the location to run the report for</param>
        /// <param name="permissedLocation">the locations a user is permissed to run reports for</param>
        public FinanceQuery(DateTime dateFrom, DateTime dateTo, string locationId, IEnumerable<string> permissedLocation)
        {
            DateFrom = dateFrom;
            DateTo = dateTo;
            LocationId = locationId;
            PermissedLocation = permissedLocation;
        }

        /// <summary>
        /// Users permissed locations
        /// </summary>
        public IEnumerable<string> PermissedLocation { get; }

        /// <summary>
        /// Date to run the report from - based off the Examination Created Date
        /// </summary>
        public DateTime DateFrom { get; }

        /// <summary>
        /// Date to run the report to - based off the Examination Created Date
        /// </summary>
        public DateTime DateTo { get; }

        /// <summary>
        /// the location to run the report for
        /// </summary>
        public string LocationId { get; }
    }
}
