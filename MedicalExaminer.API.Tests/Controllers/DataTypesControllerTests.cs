using System.Collections.Generic;
using FluentAssertions;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.Common.Loggers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace MedicalExaminer.API.Tests.Controllers
{
    public class DataTypesControllerTests : ControllerTestsBase<DataTypesController>
    {
        public DataTypesControllerTests()
        {
            var mockLogger = new MELoggerMocker();
            _controller = new DataTypesController(mockLogger, Mapper);
        }

        private readonly DataTypesController _controller;

        [Fact]
        public void GetCoronerStatuses_When_Called_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetCoronerStatuses();

            // Assert
            var okResult = response.Should().BeAssignableTo<OkObjectResult>().Subject;
            var dictionary = okResult.Value.Should().BeAssignableTo<Dictionary<string, int>>().Subject;

            Assert.Equal(4, dictionary.Keys.Count);
        }

        [Fact]
        public void GetExaminationGenders_When_Called_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetExaminationGenders();

            // Assert
            var okResult = response.Should().BeAssignableTo<OkObjectResult>().Subject;
            var dictionary = okResult.Value.Should().BeAssignableTo<Dictionary<string, int>>().Subject;

            Assert.Equal(3, dictionary.Keys.Count);
        }

        [Fact]
        public void GetModesOfDisposal_When_Called_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetModesOfDisposal();

            // Assert
            var okResult = response.Should().BeAssignableTo<OkObjectResult>().Subject;
            var dictionary = okResult.Value.Should().BeAssignableTo<Dictionary<string, int>>().Subject;

            Assert.Equal(4, dictionary.Keys.Count);
        }

        [Fact]
        public void GetAnalysisEntryTypes_When_Called_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetAnalysisEntryTypes();

            // Assert
            var okResult = response.Should().BeAssignableTo<OkObjectResult>().Subject;
            var dictionary = okResult.Value.Should().BeAssignableTo<Dictionary<string, int>>().Subject;

            Assert.Equal(6, dictionary.Keys.Count);
        }

        
        public void GetCaseStatuses_When_Called_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetCaseStatuses();

            // Assert
            var okResult = response.Should().BeAssignableTo<OkObjectResult>().Subject;
            var dictionary = okResult.Value.Should().BeAssignableTo<Dictionary<string, int>>().Subject;

            Assert.Equal(2, dictionary.Keys.Count);
        }

        [Fact]
        public void GetEventTypeTypes_When_Called_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetEventTypes();

            // Assert
            var okResult = response.Should().BeAssignableTo<OkObjectResult>().Subject;
            var dictionary = okResult.Value.Should().BeAssignableTo<Dictionary<string, int>>().Subject;

            Assert.Equal(1, dictionary.Keys.Count);
            

        }
    }
}