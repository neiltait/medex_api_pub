using System.Collections.Generic;
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
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Controllers
{
    public class PatientDetailsControllerTest : ControllerTestsBase<PatientDetailsController>
    {
        [Fact]
        public void GetExamination_When_Called_With_Invalid_Id_Returns_Expected_Type()
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
        public async void GetExamination_When_Called_With_Valid_Id_Returns_Expected_Type()
        {
            // Arrange
            Examination examinationObj = new Examination()
            {
                Id = "a"
            };
            var getResponse = new GetExaminationResponse()
            {
                id = "a"
            };
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<GetExaminationResponse>(examinationObj)).Returns(getResponse);
            var createExaminationService = new Mock<IAsyncQueryHandler<CreateExaminationQuery, string>>();
            var examinationRetrievalQueryService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var examinationsRetrievalQueryService = new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>>();
            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examinationObj));
            var sut = new ExaminationsController(logger.Object, mapper.Object, createExaminationService.Object,
                examinationRetrievalQueryService.Object, examinationsRetrievalQueryService.Object);
            // Act
            var response = sut.GetExamination("a").Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetExaminationResponse>>().Subject;
            var okResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            okResult.Value.Should().BeAssignableTo<GetExaminationResponse>();
            Assert.Equal("a", ((GetExaminationResponse)okResult.Value).id);
        }

    }
}
