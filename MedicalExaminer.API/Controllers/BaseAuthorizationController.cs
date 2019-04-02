using AutoMapper;
using MedicalExaminer.Common.Loggers;
using Microsoft.AspNetCore.Authorization;

namespace MedicalExaminer.API.Controllers
{
    /// <summary>
    /// Base Authorization Controller.
    /// </summary>
    public abstract class BaseAuthorizationController : BaseController
    {
        /// <summary>
        /// Authorization Service.
        /// </summary>
        protected IAuthorizationService AuthorizationService { get; }

        /// <summary>
        /// Authorization Based Controller.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="authorizationService"></param>
        protected BaseAuthorizationController(
            IMELogger logger,
            IMapper mapper,
            IAuthorizationService authorizationService)
            : base(logger, mapper)
        {
            AuthorizationService = authorizationService;
        }
    }
}
