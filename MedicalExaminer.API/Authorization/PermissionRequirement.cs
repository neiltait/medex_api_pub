using MedicalExaminer.Common.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace MedicalExaminer.API.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public Permission Permission { get; }

        public PermissionRequirement(Permission permission)
        {
            Permission = permission;
        }
    }
}
