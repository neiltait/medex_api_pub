using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Authorization;
using MedicalExaminer.Common.Authorization;
using MedicalExaminer.Common.Loggers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalExaminer.API.Controllers
{
    /// <summary>
    /// Test Feature Controller.
    /// </summary>
    public class TestFeatureController : AuthorizationBasedController
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

        [AuthorizePermission(Permission.GetLocation)]
        public bool Action()
        {
            return true;
        }
    }
}
