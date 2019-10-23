using FluentAssertions;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Models.v1.Report;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MedicalExaminer.API.Tests.Controllers
{
    public class ReportControllerTests : AuthorizedControllerTestsBase<ReportController>
    {
        private readonly Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>> _examinationsRetrievalQueryServiceMock;
        private readonly Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>> _examinationRetrievalQueryServiceMock;
        private readonly Mock<IAsyncQueryHandler<FinanceQuery, IEnumerable<Examination>>> _financeQuery;
        private readonly Mock<IAsyncQueryHandler<LocationsRetrievalByIdQuery, IEnumerable<Location>>> _locationsRetrievalService;


        public ReportControllerTests()
            : base(setupAuthorize: false)
        {
            _examinationsRetrievalQueryServiceMock = new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>>(MockBehavior.Strict);
            _examinationRetrievalQueryServiceMock = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>(MockBehavior.Strict);
            _financeQuery = new Mock<IAsyncQueryHandler<FinanceQuery, IEnumerable<Examination>>>(MockBehavior.Strict);
            _locationsRetrievalService = new Mock<IAsyncQueryHandler<LocationsRetrievalByIdQuery, IEnumerable<Location>>>(MockBehavior.Strict);

            Controller = new ReportController(
                LoggerMock.Object,
                Mapper,
                UsersRetrievalByOktaIdServiceMock.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object,
                _examinationRetrievalQueryServiceMock.Object,
                _financeQuery.Object,
                _locationsRetrievalService.Object);
            Controller.ControllerContext = GetControllerContext();
        }

        [Fact]
        public void GetCoronerReferralDownload_ReturnsBadRequest_WhenNullFilter()
        {
            // Arrange
            SetupAuthorize(AuthorizationResult.Success());
            var examination1 = new Examination();
            var examination2 = new Examination();
            IEnumerable<Examination> examinationsResult = new List<Examination> { examination1, examination2 };

            _examinationsRetrievalQueryServiceMock
                .Setup(service => service.Handle(It.IsAny<ExaminationsRetrievalQuery>()))
                .Returns(Task.FromResult(examinationsResult));

            // Act
            var response = Controller.GetCoronerReferralDownload(null).Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetCoronerReferralDownloadResponse>>().Subject;
            taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async void GetCoronerReferralDownload_ReturnsResultNotFound_WhenNoExaminationFound()
        {
            // Arrange
            SetupAuthorize(AuthorizationResult.Failed());
            
            var mockMeUser = new MeUser()
            {
                UserId = "abcd"
            };

            var parentLocations = new List<Location>();

            UsersRetrievalByOktaIdServiceMock.Setup(service => service.Handle(It.IsAny<UserRetrievalByOktaIdQuery>()))
                .Returns(Task.FromResult(mockMeUser));

            _examinationRetrievalQueryServiceMock.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>())).Returns(Task.FromResult(default(Examination)));

            // Act
            var response = await Controller.GetCoronerReferralDownload("examinationId");

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundResult>();
        }

        [Fact]
        public async void GetCoronerReferralDownload_ReturnsForbid_WhenNotHavePermission()
        {
            // Arrange
            SetupAuthorize(AuthorizationResult.Failed());

            var mockMeUser = new MeUser()
            {
                UserId = "abcd"
            };

            var parentLocations = new List<Location>();

            UsersRetrievalByOktaIdServiceMock.Setup(service => service.Handle(It.IsAny<UserRetrievalByOktaIdQuery>()))
                .Returns(Task.FromResult(mockMeUser));

            _examinationRetrievalQueryServiceMock.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>())).Returns(Task.FromResult(CreateExamination()));

            // Act
            var response = await Controller.GetCoronerReferralDownload("examinationId");

            // Assert
            response.Result.Should().BeAssignableTo<ForbidResult>();
        }

        [Fact]
        public void GetFinanceDownload_ReturnsResults_WhenExaminationsArePresent()
        {
            // Arrange
            SetupAuthorize(AuthorizationResult.Success());
            var examination1 = new Examination()
            {
                SiteLocationId = "location1"
            };
            var examination2 = new Examination()
            {
                SiteLocationId = "location1"
            };

            var location = new Location()
            {
                LocationId = "location1",
                Name = "location name"
            };

            IEnumerable<Location> locations = new List<Location>() { location };
            IEnumerable<Examination> examinationsResult = new List<Examination> { examination1, examination2 };

            _financeQuery
                .Setup(service => service.Handle(It.IsAny<FinanceQuery>()))
                .Returns(Task.FromResult(examinationsResult));

            _locationsRetrievalService.Setup(service => service.Handle(It.IsAny<LocationsRetrievalByIdQuery>())).
                Returns(Task.FromResult(locations));

            var financeRequest = CreateGetFinanceDownloadRequest();

            // Act
            var response = Controller.GetFinanceDownload(financeRequest).Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetFinanceDownloadResponse>>().Subject;
            taskResult.Result.Should().BeAssignableTo<OkObjectResult>();
        }

        [Fact]
        public async void GetFinanceDownload_ReturnsForbid_WhenNotHavePermission()
        {
            // Arrange
            SetupAuthorize(AuthorizationResult.Failed());

            var mockMeUser = new MeUser()
            {
                UserId = "abcd"
            };

            var parentLocations = new List<Location>();

            UsersRetrievalByOktaIdServiceMock.Setup(service => service.Handle(It.IsAny<UserRetrievalByOktaIdQuery>()))
                .Returns(Task.FromResult(mockMeUser));

            _examinationRetrievalQueryServiceMock.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>())).Returns(Task.FromResult(default(Examination)));

            var financeDownloadRequest = CreateGetFinanceDownloadRequest();

            // Act
            var response = await Controller.GetFinanceDownload(financeDownloadRequest);

            // Assert
            response.Result.Should().BeAssignableTo<ForbidResult>();
        }

        private GetFinanceDownloadRequest CreateGetFinanceDownloadRequest()
        {
            return new GetFinanceDownloadRequest()
            {
                ExaminationsCreatedFrom = new System.DateTime(2019, 9, 1),
                ExaminationsCreatedTo = new System.DateTime(2019, 9, 1),
                LocationId = "locationId",
            };
        }

        private Examination CreateExamination()
        {
            return new Examination()
            {
                ExaminationId = "examinationId",
                SiteLocationId = "locationId",
            };
        }
    }
}
