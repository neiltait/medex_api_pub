using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.Common.Loggers;
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
        private readonly IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> _userRetrievalService;

        /// <summary>
        /// Initialise a new instance of <see cref="AuthenticatedBaseController"/>.
        /// </summary>
        /// <param name="logger">The Logger.</param>
        /// <param name="mapper">The Mapper.</param>
        /// <param name="userRetrievalService">User Retrieval Service.</param>
        protected AuthenticatedBaseController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> userRetrievalService)
            : base(logger, mapper)
        {
            _userRetrievalService = userRetrievalService;
        }

        /// <summary>
        /// Get the Current User.
        /// </summary>
        /// <returns>Current User.</returns>
        protected async Task<MeUser> CurrentUser()
        {
            var emailAddress = User.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).First();

            try
            {
                var user = await _userRetrievalService.Handle(new UserRetrievalByEmailQuery(emailAddress));

                return user;
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
    }
}
