using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Medical_Examiner_API.Controllers;
using Medical_Examiner_API.Loggers;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Models.V1.Users;
using Medical_Examiner_API.Persistence;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Medical_Examiner_API_Tests.Controllers
{
    /// <summary>
    /// Tests the Users Controller
    /// </summary>
    public class TestPermissionsController
    {
        /// <summary>
        /// The User Persistence and permission persistence mock.
        /// </summary>
        private readonly Mock<IUserPersistence> _userPersistence;

        private readonly Mock<IPermissionPersistence> _permissionPersistence;

        /// <summary>
        /// The system under test.
        /// </summary>
        private readonly PermissionsController _permissionsController;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestPermissionsController"/> class.
        /// </summary>
        public TestPermissionsController()
        {
            _permissionPersistence = new Mock<IPermissionPersistence>();
            _userPersistence = new Mock<IUserPersistence>();
            var logger = new Mock<IMELogger>();

            _permissionsController =
                new PermissionsController(_userPersistence.Object, _permissionPersistence.Object, logger.Object);
        }

        /// <summary>
        /// Test returning an empty list
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetEmptyResponse()
        {
            // Arrange
            var userId = "fake_id_01";

            _permissionPersistence.Setup(pp => pp.GetPermissionsAsync(userId)).Returns(
                Task.FromResult<IEnumerable<Permission>>(
                    new List<Permission>()));

            // Act
            var response = await _permissionsController.GetPermissions(userId);

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult) response.Result;
            result.Value.Should().BeAssignableTo<GetPermissionsResponse>();
            var model = (GetPermissionsResponse) result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.Permissions.Count().Should().Be(0);
        }

        /// <summary>
        /// Test get a list of users
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetGoodResponse()
        {
            // Arrange
            const string expectedPermissionId = "fake_id_02";
            const string userId = "fake_id_01";

            _permissionPersistence.Setup(pp => pp.GetPermissionsAsync("fake_id_01")).Returns(
                Task.FromResult<IEnumerable<Permission>>(
                    new List<Permission>
                        {new Permission {UserId = "fake_id_01", PermissionId = expectedPermissionId}}));

            // Act
            var response = await _permissionsController.GetPermissions(userId);

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult) response.Result;
            result.Value.Should().BeAssignableTo<GetPermissionsResponse>();
            var model = (GetPermissionsResponse) result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.Permissions.Count().Should().Be(1);
            model.Permissions.First().PermissionId.Should().Be(expectedPermissionId);
        }

        /// <summary>
        /// Test that a good response is returned in full
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetIdGoodResponse()
        {
            // Arrange
            const string expectedPermissionId = "expectedPermissionId";
            const string expectedUserId = "expectedUserId";

            var expectedPermission = new Permission {PermissionId = expectedPermissionId, UserId = expectedUserId};

            _permissionPersistence.Setup(pp => pp.GetPermissionAsync(expectedUserId, expectedPermissionId)).Returns(
                Task.FromResult(expectedPermission));

            // Act
            var response = await _permissionsController.GetPermission(expectedUserId, expectedPermissionId);

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult) response.Result;
            result.Value.Should().BeAssignableTo<GetPermissionResponse>();
            var model = (GetPermissionResponse) result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();
            model.UserId.Should().Be(expectedUserId);
            model.PermissionId.Should().Be(expectedPermissionId);
        }

        /// <summary>
        /// Test when no user is found
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetIdNotFoundResponse()
        {
            // Arrange
            const string expectedUserId = "expectedUserId";

            _userPersistence.Setup(up => up.GetUserAsync(It.IsAny<string>())).Returns(Task.FromResult<MeUser>(null));

            // Act
            var response = await _permissionsController.GetPermission(expectedUserId, "Something_that_does_not_exist");

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            var result = (NotFoundObjectResult) response.Result;
            result.Value.Should().BeAssignableTo<GetUserResponse>();
            var model = (GetUserResponse) result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.UserId.Should().Be(null);
        }

        /// <summary>
        /// Test that model validation error causes validation failure
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetIdValidationFailure()
        {
            // Arrange
            _permissionsController.ModelState.AddModelError("An", "Error");

            // Act
            var response = await _permissionsController.GetPermission(string.Empty, string.Empty);

            // Assert
            response.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            var result = (BadRequestObjectResult) response.Result;
            result.Value.Should().BeAssignableTo<GetPermissionResponse>();
            var model = (GetPermissionResponse) result.Value;
            model.Errors.Count.Should().Be(1);
            model.Success.Should().BeFalse();
        }
    }
}