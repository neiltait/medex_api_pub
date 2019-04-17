using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        ///     The Permission Persistence Layer
        /// </summary>
        private readonly IPermissionPersistence _permissionPersistence;

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
            IPermissionPersistence permissionPersistence)
            : base(logger, mapper, usersRetrievalByEmailService, authorizationService, permissionService)
        {
            _userRetrievalByIdService = userRetrievalByIdService;
            _userUpdateService = userUpdateService;
            _permissionPersistence = permissionPersistence;
            _locationParentsService = locationParentsService;
        }

        /// <summary>
        ///     Get all Permissions for a User ID.
        /// </summary>
        /// <param name="meUserId">The User Identifier.</param>
        /// <returns>A GetPermissionsResponse.</returns>
        [HttpGet]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetPermissionsResponse>> GetPermissions(string meUserId)
        {
            // Is this user me - then we can return permissions
            // Do we have the get permissions permission?

            try
            {
                var permissions = await _permissionPersistence.GetPermissionsAsync(meUserId);

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
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetPermissionResponse>> GetPermission(string meUserId, string permissionId)
        {
            // Can only get if
            // The user is you
            // The location is one you have permission on.

            if (!ModelState.IsValid)
            {
                return BadRequest(new GetPermissionResponse());
            }

            try
            {
                var permission = await _permissionPersistence.GetPermissionAsync(meUserId, permissionId);
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
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PostPermissionResponse>> CreatePermission(
            [FromBody]
            PostPermissionRequest postPermission)
        {
            // Do I have permission in the location that is being added.
            // You can only give permission to somebody that you already have.

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
                    Forbid();
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
                    Forbid();
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