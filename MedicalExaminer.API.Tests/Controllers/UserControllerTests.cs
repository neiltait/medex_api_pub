using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.API.Tests.Controllers;
using MedicalExaminer.API.Tests.Persistence;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class UserControllerTests : ControllerTestsBase<UsersController>
{

    private Mock<IMapper> mapper;
    private Mock<IMELogger> logger;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="UserControllerTests"/> class.
    /// </summary>
    public UserControllerTests()
    {
        logger = new Mock<IMELogger>();
        mapper = new Mock<IMapper>();
        var createExaminationService = new Mock<IAsyncQueryHandler<CreateUserQuery, MeUser>>();
        var examinationRetrievalQuery = new Mock<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>>();
        var examinationsRetrievalQuery = new Mock<IAsyncQueryHandler<UsersRetrievalQuery, IEnumerable<MeUser>>>();
            
        Controller = new UsersController(
            logger.Object,
            mapper.Object,
            createExaminationService.Object,
            examinationRetrievalQuery.Object,
            examinationsRetrievalQuery.Object);
    }

    [Fact]
    public void CreateUser_When_Called_Returns_Expected_Type()
    {
        // Arrange
        var expectedEmail = "aaa@bbb.com.co.gov.uk.com";
        var expectedRequest = new PostUserRequest
        {
            Email = expectedEmail,
        };
            
        var user = new MeUser()
        {
            Email = expectedEmail,
            UserId = "aabbccddss",
            FirstName = "aaa",
            LastName = "bbb"
        };
        mapper.Setup(m => m.Map<MeUser>(It.IsAny<PostUserRequest>())).Returns(user);
        var createExaminationService = new Mock<IAsyncQueryHandler<CreateUserQuery, MeUser>>();
        var examinationRetrievalQuery = new Mock<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>>();
        var examinationsRetrievalQuery =
            new Mock<IAsyncQueryHandler<UsersRetrievalQuery, IEnumerable<MeUser>>>();
            
        // Act
        var response = Controller.CreateUser(expectedRequest);

        // Assert
        var taskResult = response.Should().BeOfType<Task<ActionResult<PostUserResponse>>>().Subject;
        var okResult = taskResult.Result.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
        var returnUser = okResult.Value.Should().BeAssignableTo<PostUserResponse>().Subject;

        returnUser.Email.Should().Be(expectedEmail);
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

namespace MedicalExaminer.API.Tests.Controllers
{
}