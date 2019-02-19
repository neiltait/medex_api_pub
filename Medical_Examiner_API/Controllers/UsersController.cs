using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using Medical_Examiner_API.Loggers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Medical_Examiner_API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : BaseController
    {
        private IUserPersistence _user_persistence;

        public UsersController(IUserPersistence user_persistence, IMELogger logger) : base(logger)
        {
            _user_persistence = user_persistence;
        }

        // GET api/users
        [HttpGet]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<IEnumerable<Models.User>>> GetUsers()
        {
            var Users = await _user_persistence.GetUsersAsync();
            return Ok(Users);
        }

        // GET api/users/{user_id}
        [HttpGet("{id}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<Examination>> GetUser(string user_id)
        {
            try
            {
                return Ok(await _user_persistence.GetUserAsync(user_id));
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

        // PUT api/users
        [HttpPut]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<Examination>> PutUser(Models.User user)
        {
            try
            {
                return Ok(await _user_persistence.CreateUserAsync(user));
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
