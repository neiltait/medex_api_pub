using System.Collections.Generic;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Queries.User
{
    public class UsersRetrievalQuery : IQuery<IEnumerable<MeUser>>
    {
        public UsersRetrievalQuery(bool forLookup, UserRoles? userRole)
        {
            role = userRole;
            ForLookup = forLookup;
        }

        public bool ForLookup { get; }

        public UserRoles? role { get; }
    }
}