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
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
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
    public class AccountController : BaseController
    {
        /// <summary>
        ///     Okta Client.
        /// </summary>
        private readonly OktaClient oktaClient;

        private readonly IAsyncQueryHandler<CreateUserQuery, MeUser> userCreationService;

        /// <summary>
        ///     The User Persistence Layer.
        /// </summary>
        private readonly IUserPersistence userPersistence;

        private readonly IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> userRetrievalService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AccountController" /> class.
        /// </summary>
        /// <param name="logger">Initialise with IMELogger instance.</param>
        /// <param name="mapper">The Mapper.</param>
        /// <param name="oktaClient">Okta client.</param>
        /// <param name="userPersistence">User persistence.</param>
        /// <param name="userCreationService">User Creation Service.</param>
        /// <param name="userRetrievalService">User Retrieval Service.</param>
        public AccountController(
            IMELogger logger,
            IMapper mapper,
            OktaClient oktaClient,
            IUserPersistence userPersistence,
            IAsyncQueryHandler<CreateUserQuery, MeUser> userCreationService,
            IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> userRetrievalService)
            : base(logger, mapper)
        {
            this.oktaClient = oktaClient;
            this.userPersistence = userPersistence;
            this.userCreationService = userCreationService;
            this.userRetrievalService = userRetrievalService;
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

            // Get everything that Okta knows about this user
            var oktaUser = await oktaClient.Users.GetUserAsync(emailAddress);

            // Try and look them up in our database
            var meUser = await GetUser(emailAddress);

            // Create the user if it doesn't already exist
            if (meUser == null)
            {
                var createdMeUser = await CreateUser(new MeUser
                {
                    FirstName = oktaUser.Profile.FirstName,
                    LastName = oktaUser.Profile.LastName,
                    Email = oktaUser.Profile.Email,
                    UserRole = UserRoles.ServiceOwner,
                    LastModifiedBy = "whodunit",
                    ModifiedAt = DateTimeOffset.Now,
                    CreatedAt = DateTimeOffset.Now,
                });
                meUser = createdMeUser;
            }

            if (meUser == null)
            {
                throw new Exception("Failed to create user");
            }

            return new PostValidateSessionResponse
            {
                UserId = meUser.UserId,
                EmailAddress = meUser.Email,
                FirstName = meUser.FirstName,
                LastName = meUser.LastName,
            };
        }

        // TODO: Candidates for servicing / facading this functionality now
        private async Task<MeUser> GetUser(string emailAddress)
        {
            try
            {
                var user = await userRetrievalService.Handle(new UserRetrievalByEmailQuery(emailAddress));

                return user;
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        private async Task<MeUser> CreateUser(MeUser toCreate)
        {
            try
            {
                var createdUser = await userCreationService.Handle(new CreateUserQuery(toCreate));

                return createdUser;
            }
            catch (DocumentClientException)
            {
                return null;
            }
        }
    }
}