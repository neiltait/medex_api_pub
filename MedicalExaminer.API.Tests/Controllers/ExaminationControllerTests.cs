using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
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
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<GetExaminationsResponse>(It.IsAny<Examination>())).Returns(er.Object);
            var createExaminationService = new Mock<IAsyncQueryHandler<CreateExaminationQuery, Examination>>();
            var examinationsRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>>();
            var examinationsDashboardService =
                new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview>>();

            examinationsRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationsRetrievalQuery>()))
                .Returns(Task.FromResult(examinationsResult));
            var sut = new ExaminationsController(
                logger.Object,
                mapper.Object,
                UsersRetrievalByEmailServiceMock.Object,
                AuthorizationServiceMock.Object,
                createExaminationService.Object,
                examinationsRetrievalQueryService.Object,
                examinationsDashboardService.Object);

            // Act
            var response = sut.GetExaminations(null).Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetExaminationsResponse>>().Subject;
            var okResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            //var examinationResponse = okResult..Value.Should().BeAssignableTo<GetExaminationsResponse>().Subject;
            //var examinations = examinationResponse.Examinations;
            //Assert.Equal(2, examinations.Count());
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

            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<Examination>(It.IsAny<PostExaminationRequest>())).Returns(examination);
            var sut = new ExaminationsController(
                logger.Object,
                mapper.Object,
                UsersRetrievalByEmailServiceMock.Object,
                AuthorizationServiceMock.Object,
                createExaminationService.Object,
                examinationsRetrievalQueryService.Object,
                examinationsDashboardService.Object);

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

            createExaminationService.Setup(ecs => ecs.Handle(It.IsAny<CreateExaminationQuery>()))
                .Returns(Task.FromResult(examination));

            mapper.Setup(m => m.Map<Examination>(It.IsAny<PostExaminationRequest>())).Returns(examination);

            var sut = new ExaminationsController(
                logger.Object,
                mapper.Object,
                UsersRetrievalByEmailServiceMock.Object,
                AuthorizationServiceMock.Object,
                createExaminationService.Object,
                examinationsRetrievalQueryService.Object,
                examinationsDashboardService.Object);

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
    }
}
