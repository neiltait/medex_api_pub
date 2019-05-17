using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.API.Models;
using MedicalExaminer.Common.Authorization;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Microsoft.Extensions.Options;
using Permission = MedicalExaminer.Common.Authorization.Permission;

namespace MedicalExaminer.API.Services.Implementations
{
    /// <summary>
    /// Permission Service.
    /// </summary>
    public class PermissionService : IPermissionService
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
        /// Authorization Settings.
        /// </summary>
        private readonly IOptions<AuthorizationSettings> _authorizationSettings;

        /// <summary>
        /// Permission Service.
        /// </summary>
        /// <param name="rolePermissions">Role Permissions.</param>
        /// <param name="userRetrievalService">User Retrieval Service.</param>
        /// <param name="authorizationSettings">Authorization Settings.</param>
        public PermissionService(
            IRolePermissions rolePermissions,
            IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> userRetrievalService,
            IOptions<AuthorizationSettings> authorizationSettings)
        {
            _rolePermissions = rolePermissions;
            _userRetrievalService = userRetrievalService;
            _authorizationSettings = authorizationSettings;
        }

        /// <inheritdoc/>
        public async Task<bool> HasPermission(string emailAddress, ILocationPath document, Permission permission)
        {
            if (_authorizationSettings.Value.Disable)
            {
                return true;
            }

            var meUser = await _userRetrievalService.Handle(new UserRetrievalByEmailQuery(emailAddress));

            if (meUser.Permissions != null)
            {
                var hasPermission = meUser.Permissions.Any(
                    p => _rolePermissions.Can((UserRoles)p.UserRole, permission)
                         && document.LocationIds().Any(l => l == p.LocationId));

                if (hasPermission)
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public async Task<bool> HasPermission(string emailAddress, Permission permission)
        {
            if (_authorizationSettings.Value.Disable)
            {
                return true;
            }

            var meUser = await _userRetrievalService.Handle(new UserRetrievalByEmailQuery(emailAddress));

            if (meUser.Permissions != null)
            {
                var hasPermission = meUser.Permissions.Any(
                    p => _rolePermissions.Can((UserRoles)p.UserRole, permission));

                if (hasPermission)
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public IEnumerable<string> LocationIdsWithPermission(MeUser user, Permission permission)
        {
            var locations = user
                .Permissions
                .Where(up => up.LocationId != null)
                .Where(up => _rolePermissions.Can((UserRoles)up.UserRole, permission))
                .Select(up => up.LocationId);

            return locations;
        }
    }
}
