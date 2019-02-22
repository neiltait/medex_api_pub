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
    [Route("users")]
    [ApiController]
    public class UsersController : BaseController
    {
        /// <summary>
        /// The User Persistence Layer
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
        public async Task<ActionResult<GetUsersResponse>> GetUsers()
        {
            try
            {
                var users = await _userPersistence.GetUsersAsync();
                
                return Ok(new GetUsersResponse
                {
                    Users = users.Select(u => u.ToUserItem()),
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
        /// Get a User by its Identifier.
        /// </summary>
        /// <param name="meUserId">The User Identifier.</param>
        /// <returns>A GetUserResponse.</returns>
        // GET api/users/{user_id}
        [HttpGet("{id}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetUserResponse>> GetUser(string meUserId)
        {
            if (!ModelState.IsValid) return BadRequest(new GetUserResponse());
            
            // Throws exception when not found; maybe should return null and exception
            // handled in logging?
            try
            {
                var user = await _userPersistence.GetUserAsync(meUserId);
                return Ok(user.ToGetUserResponse());
            }
            catch (ArgumentException)
            {
                return NotFound(new GetUserResponse());
            }
            catch (NullReferenceException)
            {
                return NotFound(new GetUserResponse());
            }

            return BadRequest(new GetUserResponse());
        }
        
        /// <summary>
        /// Create a new User.
        /// </summary>
        /// <param name="postUser">The PostUserRequest.</param>
        /// <returns>A PostUserResponse.</returns>
       // POST api/users
        [HttpPost]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PostUserResponse>> CreateUser(PostUserRequest postUser)
        {
            try
            {
                if (!ModelState.IsValid) return new BadRequestObjectResult(ModelState);

                var user = postUser.ToUser();
                await _userPersistence.CreateUserAsync(user);

                // TODO: Is ID populated after saving?
                // TODO : Question : Should this be the whole user object? 
                return Ok(new PostUserResponse
                {
                    UserId = user.UserId
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
        /// <param name="putUser">The PutUserRequest.</param>
        /// <returns>A PutUserResponse.</returns>
        // POST api/users
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutUserResponse>> UpdateUser(PutUserRequest putUser)
        {
            try
            {
                if (!ModelState.IsValid) return new BadRequestObjectResult(ModelState);
                var user = putUser.ToUser();
                var updated_user = await _userPersistence.UpdateUserAsync(user);
                return Ok(updated_user.ToPutUserResponse());
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