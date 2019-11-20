using System.Collections.Generic;
using FluentAssertions;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.Common.Queries.MELogger;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Controllers
{
    public class DataTypesControllerTests : ControllerTestsBase<DataTypesController>
    {
        public DataTypesControllerTests()
        {
            var mockLogger = new Mock<IAsyncQueryHandler<CreateMELoggerQuery, LogMessageActionDefault>>();
            _controller = new DataTypesController(mockLogger.Object, Mapper);
        }

        private readonly DataTypesController _controller;

        [Fact]
        public void GetStatusBarResult_When_Called_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetStatusBarResult();

            // Assert
            var okResult = response.Should().BeAssignableTo<OkObjectResult>().Subject;
            var dictionary = okResult.Value.Should().BeAssignableTo<Dictionary<string, int>>().Subject;

            Assert.Equal(4, dictionary.Keys.Count);
        }

        [Fact]
        public void GetPersonalEffects_When_Called_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetPersonalEffects();

            // Assert
            var okResult = response.Should().BeAssignableTo<OkObjectResult>().Subject;
            var dictionary = okResult.Value.Should().BeAssignableTo<Dictionary<string, int>>().Subject;

            Assert.Equal(3, dictionary.Keys.Count);
        }

        [Fact]
        public void GetAnyImplants_When_Called_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetAnyImplants();

            // Assert
            var okResult = response.Should().BeAssignableTo<OkObjectResult>().Subject;
            var dictionary = okResult.Value.Should().BeAssignableTo<Dictionary<string, int>>().Subject;

            Assert.Equal(3, dictionary.Keys.Count);
        }

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

            Assert.Equal(5, dictionary.Keys.Count);
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

        [Fact]
        public void GetCaseStatuses_When_Called_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetCaseStatuses();

            // Assert
            var okResult = response.Should().BeAssignableTo<OkObjectResult>().Subject;
            var dictionary = okResult.Value.Should().BeAssignableTo<Dictionary<string, int>>().Subject;

            Assert.Equal(8, dictionary.Keys.Count);
        }

        [Fact]
        public void GetEventTypeTypes_When_Called_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetEventTypes();

            // Assert
            var okResult = response.Should().BeAssignableTo<OkObjectResult>().Subject;
            var dictionary = okResult.Value.Should().BeAssignableTo<Dictionary<string, int>>().Subject;

            Assert.Equal(10, dictionary.Keys.Count);
        }

        [Fact]
        public void GetOverallCircumstancesOfDeath_When_Called_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetOverallCircumstancesOfDeath();

            // Assert
            var okResult = response.Should().BeAssignableTo<OkObjectResult>().Subject;
            var dictionary = okResult.Value.Should().BeAssignableTo<Dictionary<string, int>>().Subject;

            Assert.Equal(4, dictionary.Keys.Count);
        }

        [Fact]
        public void GetOverallOutcomeOfPreScrutiny_When_Called_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetOverallOutcomeOfPreScrutiny();

            // Assert
            var okResult = response.Should().BeAssignableTo<OkObjectResult>().Subject;
            var dictionary = okResult.Value.Should().BeAssignableTo<Dictionary<string, int>>().Subject;

            Assert.Equal(3, dictionary.Keys.Count);
        }

        [Fact]
        public void GetClinicalGovernanceReview_When_Called_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetClinicalGovernanceReview();

            // Assert
            var okResult = response.Should().BeAssignableTo<OkObjectResult>().Subject;
            var dictionary = okResult.Value.Should().BeAssignableTo<Dictionary<string, int>>().Subject;

            Assert.Equal(3, dictionary.Keys.Count);
        }

        [Fact]
        public void GetQapDiscussionOutcome_When_Called_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetQapDiscussionOutcome();

            // Assert
            var okResult = response.Should().BeAssignableTo<OkObjectResult>().Subject;
            var dictionary = okResult.Value.Should().BeAssignableTo<Dictionary<string, int>>().Subject;

            Assert.Equal(6, dictionary.Keys.Count);
        }

        [Fact]
        public void GetInformedAtDeath_When_Called_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetInformedAtDeath();
            // Assert
            var okResult = response.Should().BeAssignableTo<OkObjectResult>().Subject;
            var dictionary = okResult.Value.Should().BeAssignableTo<Dictionary<string, int>>().Subject;

            Assert.Equal(3, dictionary.Keys.Count);
        }

        [Fact]
        public void GetBereavedDiscussionOutcome_When_Called_Returns_Expected_Type()
        {
            // Act
            var response = _controller.GetBereavedDiscussionOutcome();

            // Assert
            var okResult = response.Should().BeAssignableTo<OkObjectResult>().Subject;
            var dictionary = okResult.Value.Should().BeAssignableTo<Dictionary<string, int>>().Subject;

            Assert.Equal(5, dictionary.Keys.Count);
        }
    }
}