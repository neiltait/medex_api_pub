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
        /// Permissions this role has been granted.
        /// </summary>
        private readonly HashSet<Permission> _granted = new HashSet<Permission>();

        /// <summary>
        /// Initialise a new instance of <see cref="Role"/>.
        /// </summary>
        /// <param name="userRole">User role to initialise.</param>
        protected Role(UserRoles userRole)
        {
            UserRole = userRole;
        }

        /// <summary>
        /// The User Role this Role is representing.
        /// </summary>
        public UserRoles UserRole { get; }

        /// <summary>
        /// Grant the role a permission.
        /// </summary>
        /// <param name="permission">Permission.</param>
        public void Grant(Permission permission)
        {
            _granted.Add(permission);
        }

        /// <summary>
        /// Grant the role a number of permissions.
        /// </summary>
        /// <param name="permissions">Permissions.</param>
        public void Grant(params Permission[] permissions)
        {
            foreach (var permission in permissions)
            {
                Grant(permission);
            }
        }

        /// <summary>
        /// Can role do Permission.
        /// </summary>
        /// <remarks>Is this role granted with this permission.</remarks>
        /// <param name="permission">Permission.</param>
        /// <returns>Boolean indicating whether it can.</returns>
        public bool Can(Permission permission)
        {
            return _granted.Contains(permission);
        }
    }
}
