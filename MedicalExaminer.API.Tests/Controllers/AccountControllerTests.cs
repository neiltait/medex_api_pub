using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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
        /// <summary>
        /// Use proxy class for AccountController as not possible to Mock OktaClient
        /// Override CreateNewUser and flag if it has been called or not
        /// </summary>
        internal class AccountControllerProxy : AccountController
        {
            /// <summary>
            /// Flag to indicate if CreateNewUser method has been called
            /// </summary>
            internal bool CreateNewUserCalled { get; set; }

            internal AccountControllerProxy(
                IMELogger logger,
                IMapper mapper,
                OktaClient oktaClient,
                IAsyncQueryHandler<CreateUserQuery, MeUser> userCreationService,
                IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> usersRetrievalByEmailService,
                IAsyncQueryHandler<UsersUpdateOktaTokenQuery, MeUser> userUpdateOktaTokenService,
                IOptions<OktaSettings> oktaSettings)
                : base(logger, mapper, oktaClient, userCreationService, usersRetrievalByEmailService,
                    userUpdateOktaTokenService, oktaSettings)
            {

            }

            /// <summary>
            /// Overrides method in AccountController and sets CreateNewUserCalled to indicate it has been called
            /// </summary>
            /// <param name="emailAddress">Email address</param>
            /// <param name="oktaToken">Okta token</param>
            /// <returns>Always will return null</returns>
            protected override async Task<MeUser> CreateNewUser(string emailAddress, string oktaToken)
            {
                CreateNewUserCalled = true;
                return null;
            }

        }


        private readonly Mock<IMELogger> _mockLogger;

        private readonly Mock<IMapper> _mockMapper;

        private readonly Mock<IAsyncQueryHandler<CreateUserQuery, MeUser>> _mockUserCreationService;

        private readonly Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>> _mockUsersRetrievalByEmailService;

        private readonly Mock<IAsyncQueryHandler<UsersUpdateOktaTokenQuery, MeUser>> _mockUserUpdateOktaTokenService;

        private readonly Mock<IOptions<OktaSettings>> _mockOktaSettings;

        private AccountControllerProxy _accountController;

        private readonly ControllerContext _controllerContext;

        public AccountControllerTests()
        {
            _mockLogger = new Mock<IMELogger>();
            _mockMapper = new Mock<IMapper>();
            var oktaSettings = Options.Create(new OktaSettings());
            oktaSettings.Value.LocalTokenExpiryTimeMinutes = "30";

            var oktaClientConfiguration = new OktaClientConfiguration
            {
                OktaDomain = "https://xxx-123456.test.com",
                Token = "Token1",
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


            _accountController = new AccountControllerProxy(_mockLogger.Object, _mockMapper.Object, oktaClient, _mockUserCreationService.Object, _mockUsersRetrievalByEmailService.Object, _mockUserUpdateOktaTokenService.Object, oktaSettings)
            {
                ControllerContext = context
            };

        }

        [Fact]
        public void ValidateSession_UserFoundNoNeedToRetrieveDetailsFromOkta()
        {
            //Arrange
            var meUser = new MeUser();
            _mockUsersRetrievalByEmailService.Setup(es => es.Handle(It.IsAny<UserRetrievalByEmailQuery>()))
                .Returns(Task.FromResult(meUser));

            //Act
            _accountController.ValidateSession();

            //Assert
            Assert.Equal(false, _accountController.CreateNewUserCalled);
        }

        [Fact]
        public void ValidateSession_UserNotFoundRetrieveDetailsFromOkta()
        {
            //Arrange
            MeUser meUser = null;
            _mockUsersRetrievalByEmailService.Setup(es => es.Handle(It.IsAny<UserRetrievalByEmailQuery>()))
                .Returns(Task.FromResult(meUser));

            //Act
            _accountController.ValidateSession();

            //Assert
            Assert.Equal(true, _accountController.CreateNewUserCalled);
        }
    }
}
