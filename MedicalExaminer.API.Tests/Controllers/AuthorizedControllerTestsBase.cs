using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Moq;

namespace MedicalExaminer.API.Tests.Controllers
{
    public class AuthorizedControllerTestsBase<TController>
        : ControllerTestsBase<TController>
        where TController : AuthorizedBaseController
    {
        public AuthorizedControllerTestsBase()
        {
            UsersRetrievalByEmailServiceMock = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();

            AuthorizationServiceMock = new Mock<IAuthorizationService>();

            PermissionServiceMock = new Mock<IPermissionService>();

            AuthorizationServiceMock
                .Setup(aus => aus.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<ILocationBasedDocument>(),
                    It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .Returns(Task.FromResult(AuthorizationResult.Success()));
        }

        protected Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>> UsersRetrievalByEmailServiceMock { get; }

        protected Mock<IAuthorizationService> AuthorizationServiceMock { get; }

        protected Mock<IPermissionService> PermissionServiceMock { get; }
    }
}
