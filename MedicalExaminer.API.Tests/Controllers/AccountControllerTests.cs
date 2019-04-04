using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Models;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Okta.Sdk;
using Okta.Sdk.Configuration;
using Xunit;


namespace MedicalExaminer.API.Tests.Controllers
{
    public class AccountControllerTests
    {
        private readonly Mock<IMELogger> _mockLogger;

        private readonly Mock<IMapper> _mockMapper;

        private readonly Mock<IAsyncQueryHandler<CreateUserQuery, MeUser>> _mockUserCreationService;

        private readonly Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>> _mockUsersRetrievalByEmailService;

        private readonly Mock<IAsyncQueryHandler<UsersUpdateOktaTokenQuery, MeUser>> _mockUserUpdateOktaTokenService;

        private readonly Mock<IOptions<OktaSettings>> _mockOktaSettings;

        private AccountController _accountController;

        public AccountControllerTests()
        {
            _mockLogger = new Mock<IMELogger>();
            _mockMapper = new Mock<IMapper>();
            var oktaSettings = Options.Create(new OktaSettings());
            oktaSettings.Value.LocalTokenExpiryTimeMinutes = "30";

            var oktaClientConfiguration = new OktaClientConfiguration
            {
                OktaDomain = "https://xxx-123456.test.com",
                Token = "Token",
            };
            _mockUserCreationService = new Mock<IAsyncQueryHandler<CreateUserQuery, MeUser>>();
            _mockUsersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            _mockUserUpdateOktaTokenService = new Mock<IAsyncQueryHandler<UsersUpdateOktaTokenQuery, MeUser>>();


            var claims = new List<Claim>();
            var claim = new Claim(ClaimTypes.Email, "joe.doe@nhs.co.uk");
            claims.Add(claim);
            var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
            mockClaimsPrincipal.Setup(cp => cp.Claims).Returns(claims);

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = mockClaimsPrincipal.Object
                }
            };

            context.HttpContext.Request.Headers.Add("Authorization", "bearer Token1");

            var oktaClient = new OktaClient(oktaClientConfiguration);
   

            _accountController = new AccountController(_mockLogger.Object, _mockMapper.Object, oktaClient, _mockUserCreationService.Object, _mockUsersRetrievalByEmailService.Object, _mockUserUpdateOktaTokenService.Object, oktaSettings)
            {
                ControllerContext = context


            };
           
        }

        [Fact]
        public void ValidateSession_UserFoundNoNeedToRetrieveDetailsFromOkta()
        {
            //Act
            _accountController.ValidateSession();
        }
    }
}
