using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Medical_Examiner_API.Controllers;
using Medical_Examiner_API.Extensions.Data;
using Medical_Examiner_API.Loggers;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Models.V1.Users;
using Medical_Examiner_API_Tests.Controllers;
using Medical_Examiner_API_Tests.Persistence;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Medical_Examiner_API_Tests.ControllerTests
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

            Assert.Equal(2, dictionary.Keys.Count);
        }

        [Fact]
        public void GetModesOfDisposal_When_Called_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetModesOfDisposal();

            // Assert
            var okResult = response.Should().BeAssignableTo<OkObjectResult>().Subject;
            var dictionary = okResult.Value.Should().BeAssignableTo<Dictionary<string, int>>().Subject;

            Assert.Equal(2, dictionary.Keys.Count);
        }

        [Fact]
        public void GetAnalysisEntryTypes_When_Called_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetAnalysisEntryTypes();

            // Assert
            var okResult = response.Should().BeAssignableTo<OkObjectResult>().Subject;
            var dictionary = okResult.Value.Should().BeAssignableTo<Dictionary<string, int>>().Subject;

            Assert.Equal(5, dictionary.Keys.Count);
        }
    }
}
