using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Queries.Location;
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
        public AuthorizedControllerTestsBase(bool setupAuthorize = true)
        {
            UsersRetrievalByOktaIdServiceMock = new Mock<IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser>>();

            AuthorizationServiceMock = new Mock<IAuthorizationService>();

            PermissionServiceMock = new Mock<IPermissionService>();

            AuthorizedUser = new MeUser()
            {
                UserId = "authorizedUserId",
            };

            if (setupAuthorize)
            {
                AuthorizationServiceMock
                    .Setup(aus => aus.AuthorizeAsync(
                        It.IsAny<ClaimsPrincipal>(),
                        It.IsAny<ILocationPath>(),
                        It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                    .Returns(Task.FromResult(AuthorizationResult.Success()));
            }
        }

        protected void SetupAuthorize(AuthorizationResult result)
        {
            AuthorizationServiceMock
                .Setup(aus => aus.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<ILocationPath>(),
                    It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .Returns(Task.FromResult(result));
        }

        protected Mock<IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser>> UsersRetrievalByOktaIdServiceMock { get; }

        protected Mock<IAuthorizationService> AuthorizationServiceMock { get; }

        protected Mock<IPermissionService> PermissionServiceMock { get; }

        protected MeUser AuthorizedUser { get; }
    }
}
