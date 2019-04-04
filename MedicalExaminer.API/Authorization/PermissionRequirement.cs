using MedicalExaminer.Common.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace MedicalExaminer.API.Authorization
{
    /// <summary>
    /// Permission Requirement.
    /// </summary>
    public class PermissionRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// Permission.
        /// </summary>
        public Permission Permission { get; }

        /// <summary>
        /// Initialise a new instance of <see cref="PermissionRequirement"/>
        /// </summary>
        /// <param name="permission">Permission.</param>
        public PermissionRequirement(Permission permission)
        {
            Permission = permission;
        }
    }
}
