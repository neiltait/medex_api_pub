using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Authorization;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Permission = MedicalExaminer.Common.Authorization.Permission;

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

        /// <summary>
        /// Can do Permission to Document
        /// </summary>
        /// <param name="permission">The Permission.</param>
        /// <param name="document">The Document.</param>
        /// <returns>True if can.</returns>
        protected bool CanAsync(Permission permission, ILocationBasedDocument document)
        {
            var authorizationResult = AuthorizationService
                .AuthorizeAsync(User, document, new PermissionRequirement(permission)).Result;

            return authorizationResult.Succeeded;
        }
    }
}
