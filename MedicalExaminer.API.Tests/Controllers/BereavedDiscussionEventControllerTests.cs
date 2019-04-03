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
    public class BereavedDiscussionEventControllerTests
    {
        [Fact]
        public async void GetBereavedDiscussionEvent_When_Called_With_Null_Returns_Bad_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var BereavedDiscussionEventCreationService = new Mock<IAsyncQueryHandler<CreateBereavedDiscussionEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var sut = new BereavedDiscussionEventController(logger.Object, mapper.Object, 
                BereavedDiscussionEventCreationService.Object, examinationRetrievalQueryService.Object);
            // Act
            var response = await sut.GetBereavedDiscussionEvent(null);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetBereavedDiscussionEventResponse>>().Subject;
            var notFoundResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            notFoundResult.Value.Should().BeAssignableTo<GetBereavedDiscussionEventResponse>();
        }

        [Fact]
        public async void GetBereavedDiscussionEvent_When_Called_With_Invalid_Examination_Id_Returns_Bad_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var BereavedDiscussionEventCreationService = new Mock<IAsyncQueryHandler<CreateBereavedDiscussionEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var sut = new BereavedDiscussionEventController(logger.Object, mapper.Object, 
                BereavedDiscussionEventCreationService.Object, examinationRetrievalQueryService.Object);
            // Act
            var response = await sut.GetBereavedDiscussionEvent("aaaa");

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetBereavedDiscussionEventResponse>>().Subject;
            var notFoundResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            notFoundResult.Value.Should().BeAssignableTo<GetBereavedDiscussionEventResponse>();
        }

        [Fact]
        public async void GetBereavedDiscussionEvent_When_Called_With_Valid_Examination_Id_But_Examination_Not_Found_Returns_Not_Found_Response()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();

            var examinationId = "7E5D50CE-05BF-4A1F-AA6E-25418A723A7F";
            var BereavedDiscussionEventCreationService = new Mock<IAsyncQueryHandler<CreateBereavedDiscussionEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new BereavedDiscussionEventController(logger.Object, mapper.Object, 
               BereavedDiscussionEventCreationService.Object, examinationRetrievalQueryService.Object);

            // Act
            var response = await sut.GetBereavedDiscussionEvent(examinationId);

            // Assert
            examinationRetrievalQueryService.Verify(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()));
            var taskResult = response.Should().BeOfType<ActionResult<GetBereavedDiscussionEventResponse>>().Subject;
            var notFoundResult = taskResult.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject;
            notFoundResult.Value.Should().BeAssignableTo<GetBereavedDiscussionEventResponse>();
        }

        [Fact]
        public async void GetBereavedDiscussionEvent_When_Called_With_Valid_Examination_Id_Examination_Found_Returns_All_BereavedDiscussion_Events()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var examinationObj = new Mock<Examination>().Object;
            var examinationId = "7E5D50CE-05BF-4A1F-AA6E-25418A723A7F";
            var getBereavedDiscussionResponse = new Mock<GetBereavedDiscussionEventResponse>().Object;

            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<GetBereavedDiscussionEventResponse>(examinationObj)).Returns(getBereavedDiscussionResponse);

            var BereavedDiscussionEventCreationService = new Mock<IAsyncQueryHandler<CreateBereavedDiscussionEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();


            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examinationObj)).Verifiable();

            var sut = new BereavedDiscussionEventController(logger.Object, mapper.Object, 
               BereavedDiscussionEventCreationService.Object, examinationRetrievalQueryService.Object);

            // Act
            var response = await sut.GetBereavedDiscussionEvent(examinationId);

            // Assert
            examinationRetrievalQueryService.Verify(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()));

            var taskResult = response.Should().BeOfType<ActionResult<GetBereavedDiscussionEventResponse>>().Subject;
            var okResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            okResult.Value.Should().BeAssignableTo<GetBereavedDiscussionEventResponse>();

        }

        [Fact]
        public async void Put_Final_BereavedDiscussion_Event_Null_Request_Object_Returns_Invalid_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();

            var BereavedDiscussionEventCreationService = new Mock<IAsyncQueryHandler<CreateBereavedDiscussionEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();


            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new BereavedDiscussionEventController(logger.Object, mapper.Object, 
               BereavedDiscussionEventCreationService.Object, examinationRetrievalQueryService.Object);

            // Act
            var response = await sut.UpsertNewBereavedDiscussionEvent("a", null);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutBereavedDiscussionEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutBereavedDiscussionEventResponse>();
        }

        [Fact]
        public async void Put_Final_BereavedDiscussion_Event_Invalid_Request_Object_Returns_Invalid_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();

            var BereavedDiscussionEventCreationService = new Mock<IAsyncQueryHandler<CreateBereavedDiscussionEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var invalidRequest = new PutBereavedDiscussionEventRequest
            {
                EventId = null,
                //EventStatus = MedicalExaminer.Models.Enums.EventStatus.Final,
                DiscussionDetails = null
            };

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new BereavedDiscussionEventController(logger.Object, mapper.Object, 
               BereavedDiscussionEventCreationService.Object, examinationRetrievalQueryService.Object);

            sut.ModelState.AddModelError("i", "broke it");
            // Act
            var response = await sut.UpsertNewBereavedDiscussionEvent("a", invalidRequest);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutBereavedDiscussionEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutBereavedDiscussionEventResponse>();
        }

        [Fact]
        public async void Put_Final_BereavedDiscussion_Event_Valid_Request_Object_Cannot_Find_Examination()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();

            var BereavedDiscussionEventCreationService = new Mock<IAsyncQueryHandler<CreateBereavedDiscussionEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var validRequest = new PutBereavedDiscussionEventRequest
            {
                EventId = "1",
                //EventStatus = MedicalExaminer.Models.Enums.EventStatus.Final,
                DiscussionDetails = "Hello Planet"
            };

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new BereavedDiscussionEventController(logger.Object, mapper.Object, 
               BereavedDiscussionEventCreationService.Object, examinationRetrievalQueryService.Object);

            // Act
            var response = await sut.UpsertNewBereavedDiscussionEvent("a", validRequest);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutBereavedDiscussionEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutBereavedDiscussionEventResponse>();
        }

        [Fact]
        public async void Put_Final_BereavedDiscussion_Event_Valid_Request_Object_Finds_Examination_Then_Ok_Result()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var examination = new Mock<Examination>();
            var BereavedDiscussionEventCreationService = new Mock<IAsyncQueryHandler<CreateBereavedDiscussionEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var validRequest = new PutBereavedDiscussionEventRequest
            {
                EventId = "1",
                //EventStatus = MedicalExaminer.Models.Enums.EventStatus.Final,
                DiscussionDetails = "Hello Planet"
            };
            BereavedDiscussionEventCreationService.Setup(service => service.Handle(It.IsAny<CreateBereavedDiscussionEventQuery>()))
                .Returns(Task.FromResult("hi mark")).Verifiable();

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examination.Object)).Verifiable();

            var sut = new BereavedDiscussionEventController(logger.Object, mapper.Object, 
               BereavedDiscussionEventCreationService.Object, examinationRetrievalQueryService.Object);

            // Act
            var response = await sut.UpsertNewBereavedDiscussionEvent("a", validRequest);

            BereavedDiscussionEventCreationService.Verify(x => x.Handle(It.IsAny<CreateBereavedDiscussionEventQuery>()), Times.Once);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutBereavedDiscussionEventResponse>>().Subject;
            var goodRequestResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            goodRequestResult.Value.Should().BeAssignableTo<PutBereavedDiscussionEventResponse>();
        }
    }
}
