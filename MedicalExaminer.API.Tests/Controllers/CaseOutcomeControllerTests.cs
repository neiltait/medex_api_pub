using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Models.v1.CaseOutcome;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.CaseOutcome;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MedicalExaminer.API.Tests.Controllers
{
    public class CaseOutcomeControllerTests
    {
        [Fact]
        public async void PutConfirmationOfScrutiny_When_Called_With_No_Case_Id_Returns_Bad_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var confirmationOfScrutinyService = new Mock<IAsyncQueryHandler<ConfirmationOfScrutinyQuery, Examination>>();
            
            var sut = new CaseOutcomeController(
                logger.Object,
                mapper.Object,
                confirmationOfScrutinyService.Object,
                usersRetrievalByEmailService.Object);

            sut.ControllerContext = GetContollerContext();

            // Act
            var response = await sut.PutConfirmationOfScrutiny(string.Empty);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutConfirmationOfScrutinyResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutConfirmationOfScrutinyResponse>();
        }

        [Fact]
        public async void PutConfirmationOfScrutiny_When_Called_With_Invalid_Case_Id_Returns_Not_Found()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var confirmationOfScrutinyService = new Mock<IAsyncQueryHandler<ConfirmationOfScrutinyQuery, Examination>>();

            var sut = new CaseOutcomeController(
                logger.Object,
                mapper.Object,
                confirmationOfScrutinyService.Object,
                usersRetrievalByEmailService.Object);

            sut.ControllerContext = GetContollerContext();

            // Act
            var response = await sut.PutConfirmationOfScrutiny("invalidCaseId");

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutConfirmationOfScrutinyResponse>>().Subject;
            var notFoundResult = taskResult.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject;
            notFoundResult.Value.Should().BeAssignableTo<PutConfirmationOfScrutinyResponse>();
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
