using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Okta.AspNetCore;
using Medical_Examiner_API.Loggers;

namespace Medical_Examiner_API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AccountController : BaseController
    {
        public AccountController(IMELogger logger) : base(logger)
        {
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Challenge(OktaDefaults.MvcAuthenticationScheme);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            return new SignOutResult(new[] { OktaDefaults.MvcAuthenticationScheme, CookieAuthenticationDefaults.AuthenticationScheme });        }
    }
}