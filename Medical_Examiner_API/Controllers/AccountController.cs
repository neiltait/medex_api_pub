using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Okta.AspNetCore;
using Medical_Examiner_API.Loggers;
using Medical_Examiner_API.Services;
using Microsoft.AspNetCore.Authorization;

namespace Medical_Examiner_API.Controllers
{
    [Authorize]
    [Route("api/auth")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AccountController(IMELogger logger, IAuthenticationService authenticationService)
            : base(logger)
        {
            _authenticationService = authenticationService;
        }

        [Route("protected")]
        [Authorize]
        public string Protected()
        {
            return "Only if you have a valid token!";
        }

        /// <summary>
        /// Authenticate
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
        /// Test Method
        /// </summary>
        /// <returns>True.</returns>
        [HttpGet("test")]
        public bool TestAuthenticate()
        {
            return true;
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
            return new SignOutResult(new[]
            {
                OktaDefaults.MvcAuthenticationScheme,
                CookieAuthenticationDefaults.AuthenticationScheme
            });
        }
    }
}