using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.Common.Queries.Permission
{
    public class PermissionsRetrievalByLocationsAndUserServiceQuery : IQuery<IEnumerable<Models.Permission>>
    {
        public PermissionsRetrievalByLocationsAndUserServiceQuery(IEnumerable<string> locations, string meUserId)
        {
            Locations = locations;
            MeUserId = meUserId;
        }

        public IEnumerable<string> Locations { get; }

        public string MeUserId { get; }
    }
}
