using Medical_Examiner_API.Loggers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Okta.AspNetCore;

namespace Medical_Examiner_API.Controllers
{
    /// <summary>
    /// Account Controller
    /// </summary>
    [Route("auth")]
    [ApiController]
    public class AccountController : BaseController
    {
        /// <summary>
        /// Account Controller Initializer
        /// </summary>
        /// <param name="logger"></param>
        public AccountController(IMELogger logger) 
            : base(logger)
        {
        }

        /// <summary>
        /// Action to pass authentication token to data API and verify
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
        /// Signs the user out 
        /// </summary>
        /// <returns>SignOutResult</returns>
        [HttpGet]
        public IActionResult Logout()
        {
            return new SignOutResult(new[]
                {OktaDefaults.MvcAuthenticationScheme, CookieAuthenticationDefaults.AuthenticationScheme});
        }
    }
}