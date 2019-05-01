using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Authorization;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.Permissions;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common;
using MedicalExaminer.Common.Extensions.Models;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Permission = MedicalExaminer.Models.Permission;

namespace MedicalExaminer.API.Controllers
{
    /// <inheritdoc />
    /// <summary>
    ///     Permissions Controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/users/{meUserId}/permissions")]
    [ApiController]
    [Authorize]
    public class PermissionsController : AuthorizedBaseController
    {
        /// <summary>
        /// User Retrieval By Id Service.
        /// </summary>
        private readonly IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser> _userRetrievalByIdService;

        /// <summary>
        /// User Update Service.
        /// </summary>
        private readonly IAsyncQueryHandler<UserUpdateQuery, MeUser> _userUpdateService;

        /// <summary>
        /// Locations Parents Service.
        /// </summary>
        private readonly
            IAsyncQueryHandler<LocationsParentsQuery, IDictionary<string, IEnumerable<Location>>>
            _locationsParentsService;

        /// <summary>
        ///     The Permission Persistence Layer
        /// </summary>
        private readonly IPermissionPersistence _permissionPersistence;

        /// <summary>
        /// Location Parents Service.
        /// </summary>
        private readonly IAsyncQueryHandler<LocationParentsQuery, IEnumerable<Location>> _locationParentsService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PermissionsController" /> class.
        /// </summary>
        /// <param name="logger">The Logger.</param>
        /// <param name="mapper">The Mapper.</param>
        /// <param name="usersRetrievalByEmailService">Users Retrieval by Email Service.</param>
        /// <param name="authorizationService">Authorization Service.</param>
        /// <param name="permissionService">Permission Service.</param>
        /// <param name="userRetrievalByIdService">User retrieval by id service.</param>
        /// <param name="userUpdateService">User update service.</param>
        /// <param name="locationParentsService">Location Parents Service.</param>
        /// <param name="locationsParentsService">Locations Parents Service.</param>
        /// <param name="permissionPersistence">The Permission Persistence</param>
        public PermissionsController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> usersRetrievalByEmailService,
            IAuthorizationService authorizationService,
            IPermissionService permissionService,
            IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser> userRetrievalByIdService,
            IAsyncQueryHandler<UserUpdateQuery, MeUser> userUpdateService,
            IAsyncQueryHandler<LocationParentsQuery, IEnumerable<Location>> locationParentsService,
            IAsyncQueryHandler<LocationsParentsQuery, IDictionary<string, IEnumerable<Location>>> locationsParentsService,
            IPermissionPersistence permissionPersistence)
            : base(logger, mapper, usersRetrievalByEmailService, authorizationService, permissionService)
        {
            _userRetrievalByIdService = userRetrievalByIdService;
            _userUpdateService = userUpdateService;
            _permissionPersistence = permissionPersistence;
            _locationParentsService = locationParentsService;
            _locationsParentsService = locationsParentsService;
        }

        /// <summary>
        ///     Get all Permissions for a User ID.
        /// </summary>
        /// <param name="meUserId">The User Identifier.</param>
        /// <returns>A GetPermissionsResponse.</returns>
        [HttpGet]
        [AuthorizePermission(Common.Authorization.Permission.GetUserPermissions)]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetPermissionsResponse>> GetPermissions(string meUserId)
        {
            try
            {
                // Get all the permission location ids on the user in the request.
                var meUser = await _userRetrievalByIdService.Handle(new UserRetrievalByIdQuery(meUserId));
                var permissionLocations = meUser.Permissions.Select(p => p.LocationId).ToList();

                // Get all the location paths for all those locations.
                var locationPaths =
                    await _locationsParentsService.Handle(new LocationsParentsQuery(permissionLocations));

                // The locations the user making the request has direct access to.
                var permissedLocations = (await LocationsWithPermission(Common.Authorization.Permission.GetUserPermissions)).ToList();

                // Select only the permissions that the user making the request has access to from the user in question.
                var permissions = meUser
                    .Permissions
                    .Where(p => locationPaths[p.LocationId]
                        .Any(l => permissedLocations.Contains(l.LocationId)));

                return Ok(new GetPermissionsResponse
                {
                    Permissions = permissions.Select(p => Mapper.Map<PermissionItem>(p))
                });
            }
            catch (DocumentClientException)
            {
                return NotFound(new GetPermissionsResponse());
            }
            catch (ArgumentException)
            {
                return NotFound(new GetPermissionsResponse());
            }
        }

        /// <summary>
        ///     Get a Users permission by its Identifier.
        /// </summary>
        /// <param name="meUserId">The User Id.</param>
        /// <param name="permissionId">The Permission Id.</param>
        /// <returns>A GetPermissionResponse.</returns>
        [HttpGet("{permissionId}")]
        [AuthorizePermission(Common.Authorization.Permission.GetUserPermission)]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetPermissionResponse>> GetPermission(string meUserId, string permissionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GetPermissionResponse());
            }

            try
            {
                var meUser = await _userRetrievalByIdService.Handle(new UserRetrievalByIdQuery(meUserId));

                var permission = meUser.Permissions.First(p => p.PermissionId == permissionId);

                var locationDocument = (await
                        _locationParentsService.Handle(
                            new LocationParentsQuery(permission.LocationId)))
                    .ToLocationPath();

                if (!CanAsync(Common.Authorization.Permission.GetUserPermission, locationDocument))
                {
                    return Forbid();
                }

                return Ok(Mapper.Map<GetPermissionResponse>(permission));
            }
            catch (ArgumentException)
            {
                return NotFound(new GetPermissionResponse());
            }
            catch (NullReferenceException)
            {
                return NotFound(new GetPermissionResponse());
            }
        }

        /// <summary>
        ///     Create a new Permission.
        /// </summary>
        /// <param name="postPermission">The PostPermissionRequest.</param>
        /// <returns>A PostPermissionResponse.</returns>
        [HttpPost]
        [AuthorizePermission(Common.Authorization.Permission.CreateUserPermission)]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PostPermissionResponse>> CreatePermission(
            [FromBody]
            PostPermissionRequest postPermission)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PostPermissionResponse());
            }

            try
            {
                var permission = Mapper.Map<Permission>(postPermission);

                var locationDocument = (await
                        _locationParentsService.Handle(
                            new LocationParentsQuery(permission.LocationId)))
                    .ToLocationPath();

                if (!CanAsync(Common.Authorization.Permission.CreateUserPermission, locationDocument))
                {
                    return Forbid();
                }

                var createdPermission = await _permissionPersistence.CreatePermissionAsync(permission);

                await DuplicateOnUser(permission);

                return Ok(Mapper.Map<PostPermissionResponse>(createdPermission));
            }
            catch (DocumentClientException)
            {
                return NotFound(new PostPermissionResponse());
            }
            catch (ArgumentException)
            {
                return NotFound(new PostPermissionResponse());
            }
        }

        /// <summary>
        /// Updates a Permission.
        /// </summary>
        /// <param name="putPermission">The PutPermissionRequest.</param>
        /// <returns>A PutPermissionResponse.</returns>
        [HttpPut("{permissionId}")]
        [AuthorizePermission(Common.Authorization.Permission.UpdateUserPermission)]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutPermissionResponse>> UpdatePermission(
            [FromBody]
            PutPermissionRequest putPermission)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new PutPermissionResponse());
                }

                var permission = Mapper.Map<Permission>(putPermission);

                var locationDocument = (await
                        _locationParentsService.Handle(
                            new LocationParentsQuery(permission.LocationId)))
                    .ToLocationPath();

                if (!CanAsync(Common.Authorization.Permission.CreateUserPermission, locationDocument))
                {
                    return Forbid();
                }

                var updatedPermission = await _permissionPersistence.UpdatePermissionAsync(permission);

                await DuplicateOnUser(permission);

                return Ok(Mapper.Map<PutPermissionResponse>(updatedPermission));
            }
            catch (DocumentClientException)
            {
                return NotFound(new PutPermissionResponse());
            }
            catch (ArgumentException)
            {
                return NotFound(new PutPermissionResponse());
            }
        }

        /// <summary>
        /// Replace whatever permissiona are on the user object with the current permissions
        /// in the collection.
        /// </summary>
        /// <remarks>Updates the user specified in this permission.</remarks>
        /// <param name="permission">A permission.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation completed successfully.</returns>
        private async Task<bool> DuplicateOnUser(Permission permission)
        {
            var meUser = await _userRetrievalByIdService.Handle(new UserRetrievalByIdQuery(permission.UserId));

            var permissions = (await _permissionPersistence.GetPermissionsAsync(permission.UserId)).ToList();

            meUser.Permissions = permissions.Select(p => new MEUserPermission()
            {
                LocationId = p.LocationId,
                PermissionId = p.PermissionId,
                UserRole = p.UserRole,
            }).ToList();

            var updatedUser = await _userUpdateService.Handle(new UserUpdateQuery(meUser));

            return updatedUser.Permissions.Count() == permissions.Count;
        }
    }
}