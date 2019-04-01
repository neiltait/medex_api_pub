using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.Common.Loggers;
using Microsoft.AspNetCore.Authorization;

namespace MedicalExaminer.API.Controllers
{
    /// <summary>
    /// Authorization Based Controller.
    /// </summary>
    public abstract class AuthorizationBasedController : BaseController
    {
        /// <summary>
        /// Authorization Service.
        /// </summary>
        private readonly IAuthorizationService _authorizationService;

        /// <summary>
        /// Authorization Based Controller.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="authorizationService"></param>
        public AuthorizationBasedController(
            IMELogger logger,
            IMapper mapper,
            IAuthorizationService authorizationService)
        :base(logger, mapper)
        {
            _authorizationService = authorizationService;
        }
    }
}
