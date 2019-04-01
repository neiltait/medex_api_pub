using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
        /// User Retrieval Service.
        /// </summary>
        private readonly IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> _userRetrievalService;

        /// <summary>
        /// Role Permissions.
        /// </summary>
        private readonly IRolePermissions _rolePermissions;

        /// <summary>
        /// Initialise a new instance of <see cref="PermissionHandler"/>.
        /// </summary>
        /// <param name="rolePermissions">Role Permissions.</param>
        /// <param name="userRetrievalService">User Retrieval Service.</param>
        public DocumentPermissionHandler(
            IRolePermissions rolePermissions,
            IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> userRetrievalService)
        {
            _rolePermissions = rolePermissions;

            _userRetrievalService = userRetrievalService;
        }

        /// <inheritdoc/>
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement,
            ILocationBasedDocument document)
        {
            // get the user
            var emailAddress = context.User.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).First();

            var meUser = await _userRetrievalService.Handle(new UserRetrievalByEmailQuery(emailAddress));

            var hasPermission = meUser.Permissions.Any(
                p => _rolePermissions.Can((UserRoles)p.UserRole, requirement.Permission)
                     && document.LocationIds().Any(l => l == p.LocationId));

            if (hasPermission)
            {
                context.Succeed(requirement);
            }
        }
    }
}
