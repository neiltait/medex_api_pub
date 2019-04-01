using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.API.Models.v1.MedicalTeams;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Controllers
{
    /// <summary>
    /// Medical Team Controller Tests.
    /// </summary>
    public class MedicalTeamControllerTests : ControllerTestsBase<MedicalTeamController>
    {       
        [Fact]
        public async Task GetMedicalTeam_ShouldReturnBadRequest_WhenModelStateInvalid()
        {
            // Arrange
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var medicalTeamUpdateService = new Mock<IAsyncUpdateDocumentHandler>();
            var logger = new Mock<IMELogger>();

            Controller = new MedicalTeamController(logger.Object, Mapper, examinationRetrievalService.Object, medicalTeamUpdateService.Object);
            Controller.ModelState.AddModelError("An", "Error");

            // Act
            var request = "request";
            var response = await Controller.GetMedicalTeam(request);

            // Assert
            response.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            var result = (BadRequestObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetMedicalTeamResponse>();
            var model = (GetMedicalTeamResponse)result.Value;
            model.Errors.Count.Should().Be(1);
            model.Success.Should().BeFalse();
        }

        [Fact]
        public void GetMedical_When_Called_With_No_MedicalTeam_InExamination_Returns_Expected_Type()
        {
            // Arrange
            var examinationId = Guid.NewGuid().ToString();
            var examination = new Examination
            {
                ExaminationId = examinationId
            };

            var medicalTeamUpdateService = new Mock<IAsyncUpdateDocumentHandler>();
            var logger = new Mock<IMELogger>();

            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examination));

            Controller = new MedicalTeamController(logger.Object, Mapper, examinationRetrievalService.Object,  medicalTeamUpdateService.Object);

            // Act
            var response = Controller.GetMedicalTeam(examinationId).Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetMedicalTeamResponse>>().Subject;
            var emptyResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            var examinationIdReturned = (GetMedicalTeamResponse)emptyResult.Value;
            examinationIdReturned.MedicalExaminerUserId.Should().BeNull();
            examinationIdReturned.MedicalExaminerOfficerUserId.Should().BeNull();
            examinationIdReturned.NursingTeamInformation.Should().BeEmpty();
        }

        [Fact]
        public void GetMedicalTeam_ExaminationNotFound_Returns_Expected_Type()
        {
            // Arrange
            var examinationId = Guid.NewGuid().ToString();
            Examination examination = null;
            var medicalTeamUpdateService = new Mock<IAsyncUpdateDocumentHandler>();
            var logger = new Mock<IMELogger>();

            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examination));

            Controller = new MedicalTeamController(
                logger.Object,
                Mapper,
                examinationRetrievalService.Object,
                medicalTeamUpdateService.Object);

            // Act
            var response = Controller.GetMedicalTeam(examinationId).Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetMedicalTeamResponse>>().Subject;
            var emptyResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            var examinationIdReturned = (GetMedicalTeamResponse)emptyResult.Value;
            examinationIdReturned.MedicalExaminerUserId.Should().BeNull();
            examinationIdReturned.MedicalExaminerOfficerUserId.Should().BeNull();
            examinationIdReturned.NursingTeamInformation.Should().BeEmpty();
        }

        [Fact]
        public void GetMedicalTeam_Ok()
        {
            // Arrange
            var expectedNursingTeamInformation = "expectedNursingTeamInformation";
            var examinationId = Guid.NewGuid().ToString();
            var examination = new Examination
            {
                ExaminationId = examinationId,
                MedicalTeam = new MedicalTeam()
                {
                    NursingTeamInformation = expectedNursingTeamInformation,
                }

            };

            var medicalTeamUpdateService = new Mock<IAsyncUpdateDocumentHandler>();
            var logger = new Mock<IMELogger>();

            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examination));

            Controller = new MedicalTeamController(logger.Object, Mapper, examinationRetrievalService.Object, medicalTeamUpdateService.Object);

            // Act
            var response = Controller.GetMedicalTeam(examinationId).Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetMedicalTeamResponse>>().Subject;
            var okResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            ((GetMedicalTeamResponse)okResult.Value).NursingTeamInformation.Should().Be(expectedNursingTeamInformation);

        }

        [Fact]
        public async Task PutMedicalTeam_ShouldReturnBadRequest_WhenModelStateInvalid()
        {
            // Arrange
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var medicalTeamUpdateService = new Mock<IAsyncUpdateDocumentHandler>();
            var logger = new Mock<IMELogger>();

            Controller = new MedicalTeamController(logger.Object, Mapper, examinationRetrievalService.Object, medicalTeamUpdateService.Object);
            Controller.ModelState.AddModelError("An", "Error");
            var examinationId = "examinationId";
            var request = new PutMedicalTeamRequest();

            // Act
            var response = await Controller.PutMedicalTeam(examinationId, request);

            // Assert
            response.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            var result = (BadRequestObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<PutMedicalTeamResponse>();
            var model = (PutMedicalTeamResponse)result.Value;
            model.Errors.Count.Should().Be(1);
            model.Success.Should().BeFalse();
        }

        [Fact]
        public async Task PutMedicalTeam_ShouldReturnBadRequest_WhenMappingFails()
        {
            // Arrange
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var medicalTeamUpdateService = new Mock<IAsyncUpdateDocumentHandler>();
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();

            Controller = new MedicalTeamController(logger.Object, mapper.Object, examinationRetrievalService.Object, medicalTeamUpdateService.Object);
            var examinationId = "examinationId";
            var request = new PutMedicalTeamRequest();

            mapper
                .Setup(m => m.Map<MedicalTeam>(request))
                .Returns((MedicalTeam)null);

            // Act
            var response = await Controller.PutMedicalTeam(examinationId, request);

            // Assert
            response.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            var result = (BadRequestObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<PutMedicalTeamResponse>();
            var model = (PutMedicalTeamResponse)result.Value;
        }

        [Fact]
        public void PostMedicalTeam_Invalid_ExaminationId_Returns_Expected_Result()
        {
            // Arrange
            var examinationId = Guid.NewGuid().ToString();
            Examination examination = null;

            var medicalTeamUpdateService = new Mock<IAsyncUpdateDocumentHandler>();
            var logger = new Mock<IMELogger>();
            var postMedicalTeamRequest = new PutMedicalTeamRequest();
            var medicalTeam = new MedicalTeam();

            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examination));
            medicalTeamUpdateService.Setup(u => u.Handle(It.IsAny<Examination>()))
                .Returns(Task.FromResult(examination));
            Controller = new MedicalTeamController(logger.Object, Mapper, examinationRetrievalService.Object, medicalTeamUpdateService.Object);

            // Act
            var response = Controller.PutMedicalTeam(examinationId, postMedicalTeamRequest).Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutMedicalTeamResponse>>().Subject;
            var notFoundResult = taskResult.Result.Should().BeAssignableTo<NotFoundResult>().Subject;
        }

        [Fact]
        public void PostMedicalTeam_MedicalteamUpdateServiceErrors_Returns_Expected_Type()
        {
            // Arrange
            var examinationId = Guid.NewGuid().ToString();
            var examination = new Examination
            {
                ExaminationId = examinationId
            };
            var nullExamination = (Examination)null;

            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var putMedicalTeamRequest = new PutMedicalTeamRequest();
            var medicalTeam = new MedicalTeam();

            mapper
                .Setup(m => m.Map<MedicalTeam>(It.IsAny<PutMedicalTeamRequest>()))
                .Returns(medicalTeam);

            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService
                .Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examination));

            var medicalTeamUpdateService = new Mock<IAsyncUpdateDocumentHandler>();
            medicalTeamUpdateService
                .Setup(u => u.Handle(It.IsAny<Examination>()))
                .Returns(Task.FromResult(nullExamination));

            Controller = new MedicalTeamController(
                logger.Object,
                mapper.Object,
                examinationRetrievalService.Object,
                medicalTeamUpdateService.Object);

            // Act
            var response = Controller.PutMedicalTeam(examinationId, putMedicalTeamRequest).Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutMedicalTeamResponse>>().Subject;
            var badResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
        }

        [Fact]
        public void PostMedicalTeam_OK()
        {
            // Arrange
            var examinationId = Guid.NewGuid().ToString();
            var expectedNursingTeamInformation = "expectedNursingTeamInformation";

            var examination = new Examination
            {
                ExaminationId = examinationId,
            };

            var medicalTeamUpdateService = new Mock<IAsyncUpdateDocumentHandler>();
            var logger = new Mock<IMELogger>();
            var postMedicalTeamRequest = new PutMedicalTeamRequest()
            {
                NursingTeamInformation = expectedNursingTeamInformation,
            };

            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examination));
            medicalTeamUpdateService.Setup(u => u.Handle(It.IsAny<Examination>()))
                .Returns(Task.FromResult(examination));

            Controller = new MedicalTeamController(logger.Object, Mapper, examinationRetrievalService.Object,medicalTeamUpdateService.Object);

            // Act
            var response = Controller.PutMedicalTeam(examinationId, postMedicalTeamRequest).Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutMedicalTeamResponse>>().Subject;
            var okResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;

            ((PutMedicalTeamResponse)okResult.Value).NursingTeamInformation.Should().Be(expectedNursingTeamInformation);
        }
    }
}