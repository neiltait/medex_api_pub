using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Controllers
{
    public class ExaminationControllerTests : AuthorizedControllerTestsBase<ExaminationsController>
    {
        private PostExaminationRequest CreateValidNewCaseRequest()
        {
            return new PostExaminationRequest
            {
                GivenNames = "A",
                Surname = "Patient",
                Gender = ExaminationGender.Male,
                MedicalExaminerOfficeResponsible = "7"
            };
        }

        private Examination CreateValidExamination()
        {
            var examination = new Examination
            {
                Gender = ExaminationGender.Male,
                Surname = "Patient",
                GivenNames = "Barry"
            };
            return examination;
        }

        [Fact]
        public void GetExaminations_When_Called_Returns_Expected_Type()
        {
            // Arrange
            var examination1 = new Examination();
            var examination2 = new Examination();
            IEnumerable<Examination> examinationsResult = new List<Examination> { examination1, examination2 };
            var er = new Mock<GetExaminationsResponse>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<GetExaminationsResponse>(It.IsAny<Examination>())).Returns(er.Object);
            var createExaminationService = new Mock<IAsyncQueryHandler<CreateExaminationQuery, Examination>>();
            var examinationsRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>>();
            var examinationsDashboardService =
                new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview>>();
            var locationParentsService = new Mock<IAsyncQueryHandler<LocationParentsQuery, IEnumerable<Location>>>();

            examinationsRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationsRetrievalQuery>()))
                .Returns(Task.FromResult(examinationsResult));
            var sut = new ExaminationsController(
                logger.Object,
                mapper.Object,
                UsersRetrievalByEmailServiceMock.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object,
                createExaminationService.Object,
                examinationsRetrievalQueryService.Object,
                examinationsDashboardService.Object,
                locationParentsService.Object);

            // Act
            var response = sut.GetExaminations(null).Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetExaminationsResponse>>().Subject;
            var okResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
        }

        [Fact]
        public void TestCreateCaseValidationFailure()
        {
            // Arrange
            var examination = CreateValidExamination();
            var createExaminationService = new Mock<IAsyncQueryHandler<CreateExaminationQuery, Examination>>();
            var examinationsRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>>();
            var examinationsDashboardService =
                new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview>>();
            var examinationId = Guid.NewGuid();
            var locationParentsService = new Mock<IAsyncQueryHandler<LocationParentsQuery, IEnumerable<Location>>>();

            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<Examination>(It.IsAny<PostExaminationRequest>())).Returns(examination);
            var sut = new ExaminationsController(
                logger.Object,
                mapper.Object,
                UsersRetrievalByEmailServiceMock.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object,
                createExaminationService.Object,
                examinationsRetrievalQueryService.Object,
                examinationsDashboardService.Object,
                locationParentsService.Object);

            sut.ModelState.AddModelError("test", "test");

            // Act
            var response = sut.CreateExamination(CreateValidNewCaseRequest()).Result;

            // Assert
            response.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            var result = (BadRequestObjectResult) response.Result;
            result.Value.Should().BeAssignableTo<PutExaminationResponse>();
            var model = (PutExaminationResponse) result.Value;
            model.Errors.Count.Should().Be(1);
            model.Success.Should().BeFalse();
        }

        [Fact]
        public async void ValidModelStateReturnsOkResponse()
        {
            // Arrange
            var examination = CreateValidExamination();
            var createExaminationService = new Mock<IAsyncQueryHandler<CreateExaminationQuery, Examination>>();
            var examinationsRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>>();

            var examinationsDashboardService =
                new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview>>();
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var examinationId = Guid.NewGuid();
            var locationParentsService = new Mock<IAsyncQueryHandler<LocationParentsQuery, IEnumerable<Location>>>();

            createExaminationService.Setup(ecs => ecs.Handle(It.IsAny<CreateExaminationQuery>()))
                .Returns(Task.FromResult(examination));
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            mapper.Setup(m => m.Map<Examination>(It.IsAny<PostExaminationRequest>())).Returns(examination);

            var mockMeUser = new MeUser()
            {
                UserId = "abcd"
            };
            usersRetrievalByEmailService.Setup(service => service.Handle(It.IsAny<UserRetrievalByEmailQuery>()))
                .Returns(Task.FromResult(mockMeUser));

            var sut = new ExaminationsController(
                logger.Object,
                mapper.Object,
                UsersRetrievalByEmailServiceMock.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object,
                createExaminationService.Object,
                examinationsRetrievalQueryService.Object,
                examinationsDashboardService.Object,
                locationParentsService.Object);

            // Act
            var response = await sut.CreateExamination(CreateValidNewCaseRequest());

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult) response.Result;
            result.Value.Should().BeAssignableTo<PutExaminationResponse>();
            var model = (PutExaminationResponse) result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();
        }

        private ControllerContext GetContollerContext()
        {
            return new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, "username")
            }, "someAuthTypeName"))
                }
            };
        }
    }
}
