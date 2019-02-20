using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Medical_Examiner_API.Extensions.Data;
using Medical_Examiner_API.Loggers;
using Medical_Examiner_API.Models.V1.Users;
using Medical_Examiner_API.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Medical_Examiner_API.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Users Controller
    /// </summary>
    [Route("api/users")]
    [ApiController]
    public class UsersController : BaseController
    {
        /// <summary>
        /// The User Persistance Layer
        /// </summary>
        private readonly IUserPersistence _userPersistence;

        /// <summary>
        /// Initialise a new instance of the Users controller.
        /// </summary>
        /// <param name="userPersistence">The User Persistance.</param>
        /// <param name="logger">The Logger.</param>
        public UsersController(IUserPersistence userPersistence, IMELogger logger)
            : base(logger)
        {
            _userPersistence = userPersistence;
        }

        /// <summary>
        /// Get all Users.
        /// </summary>
        /// <returns>A GetUsersResponse.</returns>
        [HttpGet]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetUsersResponse>> GetAsync()
        {
            var users = await _userPersistence.GetUsersAsync();
            return Ok(new GetUsersResponse()
            {
                Users = users.Select(u => u.ToUserItem())
            });
        }

        /// <summary>
        /// Get a User by its Identifier.
        /// </summary>
        /// <param name="id">The User Identifier.</param>
        /// <returns>A GetUserResponse.</returns>
        [HttpGet("{id}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetUserResponse>> GetAsync(string id)
        {
            if (ModelState.IsValid)
            {
                // Throws excception when not found; maybe should return null and exception
                // handled in logging?
                var user = await _userPersistence.GetUserAsync(id);

                if (user != null)
                {
                    return Ok(user.ToGetUserResponse());
                }

                return NotFound(new GetUserResponse());
            }

            return BadRequest(new GetUserResponse());
        }

        /// <summary>
        /// Create a new User.
        /// </summary>
        /// <param name="postUser">The PostUserRequest.</param>
        /// <returns>A PostUserResponse.</returns>
        [HttpPost]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PostUserResponse>> PostAsync(PostUserRequest postUser)
        {
            if (ModelState.IsValid)
            {
                var user = postUser.ToUser();

                await _userPersistence.SaveUserAsync(user);

                // TODO: Is ID populated after saving?
                return Ok(new PostUserResponse()
                {
                    Id = user.Id,
                });
            }

            return new BadRequestObjectResult(ModelState);
        }

        /// <summary>
        /// Update an existing User.
        /// </summary>
        /// <param name="putUser">A PutUserRequest.</param>
        /// <returns>A PutUserResponse.</returns>
        [HttpPut]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutUserResponse>> PutAsync(PutUserRequest putUser)
        {
            if (ModelState.IsValid)
            {
                var user = putUser.ToUser();

                await _userPersistence.SaveUserAsync(user);

                return Ok(new PutUserResponse());
            }

            return new BadRequestObjectResult(ModelState);
        }

        // GET api/values/seed
        [HttpGet("seed")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<IEnumerable<Models.User>>> Seed()
        {
            Models.User us1 = new Models.User();
            Models.User us2 = new Models.User();
            Models.User us3 = new Models.User();

            us1.FirstName = "Robert";
            us2.FirstName = "Louise";
            us3.FirstName = "Crowbar";

            us1.LastName = "Bobert";
            us2.LastName = "Cheese";
            us3.LastName = "Jones";

            us1.CreatedAt = DateTime.Now;
            us2.CreatedAt = DateTime.Now;
            us3.CreatedAt = DateTime.Now;

            us1.ModifiedAt = DateTime.Now;
            us2.ModifiedAt = DateTime.Now;
            us3.ModifiedAt = DateTime.Now;

            us1.DeletedAt = null;
            us2.DeletedAt = null;
            us3.DeletedAt = null;

            await _userPersistence.SaveUserAsync(us1);
            await _userPersistence.SaveUserAsync(us2);
            await _userPersistence.SaveUserAsync(us3);

            var examinations = await _userPersistence.GetUsersAsync();
            return Ok(examinations);
        }
    }
}
