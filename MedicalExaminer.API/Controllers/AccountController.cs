using AutoMapper;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Loggers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Okta.AspNetCore;

namespace MedicalExaminer.API.Controllers
{
    /// <summary>
    /// Accounts controller, handler for authentication and token verification
    /// </summary>
    [Route("auth")]
    [ApiController]
    [Authorize]
    public class AccountController : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="logger">Initialise with IMELogger instance</param>
        /// <param name="mapper">The Mapper.</param>
        /// <param name="authenticationService">Authentication service.</param>
        public AccountController(IMELogger logger, IMapper mapper)
            : base(logger, mapper)
        {
        }

        [HttpPost("validate-session")]
        [AllowAnonymous]
        public dynamic ValidateSession()
        {
            return new
            {
                user_id = "user_id",
                first_name = "first_name",
                last_name = "last_name",
                email_address = "email",
            };
        }

        [HttpGet("get-session")]
        [AllowAnonymous]
        public bool GetSession()
        {
            return true;
        }
    }
}
