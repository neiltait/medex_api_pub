using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.API.Tests.Persistence;
using MedicalExaminer.Common;
using MedicalExaminer.Common.Loggers;
using Microsoft.AspNetCore.Mvc;
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
            Controller = new ExaminationsController(examinationPersistence, mockLogger, Mapper);
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
            Assert.Equal("aaaaa", examination.ExaminationId);
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
    }
}
