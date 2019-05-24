using System.Collections.Generic;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Authorization
{
    /// <summary>
    /// Role.
    /// </summary>
    public abstract class Role
    {
        /// <summary>
        /// Initialise a new instance of <see cref="Role"/>.
        /// </summary>
        /// <param name="userRole">User role to initialise.</param>
        protected Role(UserRoles userRole)
        {
            UserRole = userRole;
        }

        /// <summary>
        /// Granted.
        /// </summary>
        /// <remarks>Get all granted permissions.</remarks>
        public HashSet<Permission> Granted { get; } = new HashSet<Permission>();

        /// <summary>
        /// The User Role this Role is representing.
        /// </summary>
        public UserRoles UserRole { get; }

        /// <summary>
        /// Can role do Permission.
        /// </summary>
        /// <remarks>Is this role granted with this permission.</remarks>
        /// <param name="permission">Permission.</param>
        /// <returns>Boolean indicating whether it can.</returns>
        public bool Can(Permission permission)
        {
            return Granted.Contains(permission);
        }

        /// <summary>
        /// Grant the role a permission.
        /// </summary>
        /// <param name="permission">Permission.</param>
        protected void Grant(Permission permission)
        {
            Granted.Add(permission);
        }

        /// <summary>
        /// Grant the role a number of permissions.
        /// </summary>
        /// <param name="permissions">Permissions.</param>
        protected void Grant(params Permission[] permissions)
        {
            foreach (var permission in permissions)
            {
                Grant(permission);
            }
        }
    }
}
