using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Medical_Examiner_API.Controllers;
using Medical_Examiner_API.Loggers;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Models.v1.Users;
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
        /// The User Persistance mock.
        /// </summary>
        private readonly Mock<IUserPersistence> _userPersistance;

        /// <summary>
        /// The Logger mock.
        /// </summary>
        private readonly Mock<IMELogger> _logger;

        /// <summary>
        /// The system under test.
        /// </summary>
        private UsersController _sut;

        /// <summary>
        /// Setup
        /// </summary>
        public TestUsersController()
        {
            _userPersistance = new Mock<IUserPersistence>();

            _logger = new Mock<IMELogger>();

            _sut = new UsersController(_userPersistance.Object, _logger.Object);
        }

        [Fact]
        public async Task TestGetEmptyResponse()
        {
            // Arrange 
            _userPersistance.Setup(up => up.GetUsersAsync()).Returns(Task.FromResult<IEnumerable<User>>(new List<User>()));

            // Act
            var response = await _sut.GetAsync();

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetUsersResponse>();
            var model = (GetUsersResponse)result.Value;
            model.Errors.Count().Should().Be(0);
            model.Success.Should().BeTrue();

            model.Users.Count().Should().Be(0);
        }

        [Fact]
        public async Task TestGetGoodResponse()
        {
            // Arrange 
            var expectedUserId = "expectedUserId";

            _userPersistance.Setup(up => up.GetUsersAsync()).Returns(Task.FromResult<IEnumerable<User>>(new List<User>()
            {
                new User() {Id = expectedUserId}
            }));

            // Act
            var response = await _sut.GetAsync();

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult) response.Result;
            result.Value.Should().BeAssignableTo<GetUsersResponse>();
            var model = (GetUsersResponse) result.Value;
            model.Errors.Count().Should().Be(0);
            model.Success.Should().BeTrue();

            model.Users.Count().Should().Be(1);
            model.Users.First().Id.Should().Be(expectedUserId);
        }

        [Fact]
        public async Task TestGetIdGoodResponse()
        {
            // Arrange 
            var expectedUserId = "expectedUserId";
            var expectedUser = new User()
            {
                Id = expectedUserId
            };

            _userPersistance.Setup(up => up.GetUserAsync(It.IsAny<string>())).Returns(Task.FromResult<User>(expectedUser));

            // Act
            var response = await _sut.GetAsync(expectedUserId);

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetUserResponse>();
            var model = (GetUserResponse)result.Value;
            model.Errors.Count().Should().Be(0);
            model.Success.Should().BeTrue();

            model.Id.Should().Be(expectedUserId);
        }

        [Fact]
        public async Task TestGetIdNotFoundResponse()
        {
            // Arrange 
            var expectedUserId = "expectedUserId";

            _userPersistance.Setup(up => up.GetUserAsync(It.IsAny<string>())).Returns(Task.FromResult<User>(null));

            // Act
            var response = await _sut.GetAsync(expectedUserId);

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            var result = (NotFoundObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetUserResponse>();
            var model = (GetUserResponse)result.Value;
            model.Errors.Count().Should().Be(0);
            model.Success.Should().BeTrue();

            model.Id.Should().Be(null);
        }

        [Fact]
        public async Task TestGetIdValidationFailure()
        {
            // Arrange 
            _sut.ModelState.AddModelError("An", "Error");

            // Act
            var response = await _sut.GetAsync(string.Empty);

            // Assert
            response.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            var result = (BadRequestObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetUserResponse>();
            var model = (GetUserResponse)result.Value;
            model.Errors.Count().Should().Be(1);
            model.Success.Should().BeFalse();
        }
    }
}
