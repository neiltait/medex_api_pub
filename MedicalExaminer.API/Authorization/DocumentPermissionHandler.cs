using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MedicalExaminer.API.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;

namespace MedicalExaminer.API.Authorization
{
    /// <summary>
    /// Permission Handler.
    /// </summary>
    /// <inheritdoc/>
    /// <see cref="AuthorizationHandler{PermissionRequirement, ILocationPath}"/>
    public class DocumentPermissionHandler : AuthorizationHandler<PermissionRequirement, ILocationPath>
    {
        /// <summary>
        /// Permission Service.
        /// </summary>
        private readonly IPermissionService _permissionService;

        /// <summary>
        /// Initialise a new instance of <see cref="DocumentPermissionHandler"/>.
        /// </summary>
        /// <param name="permissionService">Permission Service.</param>
        public DocumentPermissionHandler(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        /// <inheritdoc/>
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement,
            ILocationPath document)
        {
            var oktaId = context.User.Claims.Where(c => c.Type == MEClaimTypes.OktaUserId).Select(c => c.Value).First();

            if (oktaId != null)
            {
                if (await _permissionService.HasPermission(oktaId, document, requirement.Permission))
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
