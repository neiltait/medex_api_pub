using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Medical_Examiner_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public ILogger<BaseController> Logger { get; set; }

        public BaseController(ILogger<BaseController> logger)
        {
            Logger = logger;
        }
    }
}