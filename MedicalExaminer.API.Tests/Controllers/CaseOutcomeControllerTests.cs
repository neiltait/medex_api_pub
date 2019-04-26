using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.CaseOutcome;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Controllers
{
    public class CaseOutcomeControllerTests
    {
        [Fact]
        public async void PutCloseCase_When_Called_With_No_Case_Id_Returns_Bad_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var closeCaseService = new Mock<IAsyncQueryHandler<CloseCaseQuery, string>>();
            var coronerReferralService = new Mock<IAsyncQueryHandler<CoronerReferralQuery, string>>();
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var saveOutstandingCaseItems = new Mock<IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string>>();

        var sut = new CaseOutcomeController(
                logger.Object,
                mapper.Object,
                coronerReferralService.Object,
                closeCaseService.Object,
                examinationRetrievalService.Object,
                saveOutstandingCaseItems.Object,
                usersRetrievalByEmailService.Object);

            sut.ControllerContext = GetControllerContext();

            // Act
            var response = await sut.PutCloseCase(string.Empty);

            // Assert
            response.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async void PutCloseCase_When_Called_With_Invalid_Case_Id_Returns_Bad_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var closeCaseService = new Mock<IAsyncQueryHandler<CloseCaseQuery, string>>();
            var coronerReferralService = new Mock<IAsyncQueryHandler<CoronerReferralQuery, string>>();
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var saveOutstandingCaseItems = new Mock<IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string>>();

            var sut = new CaseOutcomeController(
                logger.Object,
                mapper.Object,
                coronerReferralService.Object,
                closeCaseService.Object,
                examinationRetrievalService.Object,
                saveOutstandingCaseItems.Object,
                usersRetrievalByEmailService.Object);

            sut.ControllerContext = GetControllerContext();

            // Act
            var response = await sut.PutCloseCase("invalidCaseId");

            // Assert
            response.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async void PutCloseCase_When_Called_With_Valid_Examination_Id_Returns_Ok()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var examination = new Examination
            {
                ExaminationId = Guid.NewGuid().ToString()
            };

            var mockMeUser = new Mock<MeUser>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            usersRetrievalByEmailService.Setup(service => service.Handle(It.IsAny<UserRetrievalByEmailQuery>()))
                .Returns(Task.FromResult(mockMeUser.Object));

            var closeCaseService = new Mock<IAsyncQueryHandler<CloseCaseQuery, string>>();
            closeCaseService.Setup(service => service.Handle(It.IsAny<CloseCaseQuery>()))
                .Returns(Task.FromResult("test")).Verifiable();

            var coronerReferralService = new Mock<IAsyncQueryHandler<CoronerReferralQuery, string>>();
            var saveOutstandingCaseItems = new Mock<IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string>>();

            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examination)).Verifiable();

            var sut = new CaseOutcomeController(
                logger.Object,
                mapper.Object,
                coronerReferralService.Object,
                closeCaseService.Object,
                examinationRetrievalService.Object,
                saveOutstandingCaseItems.Object,
                usersRetrievalByEmailService.Object);

            sut.ControllerContext = GetControllerContext();

            // Act
            var response = await sut.PutCloseCase(examination.ExaminationId);

            // Assert
            var okResult = response.Should().BeAssignableTo<OkResult>().Subject;
        }

        private ControllerContext GetControllerContext()
        {
            return new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Email, "username")
                    }, "someAuthTypeName"))
                }
            };
        }

    }
}