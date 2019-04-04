using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.CaseBreakdown;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Controllers
{
    public class QapDiscussionEventControllerTests
    {
        
        [Fact]
        public async void Put_Final_QapDiscussion_Event_Null_Request_Object_Returns_Invalid_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();

            var qapDiscussionEventCreationService =
                new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();


            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
                qapDiscussionEventCreationService.Object, examinationRetrievalQueryService.Object);

            // Act
            var response = await sut.UpsertNewQapDiscussionEvent("a", null);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutQapDiscussionEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutQapDiscussionEventResponse>();
        }

        [Fact]
        public async void Put_Final_QapDiscussion_Event_Invalid_Request_Object_Returns_Invalid_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();

            var qapDiscussionEventCreationService =
                new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var invalidRequest = new PutQapDiscussionEventRequest
            {
                EventId = null,
                IsFinal = true,
                DiscussionDetails = null
            };

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
                qapDiscussionEventCreationService.Object, examinationRetrievalQueryService.Object);

            sut.ModelState.AddModelError("i", "broke it");
            // Act
            var response = await sut.UpsertNewQapDiscussionEvent("a", invalidRequest);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutQapDiscussionEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutQapDiscussionEventResponse>();
        }

        [Fact]
        public async void Put_Final_QapDiscussion_Event_Valid_Request_Object_Cannot_Find_Examination()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();

            var qapDiscussionEventCreationService =
                new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var validRequest = new PutQapDiscussionEventRequest
            {
                EventId = "1",
                IsFinal = true,
                DiscussionDetails = "Hello Planet"
            };

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
                qapDiscussionEventCreationService.Object, examinationRetrievalQueryService.Object);

            // Act
            var response = await sut.UpsertNewQapDiscussionEvent("a", validRequest);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutQapDiscussionEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutQapDiscussionEventResponse>();
        }

        [Fact]
        public async void Put_Final_QapDiscussion_Event_Valid_Request_Object_Finds_Examination_Then_Ok_Result()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var examination = new Mock<Examination>();
            var qapDiscussionEventCreationService =
                new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var validRequest = new PutQapDiscussionEventRequest
            {
                EventId = "1",
                IsFinal = true,
                DiscussionDetails = "Hello Planet"
            };
            qapDiscussionEventCreationService
                .Setup(service => service.Handle(It.IsAny<CreateEventQuery>()))
                .Returns(Task.FromResult("hi mark")).Verifiable();

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examination.Object)).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
                qapDiscussionEventCreationService.Object, examinationRetrievalQueryService.Object);

            // Act
            var response = await sut.UpsertNewQapDiscussionEvent("a", validRequest);

            qapDiscussionEventCreationService.Verify(x => x.Handle(It.IsAny<CreateEventQuery>()),
                Times.Once);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutQapDiscussionEventResponse>>().Subject;
            var goodRequestResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            goodRequestResult.Value.Should().BeAssignableTo<PutQapDiscussionEventResponse>();
        }
    }
}
