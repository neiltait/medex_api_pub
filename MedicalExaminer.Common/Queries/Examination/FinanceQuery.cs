using System;
using System.Collections.Generic;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.Examination
{
    public class FinanceQuery : IQuery<IEnumerable<Models.Examination>>
    {
        public FinanceQuery(DateTime dateFrom, DateTime dateTo, string locationId, IEnumerable<string> permissedLocation)
        {
            DateFrom = dateFrom;
            DateTo = dateTo;
            LocationId = locationId;
            PermissedLocation = permissedLocation;
        }

        public IEnumerable<string> PermissedLocation { get; }
        public DateTime DateFrom { get; }
        public DateTime DateTo { get; }
        public string LocationId { get; }
    }
}
