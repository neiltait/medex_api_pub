using System;
using System.Collections.Generic;
using System.Text;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Authorization
{
    /// <summary>
    /// Role Permissions Interface.
    /// </summary>
    public interface IRolePermissions
    {
        /// <summary>
        /// Can Role perform Permission.
        /// </summary>
        /// <param name="role">The Role.</param>
        /// <param name="permission">The Permission.</param>
        /// <returns>True if it can.</returns>
        bool Can(UserRoles role, Permission permission);

        /// <summary>
        /// Get Permissions for Roles
        /// </summary>
        /// <param name="roles">List of Roles</param>
        /// <returns>Dictionary of Permissions with true/false indicating they have the permission or not.</returns>
        IDictionary<Permission, bool> PermissionsForRoles(IEnumerable<UserRoles> roles);
    }
}
