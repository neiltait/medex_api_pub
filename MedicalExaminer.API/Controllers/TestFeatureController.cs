using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Authorization;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Permission = MedicalExaminer.Common.Authorization.Permission;

namespace MedicalExaminer.API.Controllers
{
    /// <summary>
    /// Test Feature Controller.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/test")]
    [ApiController]
    [Authorize]
    public class TestFeatureController : BaseAuthorizationController
    {
        /// <summary>
        /// Initialise a new instance of the <see cref="TestFeatureController"/>.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="mapper">Mapper.</param>
        /// <param name="authorizationService">Authorization Service.</param>
        public TestFeatureController(IMELogger logger, IMapper mapper, IAuthorizationService authorizationService)
            : base(logger, mapper, authorizationService)
        {
        }

        [HttpGet("action")]
        [AuthorizePermission(Permission.GetLocation)]
        public bool Action()
        {
            return true;
        }

        [HttpGet("actionInside")]
        public async Task<bool> ActionInside()
        {
            var document = new Examination()
            {

            };

            var authorizationResult = await AuthorizationService.AuthorizeAsync(User, document, new PermissionRequirement(Permission.GetLocation));

            if (authorizationResult.Succeeded)
            {
                return true;
            }

            return false;
        }

        [HttpGet("actionInside2")]
        public async Task<bool> ActionInside2()
        {
            var document = new Examination()
            {

            };

            var authorizationResult = await AuthorizationService.AuthorizeAsync(User, document, new PermissionRequirement(Permission.CreateExamination));

            if (authorizationResult.Succeeded)
            {
                return true;
            }

            return false;
        }
    }
}
