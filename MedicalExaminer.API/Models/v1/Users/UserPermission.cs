using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalExaminer.API.Models.v1.Users
{
    public class UserPermission
    {
        public string PermissionId { get; set; }

        public string LocationId { get; set; }

        public int UserRole { get; set; }
    }
}
