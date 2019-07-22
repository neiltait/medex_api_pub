using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Models.v1.Account;
using MedicalExaminer.Common.Authorization;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalExaminer.API.Controllers
{
    /// <summary>
    ///     Accounts controller, handler for authentication and token verification.
    /// </summary>
    /// <inheritdoc />
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/auth")]
    [ApiController]
    [Authorize]
    public class AccountController : AuthenticatedBaseController
    {
        private readonly IRolePermissions _rolePermissions;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController" /> class.
        /// </summary>
        /// <param name="logger">Initialise with IMELogger instance.</param>
        /// <param name="mapper">The Mapper.</param>
        /// <param name="usersRetrievalByOktaIdService">User Retrieval By Okta Id Service.</param>
        /// <param name="rolePermissions">Role Permissions.</param>
        public AccountController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser> usersRetrievalByOktaIdService,
            IRolePermissions rolePermissions)
            : base(logger, mapper, usersRetrievalByOktaIdService)
        {
            _rolePermissions = rolePermissions;
        }

        /// <summary>
        ///     Validate Session.
        /// </summary>
        /// <returns>Details about the current user.</returns>
        [HttpPost("validate_session")]
        public async Task<ActionResult<PostValidateSessionResponse>> ValidateSession()
        {
            var meUser = await CurrentUser();

            if (meUser == null)
            {
                return BadRequest(new PostValidateSessionResponse());
            }

            return Ok(new PostValidateSessionResponse
            {
                UserId = meUser.UserId,
                EmailAddress = meUser.Email,
                FirstName = meUser.FirstName,
                LastName = meUser.LastName,
                Role = meUser.Permissions?.Select(p => p.UserRole).ToArray(),
                Permissions = _rolePermissions.PermissionsForRoles(
                    meUser.Permissions?.Select(p => p.UserRole).ToList()),
            });
        }
    }
}