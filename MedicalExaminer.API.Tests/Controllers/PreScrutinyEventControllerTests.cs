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
    public class PreScrutinyEventControllerTests
    {
        [Fact]
        public async void GetPreScrutinyEvent_When_Called_With_Null_Returns_Bad_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var preScrutinyCreationService = new Mock<IAsyncQueryHandler<CreatePreScrutinyEventQuery, string>>();
            var examinationRetrievalQueryService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var sut = new PreScrutinyEventController(logger.Object, mapper.Object, preScrutinyCreationService.Object, examinationRetrievalQueryService.Object);

            // Act
            var response = await sut.GetPreScrutinyEvent(null);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetPreScrutinyEventResponse>>().Subject;
            var notFoundResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            notFoundResult.Value.Should().BeAssignableTo<GetPreScrutinyEventResponse>();
        }

        [Fact]
        public async void GetPreScrutinyEvent_When_Called_With_Invalid_Examination_Id_Returns_Bad_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var preScrutinyCreationService = new Mock<IAsyncQueryHandler<CreatePreScrutinyEventQuery, string>>();
            var examinationRetrievalQueryService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var sut = new PreScrutinyEventController(logger.Object, mapper.Object, preScrutinyCreationService.Object, examinationRetrievalQueryService.Object);
            // Act
            var response = await sut.GetPreScrutinyEvent("aaaa");

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetPreScrutinyEventResponse>>().Subject;
            var notFoundResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            notFoundResult.Value.Should().BeAssignableTo<GetPreScrutinyEventResponse>();
        }

        [Fact]
        public async void GetPreScrutinyEvent_When_Called_With_Valid_Examination_Id_But_Examination_Not_Found_Returns_Not_Found_Response()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();

            var examinationId = "7E5D50CE-05BF-4A1F-AA6E-25418A723A7F";
            var preScrutinyCreationService = new Mock<IAsyncQueryHandler<CreatePreScrutinyEventQuery, string>>();
            var examinationRetrievalQueryService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new PreScrutinyEventController(logger.Object, mapper.Object, preScrutinyCreationService.Object, examinationRetrievalQueryService.Object);

            // Act
            var response = await sut.GetPreScrutinyEvent(examinationId);

            // Assert
            examinationRetrievalQueryService.Verify(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()));
            var taskResult = response.Should().BeOfType<ActionResult<GetPreScrutinyEventResponse>>().Subject;
            var notFoundResult = taskResult.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject;
            notFoundResult.Value.Should().BeAssignableTo<GetPreScrutinyEventResponse>();
        }

        [Fact]
        public async void GetPreScrutinyEvent_When_Called_With_Valid_Examination_Id_Examination_Found_Returns_All_PreScrutiny_Events()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var examinationObj = new Mock<Examination>().Object;
            var examinationId = "7E5D50CE-05BF-4A1F-AA6E-25418A723A7F";
            var getPreScrutinyResponse = new Mock<GetPreScrutinyEventResponse>().Object;
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<GetPreScrutinyEventResponse>(examinationObj)).Returns(getPreScrutinyResponse);

            var preScrutinyCreationService = new Mock<IAsyncQueryHandler<CreatePreScrutinyEventQuery, string>>();
            var examinationRetrievalQueryService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();


            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examinationObj)).Verifiable();

            var sut = new PreScrutinyEventController(logger.Object, mapper.Object, preScrutinyCreationService.Object, examinationRetrievalQueryService.Object);

            // Act
            var response = await sut.GetPreScrutinyEvent(examinationId);

            // Assert
            examinationRetrievalQueryService.Verify(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()));

            var taskResult = response.Should().BeOfType<ActionResult<GetPreScrutinyEventResponse>>().Subject;
            var okResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            okResult.Value.Should().BeAssignableTo<GetPreScrutinyEventResponse>();

        }

        [Fact]
        public async void Put_Final_PreScrutiny_Event_Null_Request_Object_Returns_Invalid_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var preScrutinyCreationService = new Mock<IAsyncQueryHandler<CreatePreScrutinyEventQuery, string>>();
            var examinationRetrievalQueryService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            
            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new PreScrutinyEventController(logger.Object, mapper.Object, preScrutinyCreationService.Object, examinationRetrievalQueryService.Object);

            // Act
            var response = await sut.UpsertNewPreScrutinyEvent("a", null);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutPreScrutinyEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutPreScrutinyEventResponse>();
        }

        [Fact]
        public async void Put_Final_PreScrutiny_Event_Invalid_Request_Object_Returns_Invalid_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var preScrutinyCreationService = new Mock<IAsyncQueryHandler<CreatePreScrutinyEventQuery, string>>();
            var examinationRetrievalQueryService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var invalidRequest = new PutPreScrutinyEventRequest
            {
                EventId = null,
                // Uncomment when merge
                //EventStatus = MedicalExaminer.Models.Enums.EventStatus.Final,
                EventText = null
            };

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new PreScrutinyEventController(logger.Object, mapper.Object, preScrutinyCreationService.Object, examinationRetrievalQueryService.Object);

            sut.ModelState.AddModelError("i", "broke it");
            // Act
            var response = await sut.UpsertNewPreScrutinyEvent("a", invalidRequest);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutPreScrutinyEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutPreScrutinyEventResponse>();
        }

        [Fact]
        public async void Put_Final_PreScrutiny_Event_Valid_Request_Object_Cannot_Find_Examination()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var preScrutinyCreationService = new Mock<IAsyncQueryHandler<CreatePreScrutinyEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var validRequest = new PutPreScrutinyEventRequest
            {
                EventId = "1",
                // Uncomment when merge
                //EventStatus = MedicalExaminer.Models.Enums.EventStatus.Final,
                EventText = "Hello Planet"
            };

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new PreScrutinyEventController(logger.Object, mapper.Object, preScrutinyCreationService.Object, examinationRetrievalQueryService.Object);

            // Act
            var response = await sut.UpsertNewPreScrutinyEvent("a", validRequest);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutPreScrutinyEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutPreScrutinyEventResponse>();
        }

        [Fact]
        public async void Put_Final_PreScrutiny_Event_Valid_Request_Object_Finds_Examination_Then_Ok_Result()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var examination = new Mock<Examination>();
            var preScrutinyCreationService = new Mock<IAsyncQueryHandler<CreatePreScrutinyEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var validRequest = new PutPreScrutinyEventRequest
            {
                EventId = "1",
                // Uncomment when merge
                //EventStatus = MedicalExaminer.Models.Enums.EventStatus.Final,
                EventText = "Hello Planet"
            };
            preScrutinyCreationService.Setup(service => service.Handle(It.IsAny<CreatePreScrutinyEventQuery>()))
                .Returns(Task.FromResult("hi mark")).Verifiable();

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examination.Object)).Verifiable();

            var sut = new PreScrutinyEventController(logger.Object, mapper.Object, preScrutinyCreationService.Object, examinationRetrievalQueryService.Object);

            // Act
            var response = await sut.UpsertNewPreScrutinyEvent("a", validRequest);

            preScrutinyCreationService.Verify(x => x.Handle(It.IsAny<CreatePreScrutinyEventQuery>()), Times.Once);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutPreScrutinyEventResponse>>().Subject;
            var goodRequestResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            goodRequestResult.Value.Should().BeAssignableTo<PutPreScrutinyEventResponse>();
        }
    }
}
