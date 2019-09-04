using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Authorization;
using MedicalExaminer.API.Models;
using MedicalExaminer.API.Models.v1.Permissions;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Extensions.Permission;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.Location;
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
    ///     Users Controller
    /// </summary>
    /// <inheritdoc />
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/users")]
    [ApiController]
    [Authorize]
    public class UsersController : AuthorizedBaseController
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
        public UsersController(
            IMELogger logger,
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
        /// Get all Users.
        /// </summary>
        /// <returns>A GetUsersResponse.</returns>
        [HttpGet]
        [AuthorizePermission(Permission.GetUsers)]
        public async Task<ActionResult<GetUsersResponse>> GetUsers()
        {
            try
            {
                var users = await _usersRetrievalService.Handle(new UsersRetrievalQuery(false, null));
                return Ok(new GetUsersResponse
                {
                    Users = users.Select(u => Mapper.Map<UserItem>(u)),
                });
            }
            catch (DocumentClientException)
            {
                return NotFound(new GetUsersResponse());
            }
            catch (ArgumentException)
            {
                return NotFound(new GetUsersResponse());
            }
        }

        /// <summary>
        ///     Get a User by their Identifier.
        /// </summary>
        /// <param name="meUserId">The User Identifier.</param>
        /// <returns>A GetUserResponse.</returns>
        [HttpGet("{meUserId}")]
        [AuthorizePermission(Permission.GetUser)]
        public async Task<ActionResult<GetUserResponse>> GetUser(string meUserId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GetUserResponse());
            }

            try
            {
                var user = await _userRetrievalByIdService.Handle(new UserRetrievalByIdQuery(meUserId));
                if (user == null)
                {
                    return NotFound(new GetUserResponse());
                }

                var gur = new GetUserResponse();
                Mapper.Map(user, gur);

                return Ok(gur);
            }
            catch (ArgumentException)
            {
                return NotFound(new GetUserResponse());
            }
            catch (NullReferenceException)
            {
                return NotFound(new GetUserResponse());
            }
        }

        /// <summary>
        ///     Create a new User.
        /// </summary>
        /// <param name="postUser">The PostUserRequest.</param>
        /// <returns>A PostUserResponse.</returns>
        // POST api/users
        [HttpPost]
        [AuthorizePermission(Permission.InviteUser)]
        public async Task<ActionResult<PostUserResponse>> CreateUser([FromBody] PostUserRequest postUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PostUserResponse());
            }

            try
            {
                var oktaUser = await _oktaClient.Users.GetUserAsync(postUser.Email);

                if (oktaUser == null)
                {
                    var response = new PostUserResponse();
                    response.AddError(nameof(PostUserRequest.Email), "Okta User doesn't exist for email");
                    return BadRequest(response);
                }

                var userToCreate = new MeUser
                {
                    OktaId = oktaUser.Id,
                    FirstName = oktaUser.Profile.FirstName,
                    LastName = oktaUser.Profile.LastName,
                    Email = oktaUser.Profile.Email,
                };

                var currentUser = await CurrentUser();
                var createdUser = await _userCreationService.Handle(new CreateUserQuery(userToCreate, currentUser));
                return Ok(Mapper.Map<PostUserResponse>(createdUser));
            }
            catch (DocumentClientException)
            {
                return NotFound(new PostUserResponse());
            }
            catch (ArgumentException)
            {
                return NotFound(new PostUserResponse());
            }
        }

        /// <summary>
        /// Update a new User.
        /// </summary>
        /// <param name="meUserId">The user id.</param>
        /// <param name="putUser">The PutUserRequest.</param>
        /// <returns>A PutUserResponse.</returns>
        [HttpPut("{meUserId}")]
        [AuthorizePermission(Permission.UpdateUser)]
        public async Task<ActionResult<PutUserResponse>> UpdateUser(string meUserId, [FromBody] PutUserRequest putUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutUserResponse());
            }

            try
            {
                var userUpdateEmail = Mapper.Map<UserUpdateEmail>(putUser);
                userUpdateEmail.UserId = meUserId;
                var currentUser = await CurrentUser();
                var updatedUser = await _userUpdateService.Handle(new UserUpdateQuery(userUpdateEmail, currentUser));
                return Ok(Mapper.Map<PutUserResponse>(updatedUser));
            }
            catch (DocumentClientException)
            {
                return NotFound(new PutUserResponse());
            }
            catch (ArgumentException)
            {
                return NotFound(new PutUserResponse());
            }
        }
    }
}