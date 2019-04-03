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
using Xunit;

namespace MedicalExaminer.API.Tests.Controllers
{
    public class AccountControllerTests
    {
        private readonly Mock<IMELogger> _mockLogger;

        private readonly Mock<IMapper> _mockMapper;

        private readonly Mock<OktaClient> _mockOktaClient;

        private readonly Mock<IAsyncQueryHandler<CreateUserQuery, MeUser>> _mockUserCreationService;

        private readonly Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>> _mockUsersRetrievalByEmailService;

        private readonly Mock<IAsyncQueryHandler<UsersUpdateOktaTokenQuery, MeUser>> _mockUserUpdateOktaTokenService;

        private readonly Mock<IOptions<OktaSettings>> _mockOktaSettings;

        private AccountController _accountController;

        public AccountControllerTests()
        {
            _mockLogger = new Mock<IMELogger>();
            _mockMapper = new Mock<IMapper>();
            _mockOktaClient = new Mock<OktaClient>();
            _mockUserCreationService = new Mock<IAsyncQueryHandler<CreateUserQuery, MeUser>>();
            _mockUsersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            _mockUserUpdateOktaTokenService = new Mock<IAsyncQueryHandler<UsersUpdateOktaTokenQuery, MeUser>>();
            _mockOktaSettings = new Mock<IOptions<OktaSettings>>();
            _accountController = new AccountController(_mockLogger.Object, _mockMapper.Object, _mockOktaClient.Object, _mockUserCreationService.Object, _mockUsersRetrievalByEmailService.Object, _mockUserUpdateOktaTokenService.Object, _mockOktaSettings.Object);
        }

        [Fact]
        public void ValidateSession_UserFoundNoNeedToRetrieveDetailsFromOkta()
        {
            //Arrange
            var claimsPrincipal = new ClaimsPrincipal();
            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };

            //var accountController = new AccountController();
        }
    }
}
