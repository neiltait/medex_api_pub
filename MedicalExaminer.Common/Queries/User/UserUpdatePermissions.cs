using System.Collections.Generic;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    public class UserUpdatePermissions : IUserUpdate
    {
        public string UserId { get; set; }

        public IEnumerable<MEUserPermission> Permissions { get; set; }
    }
}
