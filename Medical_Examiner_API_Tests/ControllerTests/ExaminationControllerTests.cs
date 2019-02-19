using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Medical_Examiner_API.Controllers;
using Medical_Examiner_API.Loggers;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Persistence;
using Medical_Examiner_API_Tests.Persistence;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Medical_Examiner_API_Tests.ControllerTests
{
    public class ExaminationControllerTests
    {
        readonly ExaminationsController _controller;

        public ExaminationControllerTests()
        {
            // Arrange 
            IExaminationPersistence examinationPersistence = new ExaminationPersistenceFake();
            var mockLogger = new MELoggerMocker();
            _controller = new ExaminationsController(examinationPersistence, mockLogger);
        }

        [Fact]
        public void GetExaminations_When_Called_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetExaminations();

            // Assert
            var taskResult = response.Should().BeOfType<Task<ActionResult<IEnumerable<Examination>>>>().Subject;
            var okResult = taskResult.Result.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            var examinations = okResult.Value.Should().BeAssignableTo<ICollection<Examination>>().Subject;
            Assert.Equal(3, examinations.Count);
        }

        [Fact]
        public void GetExamination_When_Called_With_Valid_Id_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetExamination("aaaaa");

            // Assert
            var taskResult = response.Should().BeOfType<Task<ActionResult<Examination>>>().Subject;
            var okResult = taskResult.Result.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            var examination = okResult.Value.Should().BeAssignableTo<Examination>().Subject;
            Assert.Equal("aaaaa", examination.ExaminationId);
        }

        [Fact]
        public void GetExamination_When_Called_With_Invalid_Id_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetExamination("dfgdfgdfg");

            // Assert
            var taskResult = response.Should().BeOfType<Task<ActionResult<Examination>>>().Subject;
            Assert.Equal(TaskStatus.Faulted, taskResult.Status);
        }
    }
}
