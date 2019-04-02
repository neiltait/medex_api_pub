using System;
using System.Collections.Generic;
using System.Text;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Authorization
{
    /// <summary>
    /// Role.
    /// </summary>
    public abstract class Role
    {
        /// <summary>
        /// The User Role this Role is representing.
        /// </summary>
        public UserRoles UserRole { get; }

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
        /// Grant the role a permission.
        /// </summary>
        /// <param name="feature"></param>
        public void Grant(Permission feature)
        {
            _granted.Add(feature);
        }

        /// <summary>
        /// Can role do Permission.
        /// </summary>
        /// <remarks>Is this role granted with this permission.</remarks>
        /// <param name="feature"></param>
        /// <returns>Boolean indicating whether it can.</returns>
        public bool Can(Permission feature)
        {
            return _granted.Contains(feature);
        }
    }
}
