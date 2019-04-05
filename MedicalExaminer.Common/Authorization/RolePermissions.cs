using System.Collections.Generic;
using System.Linq;
using MedicalExaminer.Common.Authorization.Roles;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Authorization
{
    /// <summary>
    /// Role Permissions.
    /// </summary>
    /// <inheritdoc cref="IRolePermissions"/>
    public class RolePermissions : IRolePermissions
    {
        /// <summary>
        /// Roles.
        /// </summary>
        private readonly IDictionary<UserRoles, Role> _roles;

        /// <summary>
        /// Initialise a new instance of <see cref="RolePermissions"/>.
        /// </summary>
        /// <param name="roles">Roles.</param>
        public RolePermissions(IEnumerable<Role> roles)
        {
            // Ensures only a single role handler can be registered againt the enum type.
            _roles = roles.ToDictionary(r => r.UserRole, r => r);
        }

        /// <inheritdoc/>
        public bool Can(UserRoles role, Permission permission)
        {
            try
            {
                return _roles[role].Can(permission);
            }
            catch
            {
                return false;
            }
        }
    }
}
