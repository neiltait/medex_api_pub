using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Authorization;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Queries.MELogger;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Okta.Sdk;
using Permission = MedicalExaminer.Common.Authorization.Permission;

namespace MedicalExaminer.API.Controllers
{
    /// <summary>
    /// Profile Controller
    /// </summary>
    /// <inheritdoc />
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/profile")]
    [ApiController]
    [Authorize]
    public class ProfileController : AuthorizedBaseController
    {
        /// <summary>
        /// Locations Parents Service.
        /// </summary>
        private readonly
            IAsyncQueryHandler<LocationsParentsQuery, IDictionary<string, IEnumerable<Location>>>
            _locationsParentsService;

        /// <summary>
        /// User Creation Service
        /// </summary>
        private readonly IAsyncQueryHandler<CreateUserQuery, MeUser> _userCreationService;

        /// <summary>
        /// User Retrieval by Id Service.
        /// </summary>
        private readonly IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser> _userRetrievalByIdService;

        /// <summary>
        /// Users Retrieval Service
        /// </summary>
        private readonly IAsyncQueryHandler<UsersRetrievalQuery, IEnumerable<MeUser>> _usersRetrievalService;

        /// <summary>
        /// User Update Service.
        /// </summary>
        private readonly IAsyncQueryHandler<UserUpdateQuery, MeUser> _userUpdateService;

        /// <summary>
        /// Locations Retrieval Service.
        /// </summary>
        private readonly IAsyncQueryHandler<LocationsRetrievalByQuery, IEnumerable<Location>> _locationsRetrievalService;

        /// <summary>
        /// Okta Client.
        /// </summary>
        private readonly IOktaClient _oktaClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="logger">The Logger.</param>
        /// <param name="mapper">The Mapper.</param>
        /// <param name="usersRetrievalByOktaIdService">User Retrieval By Okta Id Service.</param>
        /// <param name="authorizationService">Authorization Service.</param>
        /// <param name="permissionService">Permission Service.</param>
        /// <param name="userCreationService">User creation service.</param>
        /// <param name="userRetrievalByIdService">User retrieval service.</param>
        /// <param name="usersRetrievalService">Users retrieval service.</param>
        /// <param name="userUpdateService">The userToCreate update service</param>
        /// <param name="locationsRetrievalService">Locations Retrieval Service.</param>
        /// <param name="oktaClient">Okta client.</param>
        public ProfileController(
            IAsyncQueryHandler<CreateMELoggerQuery, LogMessageActionDefault> logger,
            IMapper mapper,
            IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser> usersRetrievalByOktaIdService,
            IAuthorizationService authorizationService,
            IPermissionService permissionService,
            IAsyncQueryHandler<CreateUserQuery, MeUser> userCreationService,
            IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser> userRetrievalByIdService,
            IAsyncQueryHandler<UsersRetrievalQuery, IEnumerable<MeUser>> usersRetrievalService,
            IAsyncQueryHandler<UserUpdateQuery, MeUser> userUpdateService,
            IAsyncQueryHandler<LocationsRetrievalByQuery, IEnumerable<Location>> locationsRetrievalService,
            IOktaClient oktaClient,
            IAsyncQueryHandler<LocationsParentsQuery, IDictionary<string, IEnumerable<Location>>> locationsParentsService)
            : base(logger, mapper, usersRetrievalByOktaIdService, authorizationService, permissionService)
        {
            _userCreationService = userCreationService;
            _userRetrievalByIdService = userRetrievalByIdService;
            _usersRetrievalService = usersRetrievalService;
            _userUpdateService = userUpdateService;
            _locationsRetrievalService = locationsRetrievalService;
            _oktaClient = oktaClient;
            _locationsParentsService = locationsParentsService;
        }

        /// <summary>
        /// Get the current user profile.
        /// </summary>
        /// <returns>A GetUserResponse.</returns>
        [HttpGet]
        [AuthorizePermission(Permission.GetProfile)]
        public async Task<ActionResult<GetProfileResponse>> GetProfile()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GetProfileResponse());
            }

            var currentUser = await CurrentUser();

            try
            {
                var user = await _userRetrievalByIdService.Handle(new UserRetrievalByIdQuery(currentUser.UserId));
                if (user == null)
                {
                    return NotFound(new GetProfileResponse());
                }

                var gur = new GetProfileResponse();
                Mapper.Map(user, gur);

                return Ok(gur);
            }
            catch (ArgumentException)
            {
                return NotFound(new GetProfileResponse());
            }
            catch (NullReferenceException)
            {
                return NotFound(new GetProfileResponse());
            }
        }

        /// <summary>
        /// Update the current user profile.
        /// </summary>
        /// <param name="putUser">The PutUserRequest.</param>
        /// <returns>A PutUserResponse.</returns>
        [HttpPut]
        [AuthorizePermission(Permission.UpdateProfile)]
        public async Task<ActionResult<PutProfileResponse>> UpdateProfile([FromBody] PutProfileRequest putUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutProfileResponse());
            }

            var currentUser = await CurrentUser();

            try
            {
                var userUpdateEmail = Mapper.Map<UserUpdateProfile>(putUser);
                userUpdateEmail.UserId = currentUser.UserId;
                var updatedUser = await _userUpdateService.Handle(new UserUpdateQuery(userUpdateEmail, currentUser));
                return Ok(Mapper.Map<PutProfileResponse>(updatedUser));
            }
            catch (DocumentClientException)
            {
                return NotFound(new PutProfileResponse());
            }
            catch (ArgumentException)
            {
                return NotFound(new PutProfileResponse());
            }
        }
    }
}