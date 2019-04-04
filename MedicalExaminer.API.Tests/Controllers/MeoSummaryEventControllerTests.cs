using System;
using System.Collections.Generic;
using System.Text;
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
    public class MeoSummaryEventControllerTests
    {
        [Fact]
        public async void Put_Final_MeoSummary_Event_Null_Request_Object_Returns_Invalid_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();

            var meoSummaryEventCreationService = new Mock<IAsyncQueryHandler<CreateMeoSummaryEventQuery, string>>();
            var examinationRetrievalQueryService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();


            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
               .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new MeoSummaryEventController(logger.Object, mapper.Object,
               meoSummaryEventCreationService.Object, examinationRetrievalQueryService.Object);

            // Act
            var response = await sut.UpsertNewMeoSummaryEvent("a", null);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutMeoSummaryEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutMeoSummaryEventResponse>();
        }

        [Fact]
        public async void Put_Final_MeoSummary_Event_Invalid_Request_Object_Returns_Invalid_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();

            var meoSummaryEventCreationService = new Mock<IAsyncQueryHandler<CreateMeoSummaryEventQuery, string>>();
            var examinationRetrievalQueryService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var invalidRequest = new PutMeoSummaryEventRequest
            {
                EventId = null,
                //EventStatus = MedicalExaminer.Models.Enums.EventStatus.Final,
                SummaryDetails = null
            };

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
               .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new MeoSummaryEventController(logger.Object, mapper.Object,
               meoSummaryEventCreationService.Object, examinationRetrievalQueryService.Object);

            sut.ModelState.AddModelError("i", "broke it");
            // Act
            var response = await sut.UpsertNewMeoSummaryEvent("a", invalidRequest);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutMeoSummaryEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutMeoSummaryEventResponse>();
        }

        [Fact]
        public async void Put_Final_MeoSummary_Event_Valid_Request_Object_Cannot_Find_Examination()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();

            var meoSummaryEventCreationService = new Mock<IAsyncQueryHandler<CreateMeoSummaryEventQuery, string>>();
            var examinationRetrievalQueryService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var validRequest = new PutMeoSummaryEventRequest
            {
                EventId = "1",
                //EventStatus = MedicalExaminer.Models.Enums.EventStatus.Final,
                SummaryDetails = "Hello Planet"
            };

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
               .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new MeoSummaryEventController(logger.Object, mapper.Object,
               meoSummaryEventCreationService.Object, examinationRetrievalQueryService.Object);

            // Act
            var response = await sut.UpsertNewMeoSummaryEvent("a", validRequest);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutMeoSummaryEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutMeoSummaryEventResponse>();
        }

        [Fact]
        public async void Put_Final_MeoSummary_Event_Valid_Request_Object_Finds_Examination_Then_Ok_Result()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var examination = new Mock<Examination>();

            var meoSummaryEventCreationService = new Mock<IAsyncQueryHandler<CreateMeoSummaryEventQuery, string>>();
            var examinationRetrievalQueryService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var validRequest = new PutMeoSummaryEventRequest
            {
                EventId = "1",
                //EventStatus = MedicalExaminer.Models.Enums.EventStatus.Final,
                SummaryDetails = "Hello Planet"
            };
            meoSummaryEventCreationService
                .Setup(service => service.Handle(It.IsAny<CreateMeoSummaryEventQuery>()))
                .Returns(Task.FromResult("hi mark")).Verifiable();

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
               .Returns(Task.FromResult(examination.Object)).Verifiable();

            var sut = new MeoSummaryEventController(logger.Object, mapper.Object,
               meoSummaryEventCreationService.Object, examinationRetrievalQueryService.Object);

            // Act
            var response = await sut.UpsertNewMeoSummaryEvent("a", validRequest);

            meoSummaryEventCreationService.Verify(x => x.Handle(It.IsAny<CreateMeoSummaryEventQuery>()),
               Times.Once);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutMeoSummaryEventResponse>>().Subject;
            var goodRequestResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            goodRequestResult.Value.Should().BeAssignableTo<PutMeoSummaryEventResponse>();
        }
    }
}
