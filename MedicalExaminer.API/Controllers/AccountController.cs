using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Models;
using MedicalExaminer.API.Models.v1.Account;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Extensions.Options;
using Okta.AspNetCore;
using Okta.Sdk;
using Okta.Sdk.Configuration;

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
        public AccountController(IMELogger logger, IMapper mapper, OktaClient oktaClient, IUserPersistence userPersistence)
            : base(logger, mapper)
        {
            _oktaClient = oktaClient;
            _userPersistence = userPersistence;
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
            MeUser meUser = null;
            try
            {
                meUser = await GetUser(emailAddress);
            }
            catch
            {
                // TODO: Not implemented for now
            }

            // Create the user if it doesn't already exist
            if (meUser == null)
            {
                var createdMeUser = await CreateUser(new MeUser()
                {
                    FirstName = oktaUser.Profile.FirstName,
                    LastName = oktaUser.Profile.LastName,
                    Email = oktaUser.Profile.Email,
                });
                meUser = createdMeUser;
            }

            if (meUser == null)
            {
                // TODO: Create an exception for this response
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
                var user = await _userPersistence.GetUserByEmailAddressAsync(emailAddress);
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
                var createdUser = await _userPersistence.CreateUserAsync(toCreate);
                return createdUser;
            }
            catch (DocumentClientException)
            {
                return null;
            }
        }
    }
}
