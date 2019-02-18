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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Medical_Examiner_API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : Controller
    {
        public DocumentClient client = null;
        private IUserPersistence _user_persistence;

        public UsersController(IUserPersistence user_persistence)
        {
            _user_persistence = user_persistence;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.User>>> GetAsync()
        {
            var Users = await _user_persistence.GetUsersAsync();
            return Ok(Users);
        }

        // GET api/values/seed
        [HttpGet("seed")]
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


            await _user_persistence.SaveUserAsync(us1);
            await _user_persistence.SaveUserAsync(us2);
            await _user_persistence.SaveUserAsync(us3);

            var Examinations = await _user_persistence.GetUsersAsync();
            return Ok(Examinations);
        }
    }
}
