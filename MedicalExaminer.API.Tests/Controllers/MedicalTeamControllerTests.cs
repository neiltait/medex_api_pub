using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.API.Models.v1.MedicalTeams;
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
    public class MedicalTeamControllerTests : ControllerTestsBase<ExaminationsController>
    {
        private PostNewCaseRequest CreateValidNewCaseRequest()
        {
            return new PostNewCaseRequest
            {
                GivenNames = "A",
                Surname = "Patient",
                Gender = ExaminationGender.Male,
                MedicalExaminerOfficeResponsible = "7"
            };
        }

        private Examination CreateValidExamination()
        {
            var examination = new Examination
            {
                Gender = ExaminationGender.Male,
                Surname = "Patient",
                GivenNames = "Barry"
            };
            return examination;
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

            var createExaminationService = new Mock<IAsyncQueryHandler<CreateExaminationQuery, Examination>>();
            var examinationsRetrievalQuery =
                new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>>();
            var medicalTeamUpdateService = new Mock<IAsyncUpdateDocumentHandler>();
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();

            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examination));

            var sut = new MedicalTeamController(logger.Object, mapper.Object, createExaminationService.Object,
                examinationRetrievalService.Object, examinationsRetrievalQuery.Object, medicalTeamUpdateService.Object);

            // Act
            var response = sut.GetMedicalTeam(examinationId).Result;


            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetMedicalTeamResponse>>().Subject;
            var notFoundResult = taskResult.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject;
        }

        [Fact]
        public void GetMedicalTeam_ExaminationNotFound_Returns_Expected_Type()
        {
            // Arrange
            var examinationId = Guid.NewGuid().ToString();
            Examination examination = null;
            var createExaminationService = new Mock<IAsyncQueryHandler<CreateExaminationQuery, Examination>>();
            var examinationsRetrievalQuery =
                new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>>();
            var medicalTeamUpdateService = new Mock<IAsyncUpdateDocumentHandler>();
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();

            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examination));

            var sut = new MedicalTeamController(
                logger.Object, 
                mapper.Object, 
                createExaminationService.Object,
                examinationRetrievalService.Object, 
                examinationsRetrievalQuery.Object, 
                medicalTeamUpdateService.Object);

            // Act
            var response = sut.GetMedicalTeam(examinationId).Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetMedicalTeamResponse>>().Subject;
            var notFound = taskResult.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject;
        }

        [Fact]
        public void GetMedicalTeam_Ok()
        {
            // Arrange
            var examinationId = Guid.NewGuid().ToString();
            var examination = new Examination
            {
                ExaminationId = examinationId
            };

            var medicalTeam = new MedicalTeam();
            examination.MedicalTeam = medicalTeam;
            var createExaminationService = new Mock<IAsyncQueryHandler<CreateExaminationQuery, Examination>>();
            var examinationsRetrievalQuery =
                new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>>();
            var medicalTeamUpdateService = new Mock<IAsyncUpdateDocumentHandler>();
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();

            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examination));

            var sut = new MedicalTeamController(logger.Object, mapper.Object, createExaminationService.Object,
                examinationRetrievalService.Object, examinationsRetrievalQuery.Object, medicalTeamUpdateService.Object);

            // Act
            var response = sut.GetMedicalTeam(examinationId).Result;


            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetMedicalTeamResponse>>().Subject;
            var okResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
        }

        [Fact]
        public void PostMedicalTeam_Invalid_ExaminationId_Returns_Expected_Result()
        {
            // Arrange
            var examinationId = Guid.NewGuid().ToString();
            Examination examination = null;

            var createExaminationService = new Mock<IAsyncQueryHandler<CreateExaminationQuery, Examination>>();
            var examinationsRetrievalQuery =
                new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>>();
            var medicalTeamUpdateService = new Mock<IAsyncUpdateDocumentHandler>();
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var postMedicalTeamRequest = new PostMedicalTeamRequest();
            var medicalTeam = new MedicalTeam();


            mapper.Setup(m => m.Map<MedicalTeam>(It.IsAny<PostMedicalTeamRequest>())).Returns(medicalTeam);
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examination));
            medicalTeamUpdateService.Setup(u => u.Handle(It.IsAny<Examination>()))
                .Returns(Task.FromResult(examinationId));
            var sut = new MedicalTeamController(logger.Object, mapper.Object, createExaminationService.Object,
                examinationRetrievalService.Object, examinationsRetrievalQuery.Object, medicalTeamUpdateService.Object);

            // Act
            var response = sut.PostMedicalTeam(examinationId, postMedicalTeamRequest).Result;

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

            var createExaminationService = new Mock<IAsyncQueryHandler<CreateExaminationQuery, Examination>>();
            var examinationsRetrievalQuery =
                new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>>();
            var medicalTeamUpdateService = new Mock<IAsyncUpdateDocumentHandler>();
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var postMedicalTeamRequest = new PostMedicalTeamRequest();
            var medicalTeam = new MedicalTeam();
            string returnedMedicalTeamExaminationId = null;


            mapper.Setup(m => m.Map<MedicalTeam>(It.IsAny<PostMedicalTeamRequest>())).Returns(medicalTeam);
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examination));
            medicalTeamUpdateService.Setup(u => u.Handle(It.IsAny<Examination>()))
                .Returns(Task.FromResult(returnedMedicalTeamExaminationId));
            var sut = new MedicalTeamController(logger.Object, mapper.Object, createExaminationService.Object,
                examinationRetrievalService.Object, examinationsRetrievalQuery.Object, medicalTeamUpdateService.Object);

            // Act
            var response = sut.PostMedicalTeam(examinationId, postMedicalTeamRequest).Result;


            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutMedicalTeamResponse>>().Subject;
            var badResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
        }

        [Fact]
        public void PostMedicalTeam_OK()
        {
            // Arrange
            var examinationId = Guid.NewGuid().ToString();
            var examination = new Examination
            {
                ExaminationId = examinationId
            };

            var createExaminationService = new Mock<IAsyncQueryHandler<CreateExaminationQuery, Examination>>();
            var examinationsRetrievalQuery =
                new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>>();
            var medicalTeamUpdateService = new Mock<IAsyncUpdateDocumentHandler>();
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var postMedicalTeamRequest = new PostMedicalTeamRequest();
            var medicalTeam = new MedicalTeam();


            mapper.Setup(m => m.Map<MedicalTeam>(It.IsAny<PostMedicalTeamRequest>())).Returns(medicalTeam);
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examination));
            medicalTeamUpdateService.Setup(u => u.Handle(It.IsAny<Examination>()))
                .Returns(Task.FromResult(examinationId));
            var sut = new MedicalTeamController(logger.Object, mapper.Object, createExaminationService.Object,
                examinationRetrievalService.Object, examinationsRetrievalQuery.Object, medicalTeamUpdateService.Object);

            // Act
            var response = sut.PostMedicalTeam(examinationId, postMedicalTeamRequest).Result;


            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutMedicalTeamResponse>>().Subject;
            var okResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;

            var examinationIdReturned = ((PutMedicalTeamResponse)okResult.Value).ExaminationId;
            Assert.Equal(examinationId, examinationIdReturned);
        }
    }
}