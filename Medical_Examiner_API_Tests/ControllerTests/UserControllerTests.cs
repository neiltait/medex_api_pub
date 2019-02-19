using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Medical_Examiner_API.Controllers;
using Medical_Examiner_API.Loggers;
using Medical_Examiner_API.Models;
using Medical_Examiner_API_Tests.Persistence;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Medical_Examiner_API_Tests.ControllerTests
{
    public class UserControllerTests
    {
        public UserControllerTests()
        {
            var mockLogger = new MELoggerMocker();
            var userPersistence = new UserPersistenceFake();
            _controller = new UsersController(userPersistence, mockLogger);
        }

        private readonly UsersController _controller;

        [Fact]
        public void CreateUser_When_Called_Returns_Expected_Type()
        {
            // Act
            var user = new MeUser
                {UserId = "zzaabb", FirstName = "Bob", Email = "aaa@bbb.com.co.gov.uk.com", LastName = "Shmob"};
            var response = _controller.CreateUser(user);

            // Assert
            var taskResult = response.Should().BeOfType<Task<ActionResult<MeUser>>>().Subject;
            var okResult = taskResult.Result.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            var returnUser = okResult.Value.Should().BeAssignableTo<MeUser>().Subject;

            Assert.Equal("zzaabb", returnUser.UserId);
        }

        [Fact]
        public void GetUser_When_Called_With_invalid_Id_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetUser("zzzzz");

            // Assert
            var taskResult = response.Should().BeOfType<Task<ActionResult<Examination>>>().Subject;
            var notFoundResult = taskResult.Result.Result.Should().BeAssignableTo<NotFoundResult>().Subject;
        }

        [Fact]
        public void GetUser_When_Called_With_Valid_Id_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetUser("aaaaa0");

            // Assert
            var taskResult = response.Should().BeOfType<Task<ActionResult<Examination>>>().Subject;
            var okResult = taskResult.Result.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            var user = okResult.Value.Should().BeAssignableTo<MeUser>().Subject;
            Assert.Equal("aaaaa0", user.UserId);
        }

        [Fact]
        public void GetUsers_When_Called_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetUsers();

            // Assert
            var taskResult = response.Should().BeOfType<Task<ActionResult<IEnumerable<MeUser>>>>().Subject;
            var okResult = taskResult.Result.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            var examinations = okResult.Value.Should().BeAssignableTo<ICollection<MeUser>>().Subject;
            Assert.Equal(10, examinations.Count);
        }
    }
}