using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.API.Tests.Persistence;
using MedicalExaminer.Common.Loggers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace MedicalExaminer.API.Tests.Controllers
{
    public class UserControllerTests : ControllerTestsBase<UsersController>
    {
        /// <summary>
        ///     Initialise a new instance of the Users Controller Tests
        /// </summary>
        public UserControllerTests()
        {
            var mockLogger = new MELoggerMocker();
            var userPersistence = new UserPersistenceFake();
            Controller = new UsersController(userPersistence, mockLogger, Mapper);
        }

        [Fact]
        public void CreateUser_When_Called_Returns_Expected_Type()
        {
            // Arrange
            var expectedFirstName = "Bob";
            var expectedEmail = "aaa@bbb.com.co.gov.uk.com";
            var expectedLastName = "Shmob";
            var expectedUserId = "1";

            var expectedRequest = new PostUserRequest
            {
                FirstName = expectedFirstName,
                Email = expectedEmail,
                LastName = expectedLastName
            };

            // Act
            var response = Controller.CreateUser(expectedRequest);

            // Assert
            var taskResult = response.Should().BeOfType<Task<ActionResult<PostUserResponse>>>().Subject;
            var okResult = taskResult.Result.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            var returnUser = okResult.Value.Should().BeAssignableTo<PostUserResponse>().Subject;

            returnUser.UserId.Should().Be(expectedUserId);
        }

        [Fact]
        public void GetUser_When_Called_With_invalid_Id_Returns_Expected_Type()
        {
            // Act
            var response = Controller.GetUser("zzzzz");

            // Assert
            var taskResult = response.Should().BeOfType<Task<ActionResult<GetUserResponse>>>().Subject;
            var notFoundResult = taskResult.Result.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject;
        }

        [Fact]
        public void GetUser_When_Called_With_Valid_Id_Returns_Expected_Type()
        {
            // Act
            var response = Controller.GetUser("aaaaa0");

            // Assert
            var taskResult = response.Should().BeOfType<Task<ActionResult<GetUserResponse>>>().Subject;
            var okResult = taskResult.Result.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            var user = okResult.Value.Should().BeAssignableTo<GetUserResponse>().Subject;
            Assert.Equal("aaaaa0", user.UserId);
        }

        [Fact]
        public void GetUsers_When_Called_Returns_Expected_Type()
        {
            // Act
            var response = Controller.GetUsers();

            // Assert
            var taskResult = response.Should().BeOfType<Task<ActionResult<GetUsersResponse>>>().Subject;
            var okResult = taskResult.Result.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            var users = okResult.Value.Should().BeAssignableTo<GetUsersResponse>().Subject;
            Assert.Equal(10, users.Users.Count());
        }
    }
}