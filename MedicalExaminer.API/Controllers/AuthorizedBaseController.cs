using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Authorization;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Permission = MedicalExaminer.Common.Authorization.Permission;

namespace MedicalExaminer.API.Controllers
{
    /// <summary>
    /// Base Authorization Controller.
    /// </summary>
    [Authorize]
    public abstract class AuthorizedBaseController : AuthenticatedBaseController
    {
        /// <summary>
        /// Authorization Based Controller.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="mapper">Mapper.</param>
        /// <param name="usersRetrievalByOktaIdService">User Retrieval By Okta Id Service.</param>
        /// <param name="authorizationService">Authorization Service.</param>
        /// <param name="permissionService">Permission Service.</param>
        protected AuthorizedBaseController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser> usersRetrievalByOktaIdService,
            IAuthorizationService authorizationService,
            IPermissionService permissionService)
            : base(logger, mapper, usersRetrievalByOktaIdService)
        {
            AuthorizationService = authorizationService;
            PermissionService = permissionService;
        }

        /// <summary>
        /// Authorization Service.
        /// </summary>
        protected IAuthorizationService AuthorizationService { get; }

        /// <summary>
        /// Permission Service.
        /// </summary>
        private IPermissionService PermissionService { get; }

        /// <summary>
        /// Can do Permission somewhere against something.
        /// </summary>
        /// <param name="permission">The Permission.</param>
        /// <returns>True if can.</returns>
        protected bool CanAsync(Permission permission)
        {
            var authorizationResult = AuthorizationService
                .AuthorizeAsync(User, null, new PermissionRequirement(permission)).Result;

            return authorizationResult.Succeeded;
        }

        /// <summary>
        /// Can do Permission to Document
        /// </summary>
        /// <param name="permission">The Permission.</param>
        /// <param name="document">The Document.</param>
        /// <returns>True if can.</returns>
        protected bool CanAsync(Permission permission, ILocationPath document)
        {
            var authorizationResult = AuthorizationService
                .AuthorizeAsync(User, document, new PermissionRequirement(permission)).Result;

            return authorizationResult.Succeeded;
        }

        /// <summary>
        /// Locations With Permission.
        /// </summary>
        /// <param name="permission">Permission.</param>
        /// <returns>List of Location Ids.</returns>
        protected async Task<IEnumerable<string>> LocationsWithPermission(Permission permission)
        {
            var currentUser = await CurrentUser();

            return PermissionService.LocationIdsWithPermission(currentUser, permission).Distinct();
        }
    }
}
