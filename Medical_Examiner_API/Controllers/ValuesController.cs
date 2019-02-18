using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Medical_Examiner_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2", "Value 3" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public void Delete(int id)
        {
        }
    }
}
