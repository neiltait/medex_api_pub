using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.Users
{
    /// <summary>
    /// User Permission
    /// </summary>
    public class UserPermission
    {
        /// <summary>
        /// Permission Id.
        /// </summary>
        public string PermissionId { get; set; }

        /// <summary>
        /// Location Id.
        /// </summary>
        public string LocationId { get; set; }

        /// <summary>
        /// User Role.
        /// </summary>
        public UserRoles UserRole { get; set; }
    }
}
