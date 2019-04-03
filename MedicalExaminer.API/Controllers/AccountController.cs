using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Models.v1.Account;
using MedicalExaminer.Common;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.API.Helpers;
using MedicalExaminer.API.Models;
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
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/auth")]
    [ApiController]
    [Authorize]
    public class AccountController : AuthenticatedBaseController
    {
        private readonly IAsyncQueryHandler<CreateUserQuery, MeUser> _userCreationService;

        private readonly IAsyncQueryHandler<UsersUpdateOktaTokenQuery, MeUser> _userUpdateOktaTokenService;

        /// <summary>
        ///     Okta Client.
        /// </summary>
        private readonly OktaClient _oktaClient;

        /// <summary>
        /// Number of minutes an okta token is cached for before refer back to Okta
        /// </summary>
        private readonly int _oktaTokenExpiryMinutes;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AccountController" /> class.
        /// </summary>
        /// <param name="logger">Initialise with IMELogger instance.</param>
        /// <param name="mapper">The Mapper.</param>
        /// <param name="oktaClient">Okta client.</param>
        /// <param name="userPersistence">User persistence.</param>
        /// <param name="userCreationService">User Creation Service.</param>
        /// <param name="usersRetrievalByEmailService">User Retrieval By Email Service.</param>
        /// <param name="userUpdateOktaTokenService">User Update Okta Token Service</param>
        /// <param name="oktaSettings">Okta Settings</param>
        public AccountController(
            IMELogger logger,
            IMapper mapper,
            OktaClient oktaClient,
            IUserPersistence userPersistence,
            IAsyncQueryHandler<CreateUserQuery, MeUser> userCreationService,
            IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> usersRetrievalByEmailService,
            IAsyncQueryHandler<UsersUpdateOktaTokenQuery, MeUser> userUpdateOktaTokenService,
            IOptions<OktaSettings> oktaSettings)
            : base(logger, mapper, usersRetrievalByEmailService)
        {
            _oktaClient = oktaClient;
            _userCreationService = userCreationService;
            _userUpdateOktaTokenService = userUpdateOktaTokenService;
            _oktaTokenExpiryMinutes = Int32.Parse(oktaSettings.Value.LocalTokenExpiryTimeMinutes);
        }

        /// <summary>
        ///     Validate Session.
        /// </summary>
        /// <returns>Details about the current user.</returns>
        [HttpPost("validate_session")]
        public async Task<PostValidateSessionResponse> ValidateSession()
        {
  
            // Look up their email in the claims
            var emailAddress = User.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).First();

            var oktaToken =
                OktaTokenParser.ParseHttpRequestAuthorisation(Request.Headers["Authorization"].ToString());

            // Try and look them up in our database
            var meUser = await CurrentUser();

            // Create the user if it doesn't already exist
            if (meUser == null)
            {
                // Get everything that Okta knows about this user
                var oktaUser = await _oktaClient.Users.GetUserAsync(emailAddress);

                var expiryTime = DateTime.Now.AddMinutes(_oktaTokenExpiryMinutes);

                var createdMeUser = await CreateUser(new MeUser
                {
                    FirstName = oktaUser.Profile.FirstName,
                    LastName = oktaUser.Profile.LastName,
                    Email = oktaUser.Profile.Email,
                    // TODO: Default to null?
                    LastModifiedBy = "whodunit",
                    ModifiedAt = DateTimeOffset.Now,
                    CreatedAt = DateTimeOffset.Now,
                    OktaToken = oktaToken,
                    OktaTokenExpiry = expiryTime
                });
                meUser = createdMeUser;
            }

            if (meUser == null)
            {
                throw new Exception("Failed to create user");
            }

            //Reset token if it has changed
            if (meUser.OktaToken == null || meUser.OktaToken != oktaToken)
            {
                var expiryTime = DateTime.Now.AddMinutes(_oktaTokenExpiryMinutes);

                meUser.OktaToken = oktaToken;
                meUser.OktaTokenExpiry = expiryTime;

                meUser = await UpdateUserOktaToken(meUser);

            }

            return new PostValidateSessionResponse
            {
                UserId = meUser.UserId,
                EmailAddress = meUser.Email,
                FirstName = meUser.FirstName,
                LastName = meUser.LastName,
            };
        }

        private async Task<MeUser> CreateUser(MeUser toCreate)
        {
            try
            {
                var createdUser = await _userCreationService.Handle(new CreateUserQuery(toCreate));

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
                var updatedUser = await _userUpdateOktaTokenService.Handle(new UsersUpdateOktaTokenQuery(toUpdate));

                return updatedUser;
            }
            catch (DocumentClientException)
            {
                return null;
            }
        }
    }
}