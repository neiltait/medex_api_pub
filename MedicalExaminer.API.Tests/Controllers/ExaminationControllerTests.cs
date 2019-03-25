﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Models.v1.Examinations;
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
    public class ExaminationControllerTests : ControllerTestsBase<ExaminationsController>
    {
        [Fact]
        public void GetExamination_When_Called_With_Invalid_Id_Returns_Expected_Type()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var createExaminationService = new Mock<IAsyncQueryHandler<CreateExaminationQuery, Examination>>();
            var examinationRetrievalQuery = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var examinationsRetrievalQuery = new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>>();
			var medicaTeamUpdateService = new Mock<IAsyncUpdateDocumentHandler>();
            var examinationsDashboardService = new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview>>();

            var sut = new ExaminationsController(logger.Object, mapper.Object, createExaminationService.Object,
                examinationRetrievalQuery.Object, examinationsRetrievalQuery.Object, medicaTeamUpdateService.Object, examinationsDashboardService.Object);
                       
            // Act
            var response = sut.GetExamination("dfgdfgdfg");

            // Assert
            var taskResult = response.Should().BeOfType<Task<ActionResult<GetExaminationResponse>>>().Subject;
            Assert.Equal(TaskStatus.RanToCompletion, taskResult.Status);
        }

        [Fact]
        public async void GetExamination_When_Called_With_Valid_Id_Returns_Expected_Type()
        {
            // Arrange
            Examination examinationObj = new Examination()
            {
                Id = "a"
            };
            var getResponse = new GetExaminationResponse()
            {
                Id = "a"
            };
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<GetExaminationResponse>(examinationObj)).Returns(getResponse);
            var createExaminationService = new Mock<IAsyncQueryHandler<CreateExaminationQuery, Examination>>();
            var examinationRetrievalQueryService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var examinationsRetrievalQueryService = new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>>();
            var examinationsDashboardService = new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview>>();
            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examinationObj));
            var medicaTeamUpdateService = new Mock<IAsyncUpdateDocumentHandler>();
            var sut = new ExaminationsController(logger.Object, mapper.Object, createExaminationService.Object,
                examinationRetrievalQueryService.Object, examinationsRetrievalQueryService.Object, medicaTeamUpdateService.Object, examinationsDashboardService.Object);

            // Act
            var response = sut.GetExamination("a").Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetExaminationResponse>>().Subject;
            var okResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            okResult.Value.Should().BeAssignableTo<GetExaminationResponse>();
            Assert.Equal("a", ((GetExaminationResponse)okResult.Value).Id);
        }

        [Fact]
        public void GetExaminations_When_Called_Returns_Expected_Type()
        {
            // Arrange
            var examination1 = new Examination();
            var examination2 = new Examination();
            IEnumerable<Examination> examinationsResult = new List<Examination>() { examination1, examination2 };
            var er = new Mock<GetExaminationsResponse>();
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<GetExaminationsResponse>(It.IsAny<Examination>())).Returns(er.Object);
            var createExaminationService = new Mock<IAsyncQueryHandler<CreateExaminationQuery, Examination>>();
            var examinationRetrievalQueryService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var examinationsRetrievalQueryService = new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>>();
            var examinationsDashboardService = new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview>>();

            examinationsRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationsRetrievalQuery>()))
                .Returns(Task.FromResult(examinationsResult));
            var medicaTeamUpdateService = new Mock<IAsyncUpdateDocumentHandler>();
            var sut = new ExaminationsController(logger.Object, mapper.Object, createExaminationService.Object,
                examinationRetrievalQueryService.Object, examinationsRetrievalQueryService.Object, medicaTeamUpdateService.Object, examinationsDashboardService.Object);

            // Act
            var response = sut.GetExaminations(null).Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetExaminationsResponse>>().Subject;
            var okResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            //var examinationResponse = okResult..Value.Should().BeAssignableTo<GetExaminationsResponse>().Subject;
            //var examinations = examinationResponse.Examinations;
            //Assert.Equal(2, examinations.Count());
        }

        [Fact]
        public void TestCreateCaseValidationFailure()
        {
            // Arrange
            var examination = CreateValidExamination();
            var createExaminationService = new Mock<IAsyncQueryHandler<CreateExaminationQuery, Examination>>();
            var examinationRetrievalQuery = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var examinationsRetrievalQuery = new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>>();
            var examinationsDashboardService = new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview>>();
            var medicaTeamUpdateService = new Mock<IAsyncUpdateDocumentHandler>();

            var examinationId = Guid.NewGuid();

            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<Examination>(It.IsAny<PostNewCaseRequest>())).Returns(examination);
            var sut = new ExaminationsController(logger.Object, mapper.Object, createExaminationService.Object, examinationRetrievalQuery.Object

                , examinationsRetrievalQuery.Object, medicaTeamUpdateService.Object, examinationsDashboardService.Object);

            sut.ModelState.AddModelError("test", "test");

            // Act
            var response = sut.CreateNewCase(CreateValidNewCaseRequest()).Result;

            // Assert
            response.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            var result = (BadRequestObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<PutExaminationResponse>();
            var model = (PutExaminationResponse)result.Value;
            model.Errors.Count.Should().Be(1);
            model.Success.Should().BeFalse();
        }

        [Fact]
        public void ValidModelStateReturnsOkResponse()
        {
            // Arrange
            var examination = CreateValidExamination();
            var createExaminationService = new Mock<IAsyncQueryHandler<CreateExaminationQuery, Examination>>();
            var examinationRetrievalQuery = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var examinationsRetrievalQuery = new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>>();

            var examinationsDashboardService = new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview>>();
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
			var medicaTeamUpdateService = new Mock<IAsyncUpdateDocumentHandler>();
            var examinationId = Guid.NewGuid();
			
            createExaminationService.Setup(ecs => ecs.Handle(It.IsAny<CreateExaminationQuery>()))
                .Returns(Task.FromResult(examination));

            mapper.Setup(m => m.Map<Examination>(It.IsAny<PostNewCaseRequest>())).Returns(examination);
            var sut = new ExaminationsController(logger.Object, mapper.Object, createExaminationService.Object, 
                examinationRetrievalQuery.Object, examinationsRetrievalQuery.Object, medicaTeamUpdateService.Object, examinationsDashboardService.Object);


            // Act
            var response = sut.CreateNewCase(CreateValidNewCaseRequest()).Result;

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<PutExaminationResponse>();
            var model = (PutExaminationResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();
        }

        [Fact]
        public void PostMedicalTeam_OK()
        {
            // Arrange
            var examinationId = Guid.NewGuid().ToString();
            var examination = new Examination()
            {
                Id = examinationId
            };

            var createExaminationService = new Mock<IAsyncQueryHandler<CreateExaminationQuery, Examination>>();
            var examinationsRetrievalQuery = new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>>();
            var examinationsDashboardService = new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview>>();
            var medicalTeamUpdateService = new Mock<IAsyncUpdateDocumentHandler>();
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var postMedicalTeamRequest = new PostMedicalTeamRequest();
            var medicalTeam = new MedicalTeam();


            mapper.Setup(m => m.Map<MedicalTeam>(It.IsAny<PostMedicalTeamRequest>())).Returns(medicalTeam);
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>())).Returns(Task.FromResult(examination));
            medicalTeamUpdateService.Setup(u => u.Handle(It.IsAny<Examination>()))
                .Returns(Task.FromResult(examinationId.ToString()));
            var sut = new ExaminationsController(logger.Object, mapper.Object, createExaminationService.Object,
                examinationRetrievalService.Object, examinationsRetrievalQuery.Object, medicalTeamUpdateService.Object, examinationsDashboardService.Object);

            // Act
            var response = sut.PostMedicalTeam(examinationId, postMedicalTeamRequest).Result;


            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutExaminationResponse>>().Subject;
            var okResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;

            var examinationIdReturned = ((PutExaminationResponse) okResult.Value).ExaminationId;
            Assert.Equal(examinationId, examinationIdReturned); 
        }

        [Fact]
        public void PostMedicalTeam_MedicalteamUpdateServiceErrors_Returns_Expected_Type()
        {
            // Arrange
            var examinationId = Guid.NewGuid().ToString();
            var examination = new Examination()
            {
                Id = examinationId
            };

            var createExaminationService = new Mock<IAsyncQueryHandler<CreateExaminationQuery, Examination>>();
            var examinationsRetrievalQuery = new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>>();
            var medicalTeamUpdateService = new Mock<IAsyncUpdateDocumentHandler>();
            var examinationsDashboardService = new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview>>();
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var postMedicalTeamRequest = new PostMedicalTeamRequest();
            var medicalTeam = new MedicalTeam();
            string returnedMedicalTeamExaminationId = null;


            mapper.Setup(m => m.Map<MedicalTeam>(It.IsAny<PostMedicalTeamRequest>())).Returns(medicalTeam);
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>())).Returns(Task.FromResult(examination));
            medicalTeamUpdateService.Setup(u => u.Handle(It.IsAny<Examination>()))
                .Returns(Task.FromResult(returnedMedicalTeamExaminationId));
            var sut = new ExaminationsController(logger.Object, mapper.Object, createExaminationService.Object,
                examinationRetrievalService.Object, examinationsRetrievalQuery.Object, medicalTeamUpdateService.Object, examinationsDashboardService.Object);

            // Act
            var response = sut.PostMedicalTeam(examinationId, postMedicalTeamRequest).Result;


            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutExaminationResponse>>().Subject;
            var badResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;

        }

        [Fact]
        public void PostMedicalTeam_Invalid_ExaminationId_Returns_Expected_Result()
        {
            // Arrange
            var examinationId = Guid.NewGuid().ToString();
            Examination examination = null;

            var createExaminationService = new Mock<IAsyncQueryHandler<CreateExaminationQuery, Examination>>();
            var examinationsRetrievalQuery = new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>>();
            var medicalTeamUpdateService = new Mock<IAsyncUpdateDocumentHandler>();
            var examinationsDashboardService = new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview>>();

            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var postMedicalTeamRequest = new PostMedicalTeamRequest();
            var medicalTeam = new MedicalTeam();


            mapper.Setup(m => m.Map<MedicalTeam>(It.IsAny<PostMedicalTeamRequest>())).Returns(medicalTeam);
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>())).Returns(Task.FromResult(examination));
            medicalTeamUpdateService.Setup(u => u.Handle(It.IsAny<Examination>()))
                .Returns(Task.FromResult(examinationId.ToString()));
            var sut = new ExaminationsController(logger.Object, mapper.Object, createExaminationService.Object,
                examinationRetrievalService.Object, examinationsRetrievalQuery.Object, medicalTeamUpdateService.Object, examinationsDashboardService.Object);

            // Act
            var response = sut.PostMedicalTeam(examinationId, postMedicalTeamRequest).Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutExaminationResponse>>().Subject;
            var notFoundResult = taskResult.Result.Should().BeAssignableTo<NotFoundResult>().Subject;
        }

        [Fact]
        public void GetMedicalTeam_Ok()
        {
            // Arrange
            var examinationId = Guid.NewGuid().ToString();
            var examination = new Examination()
            {
                Id = examinationId
            };

            var medicalTeam = new MedicalTeam();
            examination.MedicalTeam = medicalTeam;
            var createExaminationService = new Mock<IAsyncQueryHandler<CreateExaminationQuery, Examination>>();
            var examinationsRetrievalQuery = new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>>();
            var medicalTeamUpdateService = new Mock<IAsyncUpdateDocumentHandler>();
            var examinationsDashboardService = new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview>>();
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();

            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>())).Returns(Task.FromResult(examination));

            var sut = new ExaminationsController(logger.Object, mapper.Object, createExaminationService.Object,
                examinationRetrievalService.Object, examinationsRetrievalQuery.Object, medicalTeamUpdateService.Object, examinationsDashboardService.Object);

            // Act
            var response = sut.GetMedicalTeam(examinationId).Result;


            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetMedicalTeamResponse>>().Subject;
            var okResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
        }

        [Fact]
        public void GetMedical_When_Called_With_No_MedicalTeam_InExamination_Returns_Expected_Type()
        {
            // Arrange
            var examinationId = Guid.NewGuid().ToString();
            var examination = new Examination()
            {
                Id = examinationId
            };

            var createExaminationService = new Mock<IAsyncQueryHandler<CreateExaminationQuery, Examination>>();
            var examinationsRetrievalQuery = new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>>();
            var medicalTeamUpdateService = new Mock<IAsyncUpdateDocumentHandler>();
            var examinationsDashboardService = new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview>>();
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();

            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>())).Returns(Task.FromResult(examination));

            var sut = new ExaminationsController(logger.Object, mapper.Object, createExaminationService.Object,
                examinationRetrievalService.Object, examinationsRetrievalQuery.Object, medicalTeamUpdateService.Object, examinationsDashboardService.Object);

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
            var examinationsRetrievalQuery = new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>>();
            var medicalTeamUpdateService = new Mock<IAsyncUpdateDocumentHandler>();
            var examinationsDashboardService = new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview>>();
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();

            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>())).Returns(Task.FromResult(examination));

            var sut = new ExaminationsController(logger.Object, mapper.Object, createExaminationService.Object,
                examinationRetrievalService.Object, examinationsRetrievalQuery.Object, medicalTeamUpdateService.Object, examinationsDashboardService.Object);

            // Act
            var response = sut.GetMedicalTeam(examinationId).Result;


            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetMedicalTeamResponse>>().Subject;
            var notFound = taskResult.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject;
        }

        private PostNewCaseRequest CreateValidNewCaseRequest()
        {
            return new PostNewCaseRequest()
            {
                GivenNames = "A",
                Surname = "Patient",
                Gender = ExaminationGender.Male,
                MedicalExaminerOfficeResponsible = "7"
            };
        }

        private Examination CreateValidExamination()
        {
            var examination = new Examination()
            {
                Gender = ExaminationGender.Male,
                Surname = "Patient",
                GivenNames = "Barry",

            };
            return examination;
        }
    }
}
