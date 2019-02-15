using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Medical_Examiner_API.Loggers;

namespace Medical_Examiner_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        public IMELogger Logger { get; set; }

        public BaseController(IMELogger logger)
        {
            Logger = logger;
        }

        // GET: api/Base
        [HttpGet]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public virtual IEnumerable<string> Get()
        {
            return new string[] { "Basevalue1", "Basevalue2" };
        }

        // GET: api/Base/5
        [HttpGet("{id}", Name = "Get")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public virtual string Get(int id)
        {
            return "Base value";
        }

        // POST: api/Base
        [HttpPost]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public virtual void Post([FromBody] string value)
        {
        }

        // PUT: api/Base/5
        [HttpPut("{id}")]
        public virtual void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public virtual void Delete(int id)
        {
        }
    }
}