using System.Collections.Generic;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Queries.User
{
    public class UsersRetrievalQuery : IQuery<IEnumerable<MeUser>>
    {
        public UsersRetrievalQuery(UserRoles? userRole)
        {
            role = userRole;
        }

        public UserRoles? role { get; }
    }
}