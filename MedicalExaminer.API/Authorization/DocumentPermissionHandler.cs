using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Authorization;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Microsoft.AspNetCore.Authorization;

namespace MedicalExaminer.API.Authorization
{
    /// <summary>
    /// Permission Handler.
    /// </summary>
    /// <inheritdoc/>
    /// <see cref="AuthorizationHandler{PermissionRequirement, ILocationBasedDocument}"/>
    public class DocumentPermissionHandler : AuthorizationHandler<PermissionRequirement, ILocationBasedDocument>
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
            ILocationBasedDocument document)
        {
            var emailAddress = context.User.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).First();

            if (await _permissionService.HasPermission(emailAddress, document, requirement.Permission))
            {
                context.Succeed(requirement);
            }
        }
    }
}
