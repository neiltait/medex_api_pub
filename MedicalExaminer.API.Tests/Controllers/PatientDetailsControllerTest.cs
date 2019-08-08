using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Models.v1.PatientDetails;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Queries.PatientDetails;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Controllers
{
    public class PatientDetailsControllerTest : AuthorizedControllerTestsBase<PatientDetailsController>
    {
        private readonly Mock<IAsyncQueryHandler<LocationParentsQuery, IEnumerable<Location>>> _locationParentsQueryServiceMock;

        public PatientDetailsControllerTest()
            : base(setupAuthorize: false)
        {
            _locationParentsQueryServiceMock = new Mock<IAsyncQueryHandler<LocationParentsQuery, IEnumerable<Location>>>();
        }

        [Fact]
        public void GetPatientDetails_ReturnsBadRequest_WhenModelError()
        {
            // Arrange
            var examination = new Examination
            {
                ExaminationId = "a"
            };
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var patientDetailsUpdateService = new Mock<IAsyncQueryHandler<PatientDetailsUpdateQuery, Examination>>();

            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examination));

            Controller = new PatientDetailsController(
                LoggerMock.Object,
                Mapper,
                UsersRetrievalByOktaIdServiceMock.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object,
                examinationRetrievalService.Object,
                patientDetailsUpdateService.Object,
                _locationParentsQueryServiceMock.Object);

            Controller.ModelState.AddModelError("An", "Error");

            // Act
            var response = Controller.GetPatientDetails("a").Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetPatientDetailsResponse>>().Subject;
            var badRequestObjectResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestObjectResult.Value.Should().BeAssignableTo<GetPatientDetailsResponse>();
        }

        [Fact]
        public void GetPatientDetails_When_Called_With_Id_Not_Found_Returns_NotFound()
        {
            // Arrange
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var patientDetailsUpdateService = new Mock<IAsyncQueryHandler<PatientDetailsUpdateQuery, Examination>>();

            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult((Examination)null));

            var sut = new PatientDetailsController(
                LoggerMock.Object,
                Mapper,
                UsersRetrievalByOktaIdServiceMock.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object,
                examinationRetrievalService.Object,
                patientDetailsUpdateService.Object,
                _locationParentsQueryServiceMock.Object);

            // Act
            var response = sut.GetPatientDetails("a").Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetPatientDetailsResponse>>().Subject;
            var notFoundResult = taskResult.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject;
            notFoundResult.Value.Should().BeAssignableTo<GetPatientDetailsResponse>();
        }

        [Fact]
        public void GetPatientDetails_ReturnsForbid_WhenNoAccess()
        {
            // Arrange
            SetupAuthorize(AuthorizationResult.Failed());
            var examination = new Examination
            {
                ExaminationId = "a"
            };
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var patientDetailsUpdateService = new Mock<IAsyncQueryHandler<PatientDetailsUpdateQuery, Examination>>();

            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examination));

            var sut = new PatientDetailsController(
                LoggerMock.Object,
                Mapper,
                UsersRetrievalByOktaIdServiceMock.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object,
                examinationRetrievalService.Object,
                patientDetailsUpdateService.Object,
                _locationParentsQueryServiceMock.Object);

            // Act
            var response = sut.GetPatientDetails("a").Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetPatientDetailsResponse>>().Subject;
            taskResult.Result.Should().BeAssignableTo<ForbidResult>();
        }

        [Fact]
        public void GetPatientDetails_When_Called_With_Invalid_Id_Returns_Expected_Type()
        {
            // Arrange
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var patientDetailsUpdateService = new Mock<IAsyncQueryHandler<PatientDetailsUpdateQuery, Examination>>();

            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult<Examination>(null));

            var sut = new PatientDetailsController(
                LoggerMock.Object,
                Mapper,
                UsersRetrievalByOktaIdServiceMock.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object,
                examinationRetrievalService.Object,
                patientDetailsUpdateService.Object,
                _locationParentsQueryServiceMock.Object);

            // Act
            var response = sut.GetPatientDetails("dfgdfgdfg");

            // Assert
            var taskResult = response.Should().BeOfType<Task<ActionResult<GetPatientDetailsResponse>>>().Subject;
            Assert.Equal(TaskStatus.RanToCompletion, taskResult.Status);
        }

        [Fact]
        public void GetPatientDetails_When_Called_With_Valid_Id_Returns_Expected_Type()
        {
            // Arrange
            SetupAuthorize(AuthorizationResult.Success());
            var examination = new Examination
            {
                ExaminationId = "a"
            };

            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService
                .Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examination));

            var patientDetailsUpdateService = new Mock<IAsyncQueryHandler<PatientDetailsUpdateQuery, Examination>>();

            var sut = new PatientDetailsController(
                LoggerMock.Object,
                Mapper,
                UsersRetrievalByOktaIdServiceMock.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object,
                examinationRetrievalService.Object,
                patientDetailsUpdateService.Object,
                _locationParentsQueryServiceMock.Object);

            // Act
            var response = sut.GetPatientDetails("a").Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetPatientDetailsResponse>>().Subject;
            taskResult.Result.Should().BeAssignableTo<OkObjectResult>();
        }

        [Fact]
        public void UpdatePatientDetails_ReturnsBadRequest_WhenModelError()
        {
            // Arrange
            var expectedExaminationId = "expectedExaminationId";

            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var patientDetailsUpdateService = new Mock<IAsyncQueryHandler<PatientDetailsUpdateQuery, Examination>>();

            Controller = new PatientDetailsController(
                LoggerMock.Object,
                Mapper,
                UsersRetrievalByOktaIdServiceMock.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object,
                examinationRetrievalService.Object,
                patientDetailsUpdateService.Object,
                _locationParentsQueryServiceMock.Object);

            Controller.ModelState.AddModelError("An", nameof(SystemValidationErrors.Duplicate));

            var expectedPutPatientDetailsRequest = new PutPatientDetailsRequest();

            // Act
            var response = Controller.UpdatePatientDetails(expectedExaminationId, expectedPutPatientDetailsRequest).Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutPatientDetailsResponse>>().Subject;
            var badRequestObjectResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestObjectResult.Value.Should().BeAssignableTo<PutPatientDetailsResponse>();
        }

        [Fact]
        public void UpdatePatientDetails_ReturnsNotFound_WhenRetrievalServiceReturnsNull()
        {
            // Arrange
            var expectedExaminationId = "expectedExaminationId";

            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var patientDetailsUpdateService = new Mock<IAsyncQueryHandler<PatientDetailsUpdateQuery, Examination>>();

            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult((Examination)null));

            Controller = new PatientDetailsController(
                LoggerMock.Object,
                Mapper,
                UsersRetrievalByOktaIdServiceMock.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object,
                examinationRetrievalService.Object,
                patientDetailsUpdateService.Object,
                _locationParentsQueryServiceMock.Object);

            var expectedPutPatientDetailsRequest = new PutPatientDetailsRequest();

            // Act
            var response = Controller.UpdatePatientDetails(expectedExaminationId, expectedPutPatientDetailsRequest).Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutPatientDetailsResponse>>().Subject;
            var notFoundObjectResult = taskResult.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject;
            notFoundObjectResult.Value.Should().BeAssignableTo<PutPatientDetailsResponse>();
        }

        [Fact]
        public void UpdatePatientDetails_ReturnsOkay_WhenUpdateSuccessful()
        {
            // Arrange
            SetupAuthorize(AuthorizationResult.Success());
            var expectedExaminationId = "expectedExaminationId";
            var expectedExamination = new Examination()
            {
                ExaminationId = expectedExaminationId,
            };

            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var patientDetailsUpdateService = new Mock<IAsyncQueryHandler<PatientDetailsUpdateQuery, Examination>>();

            UsersRetrievalByOktaIdServiceMock
                .Setup(service => service.Handle(It.IsAny<UserRetrievalByOktaIdQuery>()))
                .Returns(Task.FromResult(AuthorizedUser));

            examinationRetrievalService
                .Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(expectedExamination));

            patientDetailsUpdateService
                .Setup(pdus => pdus.Handle(It.IsAny<PatientDetailsUpdateQuery>()))
                .Returns(Task.FromResult(expectedExamination));

            Controller = new PatientDetailsController(
                LoggerMock.Object,
                Mapper,
                UsersRetrievalByOktaIdServiceMock.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object,
                examinationRetrievalService.Object,
                patientDetailsUpdateService.Object,
                _locationParentsQueryServiceMock.Object);

            Controller.ControllerContext = GetControllerContext();

            var expectedPutPatientDetailsRequest = new PutPatientDetailsRequest();

            // Act
            var response = Controller.UpdatePatientDetails(expectedExaminationId, expectedPutPatientDetailsRequest).Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutPatientDetailsResponse>>().Subject;
            var okObjectResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            var okObjectResultValue = okObjectResult.Value.Should().BeAssignableTo<PutPatientDetailsResponse>().Subject;
            okObjectResultValue.Header.Should().NotBeNull();
        }
    }
}