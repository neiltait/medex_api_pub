using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Medical_Examiner_API.Loggers;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;

namespace Medical_Examiner_API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IUserPersistence _userPersistence;

        public UsersController(IUserPersistence userPersistence, IMELogger logger) : base(logger)
        {
            _userPersistence = userPersistence;
        }

        // GET api/users
        [HttpGet]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<IEnumerable<MeUser>>> GetUsers()
        {
            var users = await _userPersistence.GetUsersAsync();
            return Ok(users);
        }

        // GET api/users/{user_id}
        [HttpGet("{id}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<Examination>> GetUser(string meUserId)
        {
            try
            {
                return Ok(await _userPersistence.GetUserAsync(meUserId));
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

        // POST api/users
        [HttpPost]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<MeUser>> CreateUser(MeUser meUser)
        {
            try
            {
                return Ok(await _userPersistence.CreateUserAsync(meUser));
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

        // POST api/users
        [HttpPut("{UserId}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<MeUser>> UpdateUser(MeUser meUser)
        {
            try
            {
                return Ok(await _userPersistence.CreateUserAsync(meUser));
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