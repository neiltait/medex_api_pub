using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Authorization;
using MedicalExaminer.API.Helpers;
using MedicalExaminer.API.Models;
using MedicalExaminer.API.Models.v1.Account;
using MedicalExaminer.Common.Authorization;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Extensions.Options;
using Okta.Sdk;

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
        private readonly IAsyncQueryHandler<CreateUserQuery, MeUser> _userCreationService;

        private readonly IAsyncQueryHandler<UsersUpdateOktaTokenQuery, MeUser> _userUpdateOktaTokenService;

        private readonly IAsyncQueryHandler<UserUpdateOktaQuery, MeUser> _userUpdateOktaService;

        private readonly IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> _userRetrievalByEmailService;

        private readonly IRolePermissions _rolePermissions;

        /// <summary>
        ///     Okta Client.
        /// </summary>
        private readonly OktaClient _oktaClient;

        /// <summary>
        /// Number of minutes an okta token is cached for before refer back to Okta
        /// </summary>
        private readonly int _oktaTokenExpiryMinutes;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController" /> class.
        /// </summary>
        /// <param name="logger">Initialise with IMELogger instance.</param>
        /// <param name="mapper">The Mapper.</param>
        /// <param name="oktaClient">Okta client.</param>
        /// <param name="userCreationService">User Creation Service.</param>
        /// <param name="usersRetrievalByOktaIdService">User Retrieval By Okta Id Service.</param>
        /// <param name="userUpdateOktaTokenService">User Update Okta Token Service.</param>
        /// <param name="userRetrievalByEmailService">User Retrieval by Email Service.</param>
        /// <param name="userUpdateOktaService">User Update Okta Service.</param>
        /// <param name="oktaSettings">Okta Settings.</param>
        /// <param name="rolePermissions">Role Permissions.</param>
        public AccountController(
            IMELogger logger,
            IMapper mapper,
            OktaClient oktaClient,
            IAsyncQueryHandler<CreateUserQuery, MeUser> userCreationService,
            IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser> usersRetrievalByOktaIdService,
            IAsyncQueryHandler<UsersUpdateOktaTokenQuery, MeUser> userUpdateOktaTokenService,
            IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> userRetrievalByEmailService,
            IAsyncQueryHandler<UserUpdateOktaQuery, MeUser> userUpdateOktaService,
            IOptions<OktaSettings> oktaSettings,
            IRolePermissions rolePermissions)
            : base(logger, mapper, usersRetrievalByOktaIdService)
        {
            _oktaClient = oktaClient;
            _userCreationService = userCreationService;
            _userUpdateOktaTokenService = userUpdateOktaTokenService;
            _userRetrievalByEmailService = userRetrievalByEmailService;
            _userUpdateOktaService = userUpdateOktaService;
            _rolePermissions = rolePermissions;
            _oktaTokenExpiryMinutes = int.Parse(oktaSettings.Value.LocalTokenExpiryTimeMinutes);
        }

        /// <summary>
        ///     Validate Session.
        /// </summary>
        /// <returns>Details about the current user.</returns>
        [HttpPost("validate_session")]
        public async Task<PostValidateSessionResponse> ValidateSession()
        {
            var oktaToken = OktaTokenParser.ParseHttpRequestAuthorisation(
                Request
                    .Headers["Authorization"]
                    .ToString());

            // Try and look them up in our database
            var meUser = await CurrentUser();

            // Create the user if it doesn't already exist
            if (meUser == null)
            {
                var oktaId = User.Claims.Where(c => c.Type == MEClaimTypes.OktaUserId).Select(c => c.Value).First();

                meUser = await CreateNewUser(oktaId, oktaToken);
            }

            if (meUser == null)
            {
                throw new Exception("Failed to create user");
            }

            //// Reset token if it has changed
            //if (meUser.OktaToken == null || meUser.OktaToken != oktaToken)
            //{
            //    var expiryTime = DateTime.Now.AddMinutes(_oktaTokenExpiryMinutes);

            //    meUser.OktaToken = oktaToken;
            //    meUser.OktaTokenExpiry = expiryTime;

            //    meUser = await UpdateUserOktaToken(meUser);
            //}

            return new PostValidateSessionResponse
            {
                UserId = meUser.UserId,
                EmailAddress = meUser.Email,
                FirstName = meUser.FirstName,
                LastName = meUser.LastName,
                Role = meUser.Permissions?.Select(p => p.UserRole).ToArray(),
                Permissions = _rolePermissions.PermissionsForRoles(
                    meUser.Permissions?.Select(p => p.UserRole).ToList()),
            };
        }

        /// <summary>
        /// Create new user with details from Okta if does not already exist
        /// </summary>
        /// <param name="oktaId">Okta ID of user from token.</param>
        /// <param name="oktaToken">Okta token</param>
        /// <returns>UserToCreate</returns>
        /// <remarks>virtual so that it can be unit tested via proxy class</remarks>
        protected virtual async Task<MeUser> CreateNewUser(string oktaId, string oktaToken)
        {
            // Get everything that Okta knows about this user
            var oktaUser = await _oktaClient.Users.GetUserAsync(oktaId);

            // See if they exist using their email
            var existingUser = await _userRetrievalByEmailService.Handle(new UserRetrievalByEmailQuery(oktaUser.Profile.Email));

            if (existingUser != null)
            {
                var updatedUser = await _userUpdateOktaService.Handle(new UserUpdateOktaQuery(existingUser.UserId, oktaId));

                return updatedUser;
            }
            else
            {
                var expiryTime = DateTime.Now.AddMinutes(_oktaTokenExpiryMinutes);

                var createdMeUser = await CreateUser(new MeUser
                {
                    FirstName = oktaUser.Profile.FirstName,
                    LastName = oktaUser.Profile.LastName,
                    Email = oktaUser.Profile.Email,
                    LastModifiedBy = null,
                    ModifiedAt = DateTimeOffset.Now,
                    CreatedAt = DateTimeOffset.Now,
                    OktaId = oktaUser.Id,
                    OktaToken = oktaToken,
                    OktaTokenExpiry = expiryTime
                });
                var meUser = createdMeUser;

                return meUser;
            }
        }

        private async Task<MeUser> CreateUser(MeUser toCreate)
        {
            try
            {
                // The user is being created so we don't know what their ID is yet. Pass an empty one for now.
                var currentUser = new MeUser()
                {
                    UserId = Guid.Empty.ToString(),
                };
                var createdUser = await _userCreationService.Handle(new CreateUserQuery(toCreate, currentUser));

                return createdUser;
            }
            catch (DocumentClientException)
            {
                return null;
            }
        }

        private async Task<MeUser> UpdateUserOktaToken(MeUser toUpdate)
        {
            try
            {
                //  var updatedUser = await _userUpdateOktaTokenService.Handle(new UsersUpdateOktaTokenQuery(toUpdate));

                return null;// updatedUser;
            }
            catch (DocumentClientException)
            {
                return null;
            }
        }
    }
}