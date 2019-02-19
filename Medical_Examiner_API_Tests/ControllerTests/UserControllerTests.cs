using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Persistence;
using Medical_Examiner_API.Controllers;
using Medical_Examiner_API.Loggers;
using ME_API_tests.Persistance;
using System.Threading.Tasks;
using Xunit;

namespace Medical_Examiner_API_Tests.ControllerTests
{
    public class UserControllerTests
    {
        MELoggerMocker _mockLogger;
        UserPersistanceFake _user_persistance;
        UsersController _controller;

        public UserControllerTests()
        {

            _mockLogger = new MELoggerMocker();
            _user_persistance = new UserPersistanceFake();
            _controller = new UsersController(_user_persistance, _mockLogger);
        }

        [Fact]
        public void GetUsers_When_Called_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetUsers();

            // Assert
            var task_result = response.Should().BeOfType<Task<ActionResult<IEnumerable<User>>>>().Subject;
            var okresult = task_result.Result.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            var examinations = okresult.Value.Should().BeAssignableTo<ICollection<User>>().Subject;
            Assert.Equal(10, examinations.Count);
        }

        [Fact]
        public void GetUser_When_Called_With_Valid_Id_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetUser("aaaaa0");

            // Assert
            var task_result = response.Should().BeOfType<Task<ActionResult<Examination>>>().Subject;
            var okresult = task_result.Result.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            var user = okresult.Value.Should().BeAssignableTo<User>().Subject;
            Assert.Equal("aaaaa0", user.UserId);
        }

        [Fact]
        public void GetUser_When_Called_With_invalid_Id_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetUser("zzzzz");

            // Assert
            var task_result = response.Should().BeOfType<Task<ActionResult<Examination>>>().Subject;
            var not_found_result = task_result.Result.Result.Should().BeAssignableTo<NotFoundResult>().Subject;
        }
    }
}
