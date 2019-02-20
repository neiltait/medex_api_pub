using Medical_Examiner_API.Loggers;
using Microsoft.AspNetCore.Mvc;

namespace Medical_Examiner_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : Controller
    {
        public IMELogger Logger { get; set; }
        
        public BaseController(IMELogger logger)
        {
            Logger = logger;
        }

    }
}