using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Models.v1.Permissions;
using MedicalExaminer.Common;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Queries.Permission;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Controllers
{
    /// <summary>
    ///     Tests the Users Controller
    /// </summary>
    public class TestPermissionsController : AuthorizedControllerTestsBase<PermissionsController>
    {
        private readonly Mock<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>> _userRetrievalByIdServiceMock;
        private readonly Mock<IAsyncQueryHandler<UserUpdateQuery, MeUser>> _userUpdateServiceMock;
        private readonly Mock<IAsyncQueryHandler<LocationParentsQuery, IEnumerable<Location>>> _locationParentsServiceMock;
        private readonly Mock<IPermissionPersistence> _permissionPersistenceMock;

        private readonly
            Mock<IAsyncQueryHandler<PermissionsRetrievalByLocationsAndUserServiceQuery, IEnumerable<Permission>>>
            _permissionsRetrievalByLocationsAndUserServiceMock;

        private readonly Mock<IAsyncQueryHandler<LocationsParentsQuery, IDictionary<string, IEnumerable<Location>>>>
            _locationsParentsServiceMock;


        /// <summary>
        ///     Initializes a new instance of the <see cref="TestPermissionsController" /> class.
        /// </summary>
        public TestPermissionsController()
        {
            _permissionPersistenceMock = new Mock<IPermissionPersistence>(MockBehavior.Strict);
            _userRetrievalByIdServiceMock = new Mock<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>>(MockBehavior.Strict);
            _userUpdateServiceMock = new Mock<IAsyncQueryHandler<UserUpdateQuery, MeUser>>(MockBehavior.Strict);
            _locationParentsServiceMock = new Mock<IAsyncQueryHandler<LocationParentsQuery, IEnumerable<Location>>>(MockBehavior.Strict);
            _permissionsRetrievalByLocationsAndUserServiceMock = new Mock<IAsyncQueryHandler<PermissionsRetrievalByLocationsAndUserServiceQuery, IEnumerable<Permission>>>(MockBehavior.Strict);
            _locationsParentsServiceMock = new Mock<IAsyncQueryHandler<LocationsParentsQuery, IDictionary<string, IEnumerable<Location>>>>(MockBehavior.Strict);


            Controller = new PermissionsController(
                LoggerMock.Object,
                Mapper,
                UsersRetrievalByEmailServiceMock.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object,
                _userRetrievalByIdServiceMock.Object,
                _userUpdateServiceMock.Object,
                _locationParentsServiceMock.Object,
                _permissionsRetrievalByLocationsAndUserServiceMock.Object,
                _locationsParentsServiceMock.Object,
                _permissionPersistenceMock.Object
            );

            Controller.ControllerContext = GetContollerContext();

        }

        private ControllerContext GetContollerContext()
        {
            return new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Email, "test@example.com")
                    }))
                }
            };
        }

        [Fact]
        public async Task GetPermissions_ReturnsPermission()
        {
            // Arrange
            var expectedEmail = "expectedEmail";
            var expectedCurrentUserId = "expectedCurrentUserId";
            var expectedUserId = "expectedUserId";
            var expectedSiteId = "site1";
            var expectedNationalId = "national1";
            var expectedRole = UserRoles.MedicalExaminer;
            var expectedCurrentUserEmail = "test@example.com";
            var expectedUser = new MeUser()
            {
                UserId = expectedUserId,
                Permissions = new[]
                {
                    new MEUserPermission()
                    {
                        LocationId = expectedSiteId,
                        UserRole = (int)expectedRole,
                    },
                }
            };
            var expectedCurrentUser = new MeUser()
            {
                UserId = expectedCurrentUserId,
                Email = expectedCurrentUserEmail,
                Permissions = new[]
                {
                    new MEUserPermission()
                    {
                        LocationId = expectedNationalId,
                        UserRole = (int) expectedRole,
                    },
                }
            };
            var expectedPermission = Common.Authorization.Permission.GetUserPermissions;
            var expectedLocations = new[]
            {
                expectedNationalId,
            };
            IDictionary<string, IEnumerable<Location>> expectedLocationsParents = new Dictionary<string, IEnumerable<Location>>()
            {
                { expectedSiteId, new[] {
                    new Location(){ LocationId = expectedSiteId },
                    new Location(){ LocationId = "trust1" },
                    new Location(){ LocationId = "region1" },
                    new Location(){ LocationId = expectedNationalId },
                }},
            };

            _userRetrievalByIdServiceMock
                .Setup(urbis => urbis.Handle(It.Is<UserRetrievalByIdQuery>(q => q.UserId == expectedUserId)))
                .Returns(Task.FromResult(expectedUser));

            UsersRetrievalByEmailServiceMock
                .Setup(urbes => urbes.Handle(It.Is<UserRetrievalByEmailQuery>(q => q.Email == expectedCurrentUserEmail)))
                .Returns(Task.FromResult(expectedCurrentUser));

            PermissionServiceMock
                .Setup(ps => ps.LocationIdsWithPermission(expectedUser, expectedPermission))
                .Returns(expectedLocations);

            _locationsParentsServiceMock
                .Setup(lps => lps.Handle(It.IsAny<LocationsParentsQuery>()))
                .Returns(Task.FromResult(expectedLocationsParents));

            /*_permissionPersistenceMock.Setup(pp => pp.GetPermissionsAsync(userId)).Returns(
                Task.FromResult<IEnumerable<Permission>>(
                    new List<Permission>()));*/

            // Act
            var response = await Controller.GetPermissions(expectedUserId);

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetPermissionsResponse>();
            var model = (GetPermissionsResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.Permissions.Count().Should().Be(1);
            model.Permissions.ElementAt(0).LocationId.Should().Be(expectedNationalId);
            model.Permissions.ElementAt(0).UserRole.Should().Be((int)expectedRole);
        }

        /// <summary>
        ///     Test get a list of users
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetGoodResponse()
        {
            // Arrange
            const string expectedPermissionId = "fake_id_02";
            const string userId = "fake_id_01";

            _permissionPersistenceMock.Setup(pp => pp.GetPermissionsAsync("fake_id_01")).Returns(
                Task.FromResult<IEnumerable<Permission>>(
                    new List<Permission>
                        { new Permission { UserId = "fake_id_01", PermissionId = expectedPermissionId } }));

            // Act
            var response = await Controller.GetPermissions(userId);

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetPermissionsResponse>();
            var model = (GetPermissionsResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.Permissions.Count().Should().Be(1);
            model.Permissions.First().PermissionId.Should().Be(expectedPermissionId);
        }

        /// <summary>
        ///     Test that a good response is returned in full
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetIdGoodResponse()
        {
            // Arrange
            const string expectedPermissionId = "expectedPermissionId";
            const string expectedUserId = "expectedUserId";

            var expectedPermission = new Permission { PermissionId = expectedPermissionId, UserId = expectedUserId };

            _permissionPersistenceMock.Setup(pp => pp.GetPermissionAsync(expectedUserId, expectedPermissionId)).Returns(
                Task.FromResult(expectedPermission));

            // Act
            var response = await Controller.GetPermission(expectedUserId, expectedPermissionId);

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetPermissionResponse>();
            var model = (GetPermissionResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();
            model.UserId.Should().Be(expectedUserId);
            model.PermissionId.Should().Be(expectedPermissionId);
        }

        /// <summary>
        ///     Test when no user is found
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetIdNotFoundResponse()
        {
            // Arrange
            const string expectedUserId = "expectedUserId";

            _permissionPersistenceMock.Setup(up => up.GetPermissionAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Throws<NullReferenceException>();

            // Act
            var response = await Controller.GetPermission(expectedUserId, "Something_that_does_not_exist");

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            var result = (NotFoundObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetPermissionResponse>();
            var model = (GetPermissionResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.UserId.Should().Be(null);
        }

        /// <summary>
        ///     Test that model validation error causes validation failure
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetIdValidationFailure()
        {
            // Arrange
            Controller.ModelState.AddModelError("An", "Error");

            // Act
            var response = await Controller.GetPermission(string.Empty, string.Empty);

            // Assert
            response.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            var result = (BadRequestObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetPermissionResponse>();
            var model = (GetPermissionResponse)result.Value;
            model.Errors.Count.Should().Be(1);
            model.Success.Should().BeFalse();
        }
    }
}