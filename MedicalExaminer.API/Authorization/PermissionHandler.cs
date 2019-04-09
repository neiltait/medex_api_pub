using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MedicalExaminer.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace MedicalExaminer.API.Authorization
{
    /// <summary>
    /// Permission Handler.
    /// </summary>
    /// <inheritdoc/>
    /// <see cref="AuthorizationHandler{PermissionRequirement}"/>
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        /// <summary>
        /// Permission Service.
        /// </summary>
        private readonly IPermissionService _permissionService;

        /// <summary>
        /// Initialise a new instance of <see cref="PermissionHandler"/>.
        /// </summary>
        /// <param name="permissionService">Permission Service.</param>
        public PermissionHandler(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        /// <inheritdoc/>
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var emailAddress = context.User.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).First();

            if (await _permissionService.HasPermission(emailAddress, requirement.Permission))
            {
                context.Succeed(requirement);
            }
        }
    }
}
