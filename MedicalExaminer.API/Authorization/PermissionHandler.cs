using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MedicalExaminer.Common.Authorization;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;

namespace MedicalExaminer.API.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement, ILocationBasedDocument>
    {
        private readonly IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> _userRetrievalService;
        private readonly IRolePermissions _rolePermissions;

        public PermissionHandler(
            IRolePermissions rolePermissions,
            IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> userRetrievalService)
        {
            _userRetrievalService = userRetrievalService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement,
            ILocationBasedDocument document)
        {
            // get the user
            var emailAddress = context.User.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).First();

            var meUser = await _userRetrievalService.Handle(new UserRetrievalByEmailQuery(emailAddress));

            var permissions = meUser.Permissions;

            // get the permissions
            var hasPermission = permissions.Any(
                p => p.UserRole document.LocationIds().Any(l => l == p.LocationId));

            if (hasPermission)
            {
                context.Succeed(requirement);
            }
        }
    }
}
