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
    public class EventsControllerTests
    {
        [Fact]
        public async void GetOtherEvent_When_Called_With_Null_Returns_Bad_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var otherEventByCaseIdService = new Mock<IAsyncQueryHandler<OtherCaseEventByCaseIdQuery, OtherEvent>>();
            var otherEventCreationService = new Mock<IAsyncQueryHandler<CreateOtherEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var sut = new OtherEventController(logger.Object, mapper.Object, otherEventByCaseIdService.Object,
                otherEventCreationService.Object, examinationRetrievalQueryService.Object);
            // Act
            var response = await sut.GetOtherEvent(null, null);

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
            var otherEventByCaseIdService = new Mock<IAsyncQueryHandler<OtherCaseEventByCaseIdQuery, OtherEvent>>();
            var otherEventCreationService = new Mock<IAsyncQueryHandler<CreateOtherEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var sut = new OtherEventController(logger.Object, mapper.Object, otherEventByCaseIdService.Object,
                otherEventCreationService.Object, examinationRetrievalQueryService.Object);
            // Act
            var response = await sut.GetOtherEvent("aaaa", null);

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
            
            var otherEventByCaseIdService = new Mock<IAsyncQueryHandler<OtherCaseEventByCaseIdQuery, OtherEvent>>();
            var otherEventCreationService = new Mock<IAsyncQueryHandler<CreateOtherEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
                       
            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new OtherEventController(logger.Object, mapper.Object, otherEventByCaseIdService.Object,
               otherEventCreationService.Object, examinationRetrievalQueryService.Object);

            // Act
            var response = await sut.GetOtherEvent(examinationId, null);

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
            //var otherEvents = new[] {new OtherEvent() {
            //    EventId = "a",
            //    EventStatus = MedicalExaminer.Models.Enums.EventStatus.Final,
            //    EventText = "please work"
            //}
            //};
            //var events = new CaseBreakDown()
            //{
            //    OtherEvents = otherEvents
            //};
            //var examinationObj = new Examination
            //{
            //    ExaminationId = examinationId,
            //    Events = events
            //};

            var getOtherResponse = new Mock<GetOtherEventResponse>().Object;
            
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<GetOtherEventResponse>(examinationObj)).Returns(getOtherResponse);

            var otherEventByCaseIdService = new Mock<IAsyncQueryHandler<OtherCaseEventByCaseIdQuery, OtherEvent>>();
            var otherEventCreationService = new Mock<IAsyncQueryHandler<CreateOtherEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();


            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examinationObj)).Verifiable();

            var sut = new OtherEventController(logger.Object, mapper.Object, otherEventByCaseIdService.Object,
               otherEventCreationService.Object, examinationRetrievalQueryService.Object);



            // Act
            var response = await sut.GetOtherEvent(examinationId, null);

            // Assert
            examinationRetrievalQueryService.Verify(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()));

            var taskResult = response.Should().BeOfType<ActionResult<GetOtherEventResponse>>().Subject;
            var okResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            okResult.Value.Should().BeAssignableTo<GetOtherEventResponse>();

        }
    }
}
