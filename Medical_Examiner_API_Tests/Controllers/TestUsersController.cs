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
    public class TestUsersController
    {
        /// <summary>
        /// The User Persistence mock.
        /// </summary>
        private readonly Mock<IUserPersistence> _userPersistence;

        /// <summary>
        /// The system under test.
        /// </summary>
        private readonly UsersController _userController;

        /// <summary>
        /// Setup
        /// </summary>
        public TestUsersController()
        {
            _userPersistence = new Mock<IUserPersistence>();

            var logger = new Mock<IMELogger>();

            _userController = new UsersController(_userPersistence.Object, logger.Object);
        }

        /// <summary>
        /// Test returning an empty list
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetEmptyResponse()
        {
            // Arrange
            _userPersistence.Setup(up => up.GetUsersAsync())
                .Returns(Task.FromResult<IEnumerable<MeUser>>(new List<MeUser>()));

            // Act
            var response = await _userController.GetUsers();

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult) response.Result;
            result.Value.Should().BeAssignableTo<GetUsersResponse>();
            var model = (GetUsersResponse) result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.Users.Count().Should().Be(0);
        }

        /// <summary>
        /// Test get a list of users
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetGoodResponse()
        {
            // Arrange
            const string expectedUserId = "expectedUserId";

            _userPersistence.Setup(up => up.GetUsersAsync()).Returns(Task.FromResult<IEnumerable<MeUser>>(
                new List<MeUser> {new MeUser {UserId = expectedUserId}}));

            // Act
            var response = await _userController.GetUsers();

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult) response.Result;
            result.Value.Should().BeAssignableTo<GetUsersResponse>();
            var model = (GetUsersResponse) result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.Users.Count().Should().Be(1);
            model.Users.First().UserId.Should().Be(expectedUserId);
        }

        /// <summary>
        /// Test that a good response is returned in full
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetIdGoodResponse()
        {
            // Arrange
            const string expectedUserId = "expectedUserId";

            var expectedUser = new MeUser {UserId = expectedUserId};

            _userPersistence.Setup(up => up.GetUserAsync(It.IsAny<string>())).Returns(Task.FromResult(expectedUser));

            // Act
            var response = await _userController.GetUser(expectedUserId);

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult) response.Result;
            result.Value.Should().BeAssignableTo<GetUserResponse>();
            var model = (GetUserResponse) result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();
            model.UserId.Should().Be(expectedUserId);
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
            var response = await _userController.GetUser(expectedUserId);

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
            _userController.ModelState.AddModelError("An", "Error");

            // Act
            var response = await _userController.GetUser(string.Empty);

            // Assert
            response.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            var result = (BadRequestObjectResult) response.Result;
            result.Value.Should().BeAssignableTo<GetUserResponse>();
            var model = (GetUserResponse) result.Value;
            model.Errors.Count.Should().Be(1);
            model.Success.Should().BeFalse();
        }
    }
}