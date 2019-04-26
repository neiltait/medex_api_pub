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

        var sut = new CaseOutcomeController(
                logger.Object,
                mapper.Object,
                coronerReferralService.Object,
                closeCaseService.Object,
                examinationRetrievalService.Object,
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

            var sut = new CaseOutcomeController(
                logger.Object,
                mapper.Object,
                coronerReferralService.Object,
                closeCaseService.Object,
                examinationRetrievalService.Object,
                usersRetrievalByEmailService.Object);

            sut.ControllerContext = GetControllerContext();

            // Act
            var response = await sut.PutCloseCase("invalidCaseId");

            // Assert
            response.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async void PutCloseCase_When_Called_With_Valid_Case_Id_Mark_Case_As_Completed()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var examination = new Mock<Examination>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var closeCaseService = new Mock<IAsyncQueryHandler<CloseCaseQuery, string>>();
            var coronerReferralService = new Mock<IAsyncQueryHandler<CoronerReferralQuery, string>>();
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examination.Object)).Verifiable();
            var mockMeUser = new Mock<MeUser>();
            usersRetrievalByEmailService.Setup(service => service.Handle(It.IsAny<UserRetrievalByEmailQuery>()))
                .Returns(Task.FromResult(mockMeUser.Object));

            var sut = new CaseOutcomeController(
                logger.Object,
                mapper.Object,
                coronerReferralService.Object,
                closeCaseService.Object,
                examinationRetrievalService.Object,
                usersRetrievalByEmailService.Object);

            sut.ControllerContext = GetControllerContext();

            // Act
            var response = await sut.PutCloseCase(examination.Object.ExaminationId);

            // Assert
            Assert.Equal(expected: true, actual: examination.Object.Completed);
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