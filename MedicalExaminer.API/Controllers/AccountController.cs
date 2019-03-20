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
    /// Accounts controller, handler for authentication and token verification
    /// </summary>
    [Route("auth")]
    [ApiController]
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly IAsyncQueryHandler<CreateUserQuery, MeUser> _userCreationService;
        private readonly IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> _userRetrievalService;

        /// <summary>
        /// Okta Client.
        /// </summary>
        private readonly OktaClient _oktaClient;

        /// <summary>
        /// The User Persistence Layer
        /// </summary>
        private readonly IUserPersistence _userPersistence;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="logger">Initialise with IMELogger instance</param>
        /// <param name="mapper">The Mapper.</param>
        /// <param name="oktaClient">Okta client.</param>
        /// <param name="userPersistence">User persistance.</param>
        /// <param name="userCreationService"></param>
        /// <param name="userRetrievalService"></param>
        public AccountController(IMELogger logger, 
            IMapper mapper, 
            OktaClient oktaClient, 
            IUserPersistence userPersistence,
            IAsyncQueryHandler<CreateUserQuery, MeUser> userCreationService,
            IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> userRetrievalService)
            : base(logger, mapper)
        {
            _oktaClient = oktaClient;
            _userPersistence = userPersistence;
            _userCreationService = userCreationService;
            _userRetrievalService = userRetrievalService;
        }

        /// <summary>
        /// Validate Session
        /// </summary>
        /// <returns>Details about the current user.</returns>
        [HttpPost("validate-session")]
        public async Task<PostValidateSessionResponse> ValidateSession()
        {
            // Look up their email in the claims
            var emailAddress = User.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).First();

            // Get everything that Okta knows about this user
            var oktaUser = await _oktaClient.Users.GetUserAsync(emailAddress);

            // Try and look them up in our database
            MeUser meUser = await GetUser(emailAddress);

            // Create the user if it doesn't already exist
            if (meUser == null)
            {
                var createdMeUser = await CreateUser(new MeUser()
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
                // TODO: Decide on an appropriate way of responding to not valid
                throw new Exception("Failed to create user");
            }

            return new PostValidateSessionResponse()
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
                var user = await _userRetrievalService.Handle(new UserRetrievalByEmailQuery(emailAddress));

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
                var createdUser = await _userCreationService.Handle(new CreateUserQuery(toCreate));

                return createdUser;
            }
            catch (DocumentClientException)
            {
                return null;
            }
        }
    }
}
