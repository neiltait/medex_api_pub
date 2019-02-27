using AutoMapper;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Loggers;
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
        private readonly IAuthenticationService _authenticationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="logger">Initialise with IMELogger instance</param>
        /// <param name="mapper">The Mapper.</param>
        /// <param name="authenticationService">Authentication service.</param>
        public AccountController(IMELogger logger, IMapper mapper, IAuthenticationService authenticationService)
            : base(logger, mapper)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Authenticate without any credentials.
        /// </summary>
        /// <returns>Token.</returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate()
        {
            var token = _authenticationService.Authenticate();

            if (token == null)
            {
                return BadRequest();
            }

            return Ok(token);
        }

        /// <summary>
        /// authenticates an authorised token for use with the data API
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Login()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Challenge(OktaDefaults.MvcAuthenticationScheme);
            }

            return Unauthorized();
        }

        /// <summary>
        /// calls to Okta and Logs out the current authenticated user token
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult Logout()
        {
            return new SignOutResult(new[]
                {OktaDefaults.MvcAuthenticationScheme, CookieAuthenticationDefaults.AuthenticationScheme});
        }
    }
}