using System;
using System.Collections.Generic;
using System.Linq;
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
using Xunit;

namespace MedicalExaminer.API.Tests.Controllers
{
    /// <summary>
    ///     Users Controller Tests
    /// </summary>
    public class UsersControllerTests : AuthorizedControllerTestsBase<UsersController>
    {
        public Mock<IAsyncQueryHandler<CreateUserQuery, MeUser>> createUserService;
        public Mock<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>> userRetrievalService;
        public Mock<IAsyncQueryHandler<UsersRetrievalQuery, IEnumerable<MeUser>>> usersRetrievalService;
        public Mock<IAsyncQueryHandler<UserUpdateQuery, MeUser>> userUpdateService;
        public Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>> userRetrievalByEmailService;
        public Mock<IAsyncQueryHandler<LocationsRetrievalByQuery, IEnumerable<Location>>> locationsService;

        public UsersControllerTests() : base()
        {
            var logger = new Mock<IMELogger>();

            createUserService = new Mock<IAsyncQueryHandler<CreateUserQuery, MeUser>>();
            userRetrievalService = new Mock<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>>();
            usersRetrievalService = new Mock<IAsyncQueryHandler<UsersRetrievalQuery, IEnumerable<MeUser>>>();
            userUpdateService = new Mock<IAsyncQueryHandler<UserUpdateQuery, MeUser>>();
            userRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            locationsService = new Mock<IAsyncQueryHandler<LocationsRetrievalByQuery, IEnumerable<Location>>>();
            Controller = new UsersController(
                logger.Object,
                Mapper,
                UsersRetrievalByOktaIdServiceMock.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object,
                createUserService.Object,
                userRetrievalService.Object,
                usersRetrievalService.Object,
                userUpdateService.Object,
                userRetrievalByEmailService.Object,
                locationsService.Object);
        }

        /// <summary>
        ///     Test when a ArgumentException is thrown.
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestCreateUserArgumentException()
        {
            // Arrange 
            createUserService.Setup(cus => cus.Handle(It.IsAny<CreateUserQuery>())).Throws<ArgumentException>();
            var expectedRequest = new PostUserRequest();
            Controller.ControllerContext = GetControllerContext();

            // Act
            var response = await Controller.CreateUser(expectedRequest);

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
        ///     Test when a DocumentClientException is thrown.
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestCreateUserDocumentClientException()
        {
            // Arrange
            var expectedRequest = new PostUserRequest();
            createUserService.Setup(cus => cus.Handle(It.IsAny<CreateUserQuery>()))
                .Throws(CreateDocumentClientExceptionForTesting());
            Controller.ControllerContext = GetControllerContext();

            // Act
            var response = await Controller.CreateUser(expectedRequest);

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
        ///     Test that an ok response is returned in full
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestCreateUserOkResponse()
        {
            // Arrange
            const string expectedUserId = "expectedUserId";
            const string expectedEmail = "thisisatest@methods.co.uk";

            var expectedRequest = new PostUserRequest
            {
                Email = expectedEmail,
            };

            var expectedUser = new MeUser
            {
                UserId = expectedUserId,
                Email = expectedEmail,
            };

            createUserService.Setup(cus => cus.Handle(It.IsAny<CreateUserQuery>()))
                .Returns(Task.FromResult(expectedUser));
            Controller.ControllerContext = GetControllerContext();

            // Act
            var response = await Controller.CreateUser(expectedRequest);

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

            userRetrievalService.Setup(up => up.Handle(It.IsAny<UserRetrievalByIdQuery>()))
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

        /// <summary>
        ///     Test that a good response is returned in full
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetUserOkResponse()
        {
            // Arrange
            const string expectedUserId = "expectedUserId";

            var expectedUser = new MeUser { UserId = expectedUserId };

            userRetrievalService.Setup(up => up.Handle(It.IsAny<UserRetrievalByIdQuery>()))
                .Returns(Task.FromResult(expectedUser));

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
            usersRetrievalService.Setup(up => up.Handle(It.IsAny<UsersRetrievalQuery>()))
                .Throws<ArgumentException>();

            var expectedUser = new MeUser { UserId = "aabbcc" };

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

        /// <summary>
        ///     Test returning an empty list
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetUsersEmptyResponse()
        {
            // Arrange
            usersRetrievalService.Setup(up => up.Handle(It.IsAny<UsersRetrievalQuery>()))
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

            usersRetrievalService.Setup(up => up.Handle(It.IsAny<UsersRetrievalQuery>())).Returns(
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
            var expectedRequest = new PutUserRequest();

            userUpdateService.Setup(up => up.Handle(It.IsAny<UserUpdateQuery>()))
                .Throws<ArgumentException>();
            Controller.ControllerContext = GetControllerContext();

            // Act
            var response = await Controller.UpdateUser(It.IsAny<string>(), expectedRequest);

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
            var expectedRequest = new PutUserRequest();

            userUpdateService.Setup(up => up.Handle(It.IsAny<UserUpdateQuery>()))
                .Throws(CreateDocumentClientExceptionForTesting());
            Controller.ControllerContext = GetControllerContext();

            // Act
            var response = await Controller.UpdateUser(It.IsAny<string>(), expectedRequest);

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

            var expectedRequest = new PutUserRequest
            {
                Email = "testing@methods.co.uk"
            };

            var expectedUser = new MeUser
            {
                UserId = expectedUserId,
                Email = "testing@methods.co.uk"
            };

            userUpdateService.Setup(up => up.Handle(It.IsAny<UserUpdateQuery>()))
                .Returns(Task.FromResult(expectedUser));
            Controller.ControllerContext = GetControllerContext();

            // Act
            var response = await Controller.UpdateUser(It.IsAny<string>(), expectedRequest);

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