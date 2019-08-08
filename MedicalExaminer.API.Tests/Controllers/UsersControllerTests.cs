using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Okta.Sdk;
using Xunit;

namespace MedicalExaminer.API.Tests.Controllers
{
    /// <summary>
    /// Users Controller Tests
    /// </summary>
    public class UsersControllerTests : AuthorizedControllerTestsBase<UsersController>
    {
        private readonly Mock<IAsyncQueryHandler<CreateUserQuery, MeUser>> _createUserService;
        private readonly Mock<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>> _userRetrievalService;
        private readonly Mock<IAsyncQueryHandler<UsersRetrievalQuery, IEnumerable<MeUser>>> _usersRetrievalService;
        private readonly Mock<IAsyncQueryHandler<UserUpdateQuery, MeUser>> _userUpdateService;
        private readonly Mock<IAsyncQueryHandler<LocationsRetrievalByQuery, IEnumerable<Location>>> _locationsService;
        private readonly Mock<IOktaClient> _mockOktaClient;

        public UsersControllerTests()
        {
            var logger = new Mock<IMELogger>();

            _createUserService = new Mock<IAsyncQueryHandler<CreateUserQuery, MeUser>>();
            _userRetrievalService = new Mock<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>>();
            _usersRetrievalService = new Mock<IAsyncQueryHandler<UsersRetrievalQuery, IEnumerable<MeUser>>>();
            _userUpdateService = new Mock<IAsyncQueryHandler<UserUpdateQuery, MeUser>>();
            _locationsService = new Mock<IAsyncQueryHandler<LocationsRetrievalByQuery, IEnumerable<Location>>>();
            _mockOktaClient = new Mock<IOktaClient>(MockBehavior.Strict);

            Controller = new UsersController(
                logger.Object,
                Mapper,
                UsersRetrievalByOktaIdServiceMock.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object,
                _createUserService.Object,
                _userRetrievalService.Object,
                _usersRetrievalService.Object,
                _userUpdateService.Object,
                _locationsService.Object,
                _mockOktaClient.Object);
        }

        /// <summary>
        /// Test when a ArgumentException is thrown.
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestCreateUserArgumentException()
        {
            // Arrange
            const string oktaFirstName = "oktaFirstName";
            const string oktaLastName = "oktaLastName";
            const string oktaId = "oktaId";
            const string oktaEmail = "oktaEmail";
            _createUserService
                .Setup(cus => cus.Handle(It.IsAny<CreateUserQuery>()))
                .Throws<ArgumentException>();
            var request = new PostUserRequest();
            Controller.ControllerContext = GetControllerContext();

            var oktaUser = new Mock<IUser>(MockBehavior.Strict);
            oktaUser.Setup(u => u.Id).Returns(oktaId);
            oktaUser.Setup(u => u.Profile.FirstName).Returns(oktaFirstName);
            oktaUser.Setup(u => u.Profile.LastName).Returns(oktaLastName);
            oktaUser.Setup(u => u.Profile.Email).Returns(oktaEmail);

            _mockOktaClient
                .Setup(s => s.Users.GetUserAsync(It.IsAny<string>(), default(CancellationToken)))
                .Returns(Task.FromResult(oktaUser.Object));

            // Act
            var response = await Controller.CreateUser(request);

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            var result = (NotFoundObjectResult) response.Result;
            result.Value.Should().BeAssignableTo<PostUserResponse>();
            var model = (PostUserResponse) result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.UserId.Should().Be(null);
        }

        /// <summary>
        /// Test when a DocumentClientException is thrown.
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestCreateUserDocumentClientException()
        {
            // Arrange
            const string oktaFirstName = "oktaFirstName";
            const string oktaLastName = "oktaLastName";
            const string oktaId = "oktaId";
            const string oktaEmail = "oktaEmail";

            var request = new PostUserRequest();
            _createUserService.Setup(cus => cus.Handle(It.IsAny<CreateUserQuery>()))
                .Throws(CreateDocumentClientExceptionForTesting());
            Controller.ControllerContext = GetControllerContext();

            var oktaUser = new Mock<IUser>(MockBehavior.Strict);
            oktaUser.Setup(u => u.Id).Returns(oktaId);
            oktaUser.Setup(u => u.Profile.FirstName).Returns(oktaFirstName);
            oktaUser.Setup(u => u.Profile.LastName).Returns(oktaLastName);
            oktaUser.Setup(u => u.Profile.Email).Returns(oktaEmail);

            _mockOktaClient
                .Setup(s => s.Users.GetUserAsync(It.IsAny<string>(), default(CancellationToken)))
                .Returns(Task.FromResult(oktaUser.Object));

            // Act
            var response = await Controller.CreateUser(request);

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            var result = (NotFoundObjectResult) response.Result;
            result.Value.Should().BeAssignableTo<PostUserResponse>();
            var model = (PostUserResponse) result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.UserId.Should().Be(null);
        }

        /// <summary>
        /// Test that an ok response is returned in full
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestCreateUserOkResponse()
        {
            // Arrange
            const string expectedUserId = "expectedUserId";
            const string expectedEmail = "thisisatest@methods.co.uk";
            const string oktaFirstName = "oktaFirstName";
            const string oktaLastName = "oktaLastName";
            const string oktaId = "oktaId";
            const string oktaEmail = "oktaEmail";

            var request = new PostUserRequest
            {
                Email = expectedEmail,
            };

            var user = new MeUser
            {
                UserId = expectedUserId,
                Email = expectedEmail,
            };

            var oktaUser = new Mock<IUser>(MockBehavior.Strict);
            oktaUser.Setup(u => u.Id).Returns(oktaId);
            oktaUser.Setup(u => u.Profile.FirstName).Returns(oktaFirstName);
            oktaUser.Setup(u => u.Profile.LastName).Returns(oktaLastName);
            oktaUser.Setup(u => u.Profile.Email).Returns(oktaEmail);

            _mockOktaClient
                .Setup(s => s.Users.GetUserAsync(It.IsAny<string>(), default(CancellationToken)))
                .Returns(Task.FromResult(oktaUser.Object));

            _createUserService
                .Setup(cus => cus.Handle(It.IsAny<CreateUserQuery>()))
                .Returns(Task.FromResult(user));
            Controller.ControllerContext = GetControllerContext();

            // Act
            var response = await Controller.CreateUser(request);

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult) response.Result;
            result.Value.Should().BeAssignableTo<PostUserResponse>();
            var model = (PostUserResponse) result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();
            model.UserId.Should().Be(expectedUserId);
        }

        /// <summary>
        /// Test that model validation error causes validation failure.
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestCreateUserReturnsBadRequest_WhenOktaUserNotFound()
        {
            // Arrange
            Controller.ControllerContext = GetControllerContext();

            _mockOktaClient
                .Setup(s => s.Users.GetUserAsync(It.IsAny<string>(), default(CancellationToken)))
                .Returns(Task.FromResult((IUser)null));

            var request = new PostUserRequest();

            // Act
            var response = await Controller.CreateUser(request);

            // Assert
            response.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            var result = (BadRequestObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<PostUserResponse>();
            var model = (PostUserResponse)result.Value;
            model.Errors.Count.Should().Be(1);
            model.Success.Should().BeFalse();
        }

        /// <summary>
        /// Test that model validation error causes validation failure.
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestCreateUserVerificationFailure()
        {
            // Arrange
            Controller.ModelState.AddModelError("An", "Error");

            // Act
            var request = new PostUserRequest();
            var response = await Controller.CreateUser(request);

            // Assert
            response.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            var result = (BadRequestObjectResult) response.Result;
            result.Value.Should().BeAssignableTo<PostUserResponse>();
            var model = (PostUserResponse) result.Value;
            model.Errors.Count.Should().Be(1);
            model.Success.Should().BeFalse();
        }

        /// <summary>
        ///     Test when no user is found
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetUserNotFoundResponse()
        {
            // Arrange
            const string expectedUserId = "expectedUserId";

            _userRetrievalService.Setup(up => up.Handle(It.IsAny<UserRetrievalByIdQuery>()))
                .Throws<NullReferenceException>();

            // Act
            var response = await Controller.GetUser(expectedUserId);

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            var result = (NotFoundObjectResult) response.Result;
            result.Value.Should().BeAssignableTo<GetUserResponse>();
            var model = (GetUserResponse) result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.UserId.Should().Be(null);
        }

        [Fact]
        public async Task TestGetUserReturnsNotFound_WhenArgumentExceptionThrown()
        {
            // Arrange
            const string expectedUserId = "expectedUserId";

            _userRetrievalService.Setup(up => up.Handle(It.IsAny<UserRetrievalByIdQuery>()))
                .Throws<ArgumentException>();

            // Act
            var response = await Controller.GetUser(expectedUserId);

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            var result = (NotFoundObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetUserResponse>();
            var model = (GetUserResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.UserId.Should().Be(null);
        }

        /// <summary>
        ///     Test that a good response is returned in full
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetUserOkResponse()
        {
            // Arrange
            const string expectedUserId = "expectedUserId";

            var user = new MeUser { UserId = expectedUserId };

            _userRetrievalService.Setup(up => up.Handle(It.IsAny<UserRetrievalByIdQuery>()))
                .Returns(Task.FromResult(user));

            // Act
            var response = await Controller.GetUser(expectedUserId);

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
        ///     Test an <see cref="ArgumentException" /> being thrown when getting users.
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetUsersArgumentException()
        {
            // Arrange
            _usersRetrievalService.Setup(up => up.Handle(It.IsAny<UsersRetrievalQuery>()))
                .Throws<ArgumentException>();

            // Act
            var response = await Controller.GetUsers();

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            var result = (NotFoundObjectResult) response.Result;
            result.Value.Should().BeAssignableTo<GetUsersResponse>();
            var model = (GetUsersResponse) result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.Users.Should().BeNull();
        }

        [Fact]
        public async Task TestGetUsersDocumentClientException()
        {
            // Arrange
            _usersRetrievalService
                .Setup(up => up.Handle(It.IsAny<UsersRetrievalQuery>()))
                .Throws(CreateDocumentClientExceptionForTesting());

            // Act
            var response = await Controller.GetUsers();

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            var result = (NotFoundObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetUsersResponse>();
            var model = (GetUsersResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.Users.Should().BeNull();
        }

        /// <summary>
        ///     Test returning an empty list
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetUsersEmptyResponse()
        {
            // Arrange
            _usersRetrievalService.Setup(up => up.Handle(It.IsAny<UsersRetrievalQuery>()))
                .Returns(Task.FromResult<IEnumerable<MeUser>>(new List<MeUser>()));

            // Act
            var response = await Controller.GetUsers();

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
        ///     Test get a list of users
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetUsersOkResponse()
        {
            // Arrange
            const string expectedUserId = "expectedUserId";

            _usersRetrievalService.Setup(up => up.Handle(It.IsAny<UsersRetrievalQuery>())).Returns(
                Task.FromResult<IEnumerable<MeUser>>(
                    new List<MeUser> { new MeUser { UserId = expectedUserId } }));

            // Act
            var response = await Controller.GetUsers();

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
        ///     Test that model validation error causes validation failure
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetUserValidationFailure()
        {
            // Arrange
            Controller.ModelState.AddModelError("An", "Error");

            // Act
            var response = await Controller.GetUser(string.Empty);

            // Assert
            response.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            var result = (BadRequestObjectResult) response.Result;
            result.Value.Should().BeAssignableTo<GetUserResponse>();
            var model = (GetUserResponse) result.Value;
            model.Errors.Count.Should().Be(1);
            model.Success.Should().BeFalse();
        }

        /// <summary>
        ///     Test when a ArgumentException is thrown.
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestUpdateUserArgumentException()
        {
            // Arrange
            var request = new PutUserRequest();

            _userUpdateService.Setup(up => up.Handle(It.IsAny<UserUpdateQuery>()))
                .Throws<ArgumentException>();
            Controller.ControllerContext = GetControllerContext();

            // Act
            var response = await Controller.UpdateUser(It.IsAny<string>(), request);

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            var result = (NotFoundObjectResult) response.Result;
            result.Value.Should().BeAssignableTo<PutUserResponse>();
            var model = (PutUserResponse) result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.UserId.Should().Be(null);
        }

        /// <summary>
        ///     Test when a DocumentClientException is thrown.
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestUpdateUserDocumentClientException()
        {
            // Arrange
            var request = new PutUserRequest();

            _userUpdateService.Setup(up => up.Handle(It.IsAny<UserUpdateQuery>()))
                .Throws(CreateDocumentClientExceptionForTesting());
            Controller.ControllerContext = GetControllerContext();

            // Act
            var response = await Controller.UpdateUser(It.IsAny<string>(), request);

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            var result = (NotFoundObjectResult) response.Result;
            result.Value.Should().BeAssignableTo<PutUserResponse>();
            var model = (PutUserResponse) result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.UserId.Should().Be(null);
        }

        /// <summary>
        ///     Test that an ok response is returned in full
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestUpdateUserOkResponse()
        {
            // Arrange
            const string expectedUserId = "expectedUserId";

            var request = new PutUserRequest
            {
                Email = "testing@methods.co.uk"
            };

            var user = new MeUser
            {
                UserId = expectedUserId,
                Email = "testing@methods.co.uk"
            };

            _userUpdateService.Setup(up => up.Handle(It.IsAny<UserUpdateQuery>()))
                .Returns(Task.FromResult(user));
            Controller.ControllerContext = GetControllerContext();

            // Act
            var response = await Controller.UpdateUser(It.IsAny<string>(), request);

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult) response.Result;
            result.Value.Should().BeAssignableTo<PutUserResponse>();
            var model = (PutUserResponse) result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();
            model.UserId.Should().Be(expectedUserId);
        }

        /// <summary>
        ///     Test that model validation error causes validation failure
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestUpdateUserVerificationFailure()
        {
            // Arrange
            Controller.ModelState.AddModelError("An", "Error");

            // Act
            var request = new PutUserRequest();
            var response = await Controller.UpdateUser(It.IsAny<string>(), request);

            // Assert
            response.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            var result = (BadRequestObjectResult) response.Result;
            result.Value.Should().BeAssignableTo<PutUserResponse>();
            var model = (PutUserResponse) result.Value;
            model.Errors.Count.Should().Be(1);
            model.Success.Should().BeFalse();
        }
    }
}