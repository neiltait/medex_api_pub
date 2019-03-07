using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.API.Tests.Persistence;
using MedicalExaminer.Common;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Controllers
{
    public class ExaminationControllerTests : ControllerTestsBase<ExaminationsController>
    {
        public ExaminationControllerTests()
        {
            // Arrange
            IExaminationPersistence examinationPersistence = new ExaminationPersistenceFake();
            var mockLogger = new MELoggerMocker();
            var createExaminationService = new Mock<IAsyncQueryHandler<CreateExaminationQuery, string>>();
            Controller = new ExaminationsController(examinationPersistence, mockLogger, Mapper, createExaminationService.Object);
        }

        [Fact]
        public void GetExamination_When_Called_With_Invalid_Id_Returns_Expected_Type()
        {
            // Act
            var response = Controller.GetExamination("dfgdfgdfg");

            // Assert
            var taskResult = response.Should().BeOfType<Task<ActionResult<GetExaminationResponse>>>().Subject;
            Assert.Equal(TaskStatus.Faulted, taskResult.Status);
        }

        [Fact]
        public void GetExamination_When_Called_With_Valid_Id_Returns_Expected_Type()
        {
            // Act
            var response = Controller.GetExamination("aaaaa");

            // Assert
            var taskResult = response.Should().BeOfType<Task<ActionResult<GetExaminationResponse>>>().Subject;
            var okResult = taskResult.Result.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            var examination = okResult.Value.Should().BeAssignableTo<GetExaminationResponse>().Subject;
            Assert.Equal("aaaaa", examination.Id);
        }

        [Fact]
        public void GetExaminations_When_Called_Returns_Expected_Type()
        {
            // Act
            var response = Controller.GetExaminations();

            // Assert
            var taskResult = response.Should().BeOfType<Task<ActionResult<GetExaminationsResponse>>>().Subject;
            var okResult = taskResult.Result.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            var examinationResponse = okResult.Value.Should().BeAssignableTo<GetExaminationsResponse>().Subject;
            var examinations = examinationResponse.Examinations;
            Assert.Equal(3, examinations.Count());
        }

        [Fact]
        public void TestCreateCaseValidationFailure()
        {
            // Arrange
            var examination = CreateValidExamination();
            var createExaminationService = new Mock<IAsyncQueryHandler<CreateExaminationQuery, string>>();
            var examinationId = Guid.NewGuid();
            var persistence = new Mock<IExaminationPersistence>();
            persistence.Setup(p => p.CreateExaminationAsync(examination)).Returns(Task.FromResult(examinationId)).Verifiable();
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<Examination>(It.IsAny<PostNewCaseRequest>())).Returns(examination);
            var sut = new ExaminationsController(persistence.Object, logger.Object, mapper.Object, createExaminationService.Object);
            sut.ModelState.AddModelError("test", "test");

            // Act
            var response = sut.CreateNewCase(CreateValidNewCaseRequest()).Result;

            // Assert
            response.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            var result = (BadRequestObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<PutExaminationResponse>();
            var model = (PutExaminationResponse)result.Value;
            model.Errors.Count.Should().Be(1);
            model.Success.Should().BeFalse();
            persistence.Verify(p => p.CreateExaminationAsync(examination), Times.Never);
        }

        [Fact]
        public void ValidModelStateReturnsOkResponse()
        {
            // Arrange
            var examination = CreateValidExamination();
            var createExaminationService = new Mock<IAsyncQueryHandler<CreateExaminationQuery, string>>();
            var examinationId = Guid.NewGuid();
            var persistence = new Mock<IExaminationPersistence>();
            persistence.Setup(p => p.CreateExaminationAsync(examination)).Returns(Task.FromResult(examinationId)).Verifiable();
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<Examination>(It.IsAny<PostNewCaseRequest>())).Returns(examination);
            var sut = new ExaminationsController(persistence.Object, logger.Object, mapper.Object, createExaminationService.Object);

            // Act
            var response = sut.CreateNewCase(CreateValidNewCaseRequest()).Result;

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<PutExaminationResponse>();
            var model = (PutExaminationResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();
            persistence.Verify(p => p.CreateExaminationAsync(examination), Times.Exactly(1));
        }

        private PostNewCaseRequest CreateValidNewCaseRequest()
        {
            return new PostNewCaseRequest()
            {
                GivenNames = "A",
                Surname = "Patient",
                Gender = ExaminationGender.Male,
                MedicalExaminerOfficeResponsible = "7"
            };
        }

        private Examination CreateValidExamination()
        {
            var examination = new Examination()
            {
                Gender = ExaminationGender.Male,
                Surname = "Patient",
                GivenNames = "Barry",
                
            };
            return examination;
        }
    }
}
