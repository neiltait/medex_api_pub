using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Authorization;
using MedicalExaminer.Common.Queries.MELogger;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Controllers
{
    /// <summary>
    /// Authenticated Base Controller.
    /// </summary>
    /// <remarks>Provides access to the Current User record.</remarks>
    public abstract class AuthenticatedBaseController : BaseController
    {
        private readonly IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser> _usersRetrievalByOktaIdService;

        /// <summary>
        /// Initialise a new instance of <see cref="AuthenticatedBaseController"/>.
        /// </summary>
        /// <param name="logger">The Logger.</param>
        /// <param name="mapper">The Mapper.</param>
        /// <param name="usersRetrievalByOktaIdService">User Retrieval By Okta Id Service.</param>
        protected AuthenticatedBaseController(
            IAsyncQueryHandler<CreateMELoggerQuery, LogMessageActionDefault> logger,
            IMapper mapper,
            IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser> usersRetrievalByOktaIdService)
            : base(logger, mapper)
        {
            _usersRetrievalByOktaIdService = usersRetrievalByOktaIdService;
        }

        /// <summary>
        /// Get the Current User.
        /// </summary>
        /// <returns>Current User.</returns>
        protected async Task<MeUser> CurrentUser()
        {
            var oktaUserId = User.Claims.Where(c => c.Type == MEClaimTypes.OktaUserId).Select(c => c.Value).First();

            try
            {
                var user = await _usersRetrievalByOktaIdService.Handle(new UserRetrievalByOktaIdQuery(oktaUserId));

                return user;
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
    }
}
