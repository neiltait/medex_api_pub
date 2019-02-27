using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.Permissions;
using MedicalExaminer.Common;
using MedicalExaminer.Common.Loggers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Permission = MedicalExaminer.Models.Permission;

namespace MedicalExaminer.API.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Permissions Controller
    /// </summary>
    [Route("users/{userId}/permissions")]
    [ApiController]
    [Authorize]
    public class PermissionsController : BaseController
    {
        /// <summary>
        /// The Permission Persistence Layer
        /// </summary>
        private readonly IPermissionPersistence permissionPersistence;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionsController"/> class.
        /// </summary>
        /// <param name="userPersistence">The User Persistence.</param>
        /// <param name="permissionPersistence">The Permission Persistence</param>
        /// <param name="logger">The Logger.</param>
        /// <param name="mapper">The Mapper.</param>
        public PermissionsController(
            IUserPersistence userPersistence,
            IPermissionPersistence permissionPersistence,
            IMELogger logger,
            IMapper mapper)
            : base(logger, mapper)
        {
            this.permissionPersistence = permissionPersistence;
        }

        /// <summary>
        /// Get all Permissions for a User ID.
        /// </summary>
        /// <param name="meUserId">The User Identifier.</param>
        /// <returns>A GetPermissionsResponse.</returns>
        [HttpGet]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetPermissionsResponse>> GetPermissions(string meUserId)
        {
            try
            {
                var permissions = await permissionPersistence.GetPermissionsAsync(meUserId);

                return Ok(new GetPermissionsResponse
                {
                    Permissions = permissions.Select(p => Mapper.Map<PermissionItem>(p)),
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
        /// Get a Users permission by its Identifier.
        /// </summary>
        /// <param name="meUserId">The User Id.</param>
        /// <param name="permissionId">The Permission Id.</param>
        /// <returns>A GetPermissionResponse.</returns>
        [HttpGet("{id}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetPermissionResponse>> GetPermission(string meUserId, string permissionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GetPermissionResponse());
            }

            try
            {
                var permission = await permissionPersistence.GetPermissionAsync(meUserId, permissionId);
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
        /// Create a new Permission.
        /// </summary>
        /// <param name="postPermission">The PostPermissionRequest.</param>
        /// <returns>A PostPermissionResponse.</returns>
        [HttpPost]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PostPermissionResponse>> CreatePermission(PostPermissionRequest postPermission)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PostPermissionResponse());
            }

            try
            {
                var permission = Mapper.Map<Permission>(postPermission);
                var createdPermission = await permissionPersistence.CreatePermissionAsync(permission);

                // TODO: Is ID populated after saving?
                // TODO : Question : Should this be the whole user object?
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
        /// Create a new Permission.
        /// </summary>
        /// <param name="putPermission">The PutPermissionRequest.</param>
        /// <returns>A PutPermissionResponse.</returns>
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutPermissionResponse>> UpdatePermission(PutPermissionRequest putPermission)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new PutPermissionResponse());
                }

                var permission = Mapper.Map<Permission>(putPermission);
                var updatedPermission = await permissionPersistence.UpdatePermissionAsync(permission);
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
    }
}