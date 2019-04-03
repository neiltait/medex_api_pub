using System;
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
    public class OtherEventsControllerTests
    {
        [Fact]
        public async void GetOtherEvent_When_Called_With_Null_Returns_Bad_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var otherEventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var sut = new OtherEventController(logger.Object, mapper.Object, 
                otherEventCreationService.Object, examinationRetrievalQueryService.Object);
            // Act
            var response = await sut.GetOtherEvent(null);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetOtherEventResponse>>().Subject;
            var notFoundResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            notFoundResult.Value.Should().BeAssignableTo<GetOtherEventResponse>();
        }

        [Fact]
        public async void GetOtherEvent_When_Called_With_Invalid_Examination_Id_Returns_Bad_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var otherEventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var sut = new OtherEventController(logger.Object, mapper.Object, 
                otherEventCreationService.Object, examinationRetrievalQueryService.Object);
            // Act
            var response = await sut.GetOtherEvent("aaaa");

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetOtherEventResponse>>().Subject;
            var notFoundResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            notFoundResult.Value.Should().BeAssignableTo<GetOtherEventResponse>();
        }

        [Fact]
        public async void GetOtherEvent_When_Called_With_Valid_Examination_Id_But_Examination_Not_Found_Returns_Not_Found_Response()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();

            var examinationId = "7E5D50CE-05BF-4A1F-AA6E-25418A723A7F";
            
            var otherEventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
                       
            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new OtherEventController(logger.Object, mapper.Object,
               otherEventCreationService.Object, examinationRetrievalQueryService.Object);

            // Act
            var response = await sut.GetOtherEvent(examinationId);

            // Assert
            examinationRetrievalQueryService.Verify(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()));
            var taskResult = response.Should().BeOfType<ActionResult<GetOtherEventResponse>>().Subject;
            var notFoundResult = taskResult.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject;
            notFoundResult.Value.Should().BeAssignableTo<GetOtherEventResponse>();
        }

        [Fact]
        public async void GetOtherEvent_When_Called_With_Valid_Examination_Id_Examination_Found_Returns_All_Other_Events()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var examinationObj = new Mock<Examination>().Object;
            var examinationId = "7E5D50CE-05BF-4A1F-AA6E-25418A723A7F";
            var getOtherResponse = new Mock<GetOtherEventResponse>().Object;
            
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<GetOtherEventResponse>(examinationObj)).Returns(getOtherResponse);

            var otherEventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();


            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examinationObj)).Verifiable();

            var sut = new OtherEventController(logger.Object, mapper.Object,
               otherEventCreationService.Object, examinationRetrievalQueryService.Object);

            // Act
            var response = await sut.GetOtherEvent(examinationId);

            // Assert
            examinationRetrievalQueryService.Verify(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()));

            var taskResult = response.Should().BeOfType<ActionResult<GetOtherEventResponse>>().Subject;
            var okResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            okResult.Value.Should().BeAssignableTo<GetOtherEventResponse>();

        }

        [Fact]
        public async void Put_Final_Other_Event_Null_Request_Object_Returns_Invalid_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            
            var otherEventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();


            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new OtherEventController(logger.Object, mapper.Object,
               otherEventCreationService.Object, examinationRetrievalQueryService.Object);

            // Act
            var response = await sut.UpsertNewOtherEvent("a", null);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutOtherEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutOtherEventResponse>();
        }

        [Fact]
        public async void Put_Final_Other_Event_Invalid_Request_Object_Returns_Invalid_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();

            var otherEventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var invalidRequest = new PutOtherEventRequest
            {
             EventId = null,
             EventStatus = MedicalExaminer.Models.Enums.EventStatus.Final,
             EventText = null
            };

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new OtherEventController(logger.Object, mapper.Object,
               otherEventCreationService.Object, examinationRetrievalQueryService.Object);

            sut.ModelState.AddModelError("i", "broke it");
            // Act
            var response = await sut.UpsertNewOtherEvent("a", invalidRequest);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutOtherEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutOtherEventResponse>();
        }

        [Fact]
        public async void Put_Final_Other_Event_Valid_Request_Object_Cannot_Find_Examination()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();

            var otherEventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var validRequest = new PutOtherEventRequest
            {
                EventId = "1",
                EventStatus = MedicalExaminer.Models.Enums.EventStatus.Final,
                EventText = "Hello Planet"
            };

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new OtherEventController(logger.Object, mapper.Object,
               otherEventCreationService.Object, examinationRetrievalQueryService.Object);

            // Act
            var response = await sut.UpsertNewOtherEvent("a", validRequest);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutOtherEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutOtherEventResponse>();
        }

        [Fact]
        public async void Put_Final_Other_Event_Valid_Request_Object_Finds_Examination_Then_Ok_Result()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var examination = new Mock<Examination>();
            var otherEventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var validRequest = new PutOtherEventRequest
            {
                EventId = "1",
                EventStatus = MedicalExaminer.Models.Enums.EventStatus.Final,
                EventText = "Hello Planet"
            };
            otherEventCreationService.Setup(service => service.Handle(It.IsAny<CreateEventQuery>()))
                .Returns(Task.FromResult("hi mark")).Verifiable();

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examination.Object)).Verifiable();

            var sut = new OtherEventController(logger.Object, mapper.Object,
               otherEventCreationService.Object, examinationRetrievalQueryService.Object);

            // Act
            var response = await sut.UpsertNewOtherEvent("a", validRequest);

            otherEventCreationService.Verify(x => x.Handle(It.IsAny<CreateEventQuery>()), Times.Once);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutOtherEventResponse>>().Subject;
            var goodRequestResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            goodRequestResult.Value.Should().BeAssignableTo<PutOtherEventResponse>();
        }
    }
}
