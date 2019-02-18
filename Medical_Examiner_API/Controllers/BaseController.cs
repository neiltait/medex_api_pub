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

    }
}