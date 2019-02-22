using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Medical_Examiner_API.Controllers;
using Medical_Examiner_API.Loggers;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Models.V1.Users;
using Medical_Examiner_API.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Azure.Documents;
using Moq;
using Xunit;
using Document = Microsoft.CodeAnalysis.Document;

namespace Medical_Examiner_API_Tests.Controllers
{
    /// <summary>
    /// Users Controller Tests
    /// </summary>
    public class UsersControllerTests : ControllerTestsBase<UsersController>
    {
        /// <summary>
        /// The User Persistence mock.
        /// </summary>
        private readonly Mock<IUserPersistence> _userPersistence;

        /// <summary>
        /// Setup
        /// </summary>
        public UsersControllerTests()
        {
            _userPersistence = new Mock<IUserPersistence>();

            var logger = new Mock<IMELogger>();

            Controller = new UsersController(_userPersistence.Object, logger.Object, Mapper);
        }

        /// <summary>
        /// Test returning an empty list
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetUsersEmptyResponse()
        {
            // Arrange
            _userPersistence.Setup(up => up.GetUsersAsync())
                .Returns(Task.FromResult<IEnumerable<MeUser>>(new List<MeUser>()));

            // Act
            var response = await Controller.GetUsers();

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetUsersResponse>();
            var model = (GetUsersResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.Users.Count().Should().Be(0);
        }

        /// <summary>
        /// Test get a list of users
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetUsersOkResponse()
        {
            // Arrange
            const string expectedUserId = "expectedUserId";

            _userPersistence.Setup(up => up.GetUsersAsync()).Returns(Task.FromResult<IEnumerable<MeUser>>(
                new List<MeUser> { new MeUser { UserId = expectedUserId } }));

            // Act
            var response = await Controller.GetUsers();

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetUsersResponse>();
            var model = (GetUsersResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.Users.Count().Should().Be(1);
            model.Users.First().UserId.Should().Be(expectedUserId);
        }

        /// <summary>
        /// Test a document client exception being thrown when getting users.
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetUsersDocumentClientException()
        {
            // Arrange
            _userPersistence.Setup(up => up.GetUsersAsync())
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
        /// Test an <see cref="ArgumentException"/> being thrown when getting users.
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetUsersArgumentException()
        {
            // Arrange
            _userPersistence.Setup(up => up.GetUsersAsync())
                .Throws<ArgumentException>();

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
        /// Test that a good response is returned in full
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetUserOkResponse()
        {
            // Arrange
            const string expectedUserId = "expectedUserId";

            var expectedUser = new MeUser { UserId = expectedUserId };

            _userPersistence.Setup(up => up.GetUserAsync(It.IsAny<string>())).Returns(Task.FromResult(expectedUser));

            // Act
            var response = await Controller.GetUser(expectedUserId);

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetUserResponse>();
            var model = (GetUserResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();
            model.UserId.Should().Be(expectedUserId);
        }

        /// <summary>
        /// Test when no user is found
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetUserNotFoundResponse()
        {
            // Arrange
            const string expectedUserId = "expectedUserId";

            _userPersistence.Setup(up => up.GetUserAsync(It.IsAny<string>()))
                .Throws<NullReferenceException>();

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
        /// Test that model validation error causes validation failure
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
            var result = (BadRequestObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetUserResponse>();
            var model = (GetUserResponse)result.Value;
            model.Errors.Count.Should().Be(1);
            model.Success.Should().BeFalse();
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
            const string expectedFirstName = "expectedFirstName";

            var expectedRequest = new PostUserRequest()
            {
                FirstName = expectedFirstName,
            };

            var expectedUser = new MeUser
            {
                UserId = expectedUserId,
                FirstName = expectedFirstName,
            };

            _userPersistence.Setup(up => up.CreateUserAsync(It.IsAny<MeUser>()))
                .Returns(Task.FromResult(expectedUser));

            // Act
            var response = await Controller.CreateUser(expectedRequest);

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<PostUserResponse>();
            var model = (PostUserResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();
            model.UserId.Should().Be(expectedUserId);
        }

        /// <summary>
        /// Test when a DocumentClientException is thrown.
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestCreateUserDocumentClientException()
        {
            // Arrange
            var expectedRequest = new PostUserRequest();

            _userPersistence.Setup(up => up.CreateUserAsync(It.IsAny<MeUser>()))
                .Throws(CreateDocumentClientExceptionForTesting());

            // Act
            var response = await Controller.CreateUser(expectedRequest);

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            var result = (NotFoundObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<PostUserResponse>();
            var model = (PostUserResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.UserId.Should().Be(null);
        }

        /// <summary>
        /// Test when a ArgumentException is thrown.
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestCreateUserArgumentException()
        {
            // Arrange
            var expectedRequest = new PostUserRequest();

            _userPersistence.Setup(up => up.CreateUserAsync(It.IsAny<MeUser>()))
                .Throws<ArgumentException>();

            // Act
            var response = await Controller.CreateUser(expectedRequest);

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            var result = (NotFoundObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<PostUserResponse>();
            var model = (PostUserResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.UserId.Should().Be(null);
        }

        /// <summary>
        /// Test that model validation error causes validation failure
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
            var result = (BadRequestObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<PostUserResponse>();
            var model = (PostUserResponse)result.Value;
            model.Errors.Count.Should().Be(1);
            model.Success.Should().BeFalse();
        }

        /// <summary>
        /// Test that an ok response is returned in full
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestUpdateUserOkResponse()
        {
            // Arrange
            const string expectedUserId = "expectedUserId";
            const string expectedFirstName = "expectedFirstName";

            var expectedRequest = new PutUserRequest()
            {
                UserId = expectedUserId,
                FirstName = expectedFirstName,
            };

            var expectedUser = new MeUser
            {
                UserId = expectedUserId,
                FirstName = expectedFirstName,
            };

            _userPersistence.Setup(up => up.UpdateUserAsync(It.IsAny<MeUser>()))
                .Returns(Task.FromResult(expectedUser));

            // Act
            var response = await Controller.UpdateUser(expectedRequest);

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<PutUserResponse>();
            var model = (PutUserResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();
            model.UserId.Should().Be(expectedUserId);
        }

        /// <summary>
        /// Test when a DocumentClientException is thrown.
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestUpdateUserDocumentClientException()
        {
            // Arrange
            var expectedRequest = new PutUserRequest();

            _userPersistence.Setup(up => up.UpdateUserAsync(It.IsAny<MeUser>()))
                .Throws(CreateDocumentClientExceptionForTesting());

            // Act
            var response = await Controller.UpdateUser(expectedRequest);

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            var result = (NotFoundObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<PutUserResponse>();
            var model = (PutUserResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.UserId.Should().Be(null);
        }

        /// <summary>
        /// Test when a ArgumentException is thrown.
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestUpdateUserArgumentException()
        {
            // Arrange
            var expectedRequest = new PutUserRequest();

            _userPersistence.Setup(up => up.UpdateUserAsync(It.IsAny<MeUser>()))
                .Throws<ArgumentException>();

            // Act
            var response = await Controller.UpdateUser(expectedRequest);

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            var result = (NotFoundObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<PutUserResponse>();
            var model = (PutUserResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.UserId.Should().Be(null);
        }

        /// <summary>
        /// Test that model validation error causes validation failure
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestUpdateUserVerificationFailure()
        {
            // Arrange
            Controller.ModelState.AddModelError("An", "Error");

            // Act
            var request = new PutUserRequest();
            var response = await Controller.UpdateUser(request);

            // Assert
            response.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            var result = (BadRequestObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<PutUserResponse>();
            var model = (PutUserResponse)result.Value;
            model.Errors.Count.Should().Be(1);
            model.Success.Should().BeFalse();
        }

        private static DocumentClientException CreateDocumentClientExceptionForTesting()
        {
            var error = new Error
            {
                Id = Guid.NewGuid().ToString(),
                Code = "some_code",
                Message = "some_message"
            };

            var testException = CreateDocumentClientExceptionForTesting(
                error,
                HttpStatusCode.InternalServerError);

            return testException;
        }

        private static DocumentClientException CreateDocumentClientExceptionForTesting(
            Error error, HttpStatusCode httpStatusCode)
        {
            var type = typeof(DocumentClientException);

            var documentClientExceptionInstance = type.Assembly.CreateInstance(
                type.FullName,
                false,
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new object[] { error, null, httpStatusCode },
                null,
                null);

            return (DocumentClientException)documentClientExceptionInstance;
        }
    }
}