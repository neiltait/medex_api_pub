using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.API.Models.v1.PatientDetails;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.PatientDetails;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Common.Services.PatientDetails;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Controllers
{
    public class PatientDetailsControllerTest : ControllerTestsBase<PatientDetailsController>
    {
        [Fact]
        public void GetPatientDetails_When_Called_With_Invalid_Id_Returns_Expected_Type()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var examinationRetrievalQuery = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var patientDetailsUpdateService = new Mock<IAsyncQueryHandler<PatientDetailsUpdateQuery, Examination>>();
            var patientDetailsByCaseIdService = new Mock<IAsyncQueryHandler<PatientDetailsByCaseIdQuery, Examination>>();

            patientDetailsByCaseIdService.Setup(service => service.Handle(It.IsAny<PatientDetailsByCaseIdQuery>()))
                .Returns(Task.FromResult<Examination>(null));

            var sut = new PatientDetailsController(logger.Object, mapper.Object, examinationRetrievalQuery.Object,
                patientDetailsUpdateService.Object, patientDetailsByCaseIdService.Object);
            // Act
            var response = sut.GetPatientDetails("dfgdfgdfg");

            // Assert
            var taskResult = response.Should().BeOfType<Task<ActionResult<GetPatientDetailsResponse>>>().Subject;
            Assert.Equal(TaskStatus.RanToCompletion, taskResult.Status);
        }

        [Fact]
        public async void GetPatientDetails_When_Called_With_Id_Not_Found_Returns_NotFound()
        {
            // Arrange
            
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var examination = new Examination()
            {
                id = "a"
            };
            var getPatientDetailsResponse = new Mock<GetPatientDetailsResponse>();
            var patientDetails = new Mock<PatientDetails>();
            mapper.Setup(m => m.Map<GetPatientDetailsResponse>(patientDetails.Object)).Returns(getPatientDetailsResponse.Object);
            var examinationRetrievalQuery = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var patientDetailsUpdateService = new Mock<IAsyncQueryHandler<PatientDetailsUpdateQuery, Examination>>();
            var patientDetailsByCaseIdService = new Mock<IAsyncQueryHandler<PatientDetailsByCaseIdQuery, Examination>>();
            patientDetailsByCaseIdService.Setup(service => service.Handle(It.IsAny<PatientDetailsByCaseIdQuery>()))
                .Returns(Task.FromResult(examination));
            var sut = new PatientDetailsController(logger.Object, mapper.Object, examinationRetrievalQuery.Object, 
                patientDetailsUpdateService.Object, patientDetailsByCaseIdService.Object);
            // Act
            var response = sut.GetPatientDetails("a").Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetPatientDetailsResponse>>().Subject;
            var notFoundResult = taskResult.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject;
            notFoundResult.Value.Should().BeAssignableTo<GetPatientDetailsResponse>();
        }

        [Fact]
        public async void GetPatientDetails_When_Called_With_Valid_Id_Returns_Expected_Type()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var examination = new Examination()
            {
                id = "a"
            };

            var patientDetailsByCaseIdQuery = new PatientDetailsByCaseIdQuery("a");

            var getPatientDetailsResponse = new Mock<GetPatientDetailsResponse>();
            var patientDetails = new Mock<PatientDetails>();
            mapper.Setup(m => m.Map<GetPatientDetailsResponse>(patientDetails.Object)).Returns(getPatientDetailsResponse.Object);
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>())).Returns(Task.FromResult(examination));
            var patientDetailsUpdateService = new Mock<IAsyncQueryHandler<PatientDetailsUpdateQuery, Examination>>();
            var patientDetailsByCaseIdService = new Mock<IAsyncQueryHandler<PatientDetailsByCaseIdQuery, Examination>>();
            patientDetailsByCaseIdService.Setup(service => service.Handle(It.IsAny<PatientDetailsByCaseIdQuery>()))
                .Returns(Task.FromResult(examination));
            var sut = new PatientDetailsController(logger.Object, mapper.Object, examinationRetrievalService.Object,
                patientDetailsUpdateService.Object, patientDetailsByCaseIdService.Object);
            // Act
            var response = sut.GetPatientDetails("a").Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetPatientDetailsResponse>>().Subject;
            var okResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            
        }


       
    }
}
