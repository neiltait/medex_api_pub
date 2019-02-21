using AutoMapper;
using Medical_Examiner_API.Loggers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Okta.AspNetCore;

namespace Medical_Examiner_API.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AccountController : BaseController
    {
        public AccountController(IMELogger logger, IMapper mapper)
            : base(logger, mapper)
        {
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            if (!HttpContext.User.Identity.IsAuthenticated) return Challenge(OktaDefaults.MvcAuthenticationScheme);

            return Unauthorized();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            return new SignOutResult(new[]
                {OktaDefaults.MvcAuthenticationScheme, CookieAuthenticationDefaults.AuthenticationScheme});
        }
    }
}