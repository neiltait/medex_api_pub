using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MedicalExaminer.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

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
            // Only check if no resource was requested;
            // Resource requests should be handled in DocumentPermissionHandler.
            if (context.Resource == null || context.Resource is AuthorizationFilterContext)
            {
                var oktaId = context.User.Claims.Where(c => c.Type == MEClaimTypes.OktaUserId).Select(c => c.Value)
                    .FirstOrDefault();

                if (oktaId != null)
                {
                    if (await _permissionService.HasPermission(oktaId, requirement.Permission))
                    {
                        context.Succeed(requirement);
                    }
                }
            }
        }
    }
}
