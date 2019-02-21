using System;
using System.Linq;
using System.Threading.Tasks;
using Medical_Examiner_API.Extensions.Data;
using Medical_Examiner_API.Loggers;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Medical_Examiner_API.Models.V1.Users;

namespace Medical_Examiner_API.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Users Controller
    /// </summary>
    [Route("users/{userId}/permissions")]
    [ApiController]
    public class PermissionsController : BaseController
    {
        /// <summary>
        /// The User Persistance Layer
        /// </summary>
        private readonly IUserPersistence userPersistence;

        private readonly IPermissionPersistence permissionPersistence;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionsController"/> class.
        /// </summary>
        /// <param name="userPersistence">The User Persistence.</param>
        /// <param name="permissionPersistence">The Permission Persistence</param>
        /// <param name="logger">The Logger.</param>
        public PermissionsController(IUserPersistence userPersistence, IPermissionPersistence permissionPersistence,
            IMELogger logger)
            : base(logger)
        {
            this.userPersistence = userPersistence;
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
                    Permissions = permissions.Select(p => p.ToPermissionItem()),
                });
            }
            catch (DocumentClientException)
            {
                return NotFound();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get a Users permission by its Identifier.
        /// </summary>
        /// <param name="meUserId">The User Id.</param>
        /// <param name="permissionId">The Permission Id.</param>
        /// <returns>A GetPermissionResponse.</returns>
        // GET api/users/{user_id}
        [HttpGet("{id}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetPermissionResponse>> GetPermission(string meUserId, string permissionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GetPermissionResponse());
            }

            // Throws exception when not found; maybe should return null and exception
            // handled in logging?
            try
            {
                var permission = await permissionPersistence.GetPermissionAsync(meUserId, permissionId);
                return Ok(permission.ToGetPermissionResponse());
            }
            catch (ArgumentException)
            {
                return NotFound(new GetUserResponse());
            }
            catch (NullReferenceException)
            {
                return NotFound(new GetUserResponse());
            }
        }

        /// <summary>
        /// Create a new User.
        /// </summary>
        /// <param name="postUser">The PostUserRequest.</param>
        /// <returns>A PostUserResponse.</returns>
        // POST api/users
        [HttpPost]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PostPermissionResponse>> CreatePermission(PostPermissionRequest postPermission)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            try
            {
                var permission = postPermission.ToPermission();
                await permissionPersistence.CreatePermissionAsync(permission);

                // TODO: Is ID populated after saving?
                // TODO : Question : Should this be the whole user object? 
                return Ok(new PostPermissionResponse
                {
                    PermissionId = permission.PermissionId,
                    UserId = permission.UserId,
                    UserRole = permission.UserRole,
                    LocationId = permission.LocationId,
                });
            }
            catch (DocumentClientException)
            {
                return NotFound();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create a new User.
        /// </summary>
        /// <param name="putPermission">The PutPermissionRequest.</param>
        /// <returns>A PutPermissionResponse.</returns>
        // POST api/users
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutPermissionResponse>> UpdatePermission(PutPermissionRequest putPermission)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new BadRequestObjectResult(ModelState);
                }

                var permission = putPermission.ToPermission();
                var updatedPermission = await permissionPersistence.UpdatePermissionAsync(permission);
                return Ok(updatedPermission.ToPutPermissionResponse());
            }
            catch (DocumentClientException)
            {
                return NotFound();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }
    }
}