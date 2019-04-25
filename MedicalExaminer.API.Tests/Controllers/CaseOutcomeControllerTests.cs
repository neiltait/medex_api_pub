using System;
using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.CaseOutcome;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
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

            var sut = new CaseOutcomeController(
                logger.Object,
                mapper.Object,
                coronerReferralService.Object,
                closeCaseService.Object,
                usersRetrievalByEmailService.Object);

            sut.ControllerContext = GetContollerContext();

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

            var sut = new CaseOutcomeController(
                logger.Object,
                mapper.Object,
                coronerReferralService.Object,
                closeCaseService.Object,
                usersRetrievalByEmailService.Object);

            sut.ControllerContext = GetContollerContext();

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
            var examinationId = Guid.NewGuid().ToString();
            var examination = new Examination
            {
                ExaminationId = examinationId
            };
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var closeCaseService = new Mock<IAsyncQueryHandler<CloseCaseQuery, string>>();
            var coronerReferralService = new Mock<IAsyncQueryHandler<CoronerReferralQuery, string>>();

            var sut = new CaseOutcomeController(
                logger.Object,
                mapper.Object,
                coronerReferralService.Object,
                closeCaseService.Object,
                usersRetrievalByEmailService.Object);

            sut.ControllerContext = GetContollerContext();

            // Act
            var response = await sut.PutCloseCase(examination.ExaminationId);

            // Assert
            examination.Completed.Equals(true);
        }

        private ControllerContext GetContollerContext()
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