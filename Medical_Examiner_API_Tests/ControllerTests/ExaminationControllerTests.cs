using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using ME_API_tests.Persistance;
using Medical_Examiner_API;
using Medical_Examiner_API.Controllers;
using Medical_Examiner_API.Loggers;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Persistence;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace ME_API_tests.ControllerTests
{
    public class ExaminationControllerTests
    {
        private MELoggerMocker _mockLogger;
        private ExaminationsController _controller;
        private IExaminationPersistence _examination_persistance;

        public ExaminationControllerTests()
        {
            // Arrange 
            _examination_persistance = new ExaminationPersistanceFake();
            _mockLogger = new MELoggerMocker();
            _controller = new ExaminationsController(_examination_persistance, _mockLogger);
        }

        [Fact]
        public void GetExaminations_When_Called_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetExaminations();

            // Assert
            var task_result = response.Should().BeOfType<Task<ActionResult<IEnumerable<Examination>>>>().Subject;
            var okresult = task_result.Result.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            var examinations = okresult.Value.Should().BeAssignableTo<ICollection<Examination>>().Subject;
            Assert.Equal(3, examinations.Count);
        }

        [Fact]
        public void GetExamination_When_Called_With_Valid_Id_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetExamination("aaaaa");

            // Assert
            var task_result = response.Should().BeOfType<Task<ActionResult<Examination>>>().Subject;
            var okresult = task_result.Result.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            var examination = okresult.Value.Should().BeAssignableTo<Examination>().Subject;
            Assert.Equal("aaaaa", examination.ExaminationId);
        }

        //[Fact]
        //public void GetExamination_When_Called_With_Invalid_Id_Returns_Expected_Type()
        //{
        //    // Act
        //    var response = _controller.GetExamination("dfgdfgdfg");

        //    // Assert
        //    var task_result = Assert.IsType<NotFoundObjectResult>(response);
        //    //var task_result = response.Should().BeOfType<Task<ActionResult<Examination>>>().Subject;
        //    //var notfound = task_result.Should().BeAssignableTo<NotFoundResult>().Subject;
        //}
    }
}
