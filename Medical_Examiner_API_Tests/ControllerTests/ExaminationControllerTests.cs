using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Medical_Examiner_API;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Persistence;
using Medical_Examiner_API.Controllers;
using Medical_Examiner_API.Loggers;
using ME_API_tests.Persistance;
using System.Threading.Tasks;
using AutoMapper;
using Medical_Examiner_API.Models.V1.Examinations;
using Moq;
using Xunit;

namespace ME_API_tests.ControllerTests
{
    public class ExaminationControllerTests
    {
        MELoggerMocker _mockLogger;
        ExaminationsController _controller;
        IExaminationPersistence _examination_persistance;
        private Mock<IMapper> _mapper;

        public ExaminationControllerTests()
        {
            _mapper = new Mock<IMapper>();

            // Arrange 
            _examination_persistance = new ExaminationPersistanceFake();
            _mockLogger = new MELoggerMocker();
            _controller = new ExaminationsController(_examination_persistance, _mockLogger, _mapper.Object);
        }

        [Fact]
        public void GetExaminations_When_Called_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetExaminations();

            // Assert
            var task_result = response.Should().BeOfType<Task<ActionResult<GetExaminationsResponse>>>().Subject;
            var okresult = task_result.Result.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            var examinationsResponse = okresult.Value.Should().BeAssignableTo<GetExaminationsResponse>().Subject;
            var examinations = examinationsResponse.Examinations;
            Assert.Equal(3, examinations.Count());
        }

        [Fact]
        public void GetExamination_When_Called_With_Valid_Id_Returns_Expected_Type()
        {
            // Arrange
            var expectedExaminationId = "aaaaa";
            _mapper.Setup(m => m.Map<GetExaminationResponse>(It.IsAny<Examination>())).Returns(
                new GetExaminationResponse()
                {
                    ExaminationId = expectedExaminationId,
                });

            // Act
            var response = _controller.GetExamination(expectedExaminationId);

            // Assert
            var task_result = response.Should().BeOfType<Task<ActionResult<GetExaminationResponse>>>().Subject;
            var okresult = task_result.Result.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            var examinationResponse = okresult.Value.Should().BeAssignableTo<GetExaminationResponse>().Subject;
            Assert.Equal("aaaaa", examinationResponse.ExaminationId);
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
