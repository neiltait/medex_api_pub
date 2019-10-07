using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.API.Models.v1.Locations;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.Location;
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
    public class ExaminationControllerTests : AuthorizedControllerTestsBase<ExaminationsController>
    {
        private readonly Mock<IAsyncQueryHandler<CreateExaminationQuery, Examination>> _createExaminationServiceMock;

        private readonly Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>> _examinationsRetrievalQueryServiceMock;

        private readonly Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview>> _examinationsDashboardServiceMock;

        private readonly Mock<IAsyncQueryHandler<LocationParentsQuery, IEnumerable<Location>>> _locationParentsServiceMock;

        private readonly Mock<IAsyncQueryHandler<LocationsRetrievalByQuery, IEnumerable<Location>>> _locationRetrievalByQueryHandlerMock;

        private readonly Mock<IAsyncQueryHandler<UsersRetrievalQuery, IEnumerable<MeUser>>> _usersRetrievalServiceMock;
        private readonly Mock<IAsyncQueryHandler<LocationsParentsQuery, IDictionary<string, IEnumerable<Location>>>> _locationsRetrievalByQueryMock;

        public ExaminationControllerTests()
            : base(setupAuthorize: false)
        {
            _createExaminationServiceMock = new Mock<IAsyncQueryHandler<CreateExaminationQuery, Examination>>(MockBehavior.Strict);
            _locationsRetrievalByQueryMock = new Mock<IAsyncQueryHandler<LocationsParentsQuery, IDictionary<string, IEnumerable<Location>>>>(MockBehavior.Strict);
            _examinationsRetrievalQueryServiceMock = new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>>(MockBehavior.Strict);

            _examinationsDashboardServiceMock = new Mock<IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview>>(MockBehavior.Strict);

            _locationParentsServiceMock = new Mock<IAsyncQueryHandler<LocationParentsQuery, IEnumerable<Location>>>(MockBehavior.Strict);

            _locationRetrievalByQueryHandlerMock = new Mock<IAsyncQueryHandler<LocationsRetrievalByQuery, IEnumerable<Location>>>(MockBehavior.Strict);

            _usersRetrievalServiceMock = new Mock<IAsyncQueryHandler<UsersRetrievalQuery, IEnumerable<MeUser>>>(MockBehavior.Strict);

            Controller = new ExaminationsController(
                LoggerMock.Object,
                Mapper,
                UsersRetrievalByOktaIdServiceMock.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object,
                _createExaminationServiceMock.Object,
                _examinationsRetrievalQueryServiceMock.Object,
                _examinationsDashboardServiceMock.Object,
                _locationParentsServiceMock.Object,
                _locationRetrievalByQueryHandlerMock.Object,
                _usersRetrievalServiceMock.Object,
                _locationsRetrievalByQueryMock.Object);
            Controller.ControllerContext = GetControllerContext();
        }

        [Fact]
        public void GetExaminations_ReturnsBadRequest_WhenNullFilter()
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
            var response = Controller.GetExaminations(null).Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetExaminationsResponse>>().Subject;
            taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public void GetExaminations_ReturnsOk()
        {
            // Arrange
            SetupAuthorize(AuthorizationResult.Success());
            var examination1 = new Examination();
            var examination2 = new Examination();
            IEnumerable<Examination> examinationsResult = new List<Examination> { examination1, examination2 };
            var locations = new List<Location> { };
            var users = new List<MeUser> { };

            var examinationOverview = new ExaminationsOverview();

            _examinationsRetrievalQueryServiceMock
                .Setup(service => service.Handle(It.IsAny<ExaminationsRetrievalQuery>()))
                .Returns(Task.FromResult(examinationsResult));

            _examinationsDashboardServiceMock
                .Setup(service => service.Handle(It.IsAny<ExaminationsRetrievalQuery>()))
                .Returns(Task.FromResult(examinationOverview));

            _locationRetrievalByQueryHandlerMock
                .Setup(service => service.Handle(It.IsAny<LocationsRetrievalByQuery>()))
                .Returns(Task.FromResult(locations.AsEnumerable()));

            _usersRetrievalServiceMock
                .Setup(service => service.Handle(It.IsAny<UsersRetrievalQuery>()))
                .Returns(Task.FromResult(users.AsEnumerable()));

            _locationsRetrievalByQueryMock.Setup(service => service.Handle(It.IsAny<LocationsParentsQuery>()))
                .Returns(Task.FromResult((IDictionary<string, IEnumerable<Location>>)(new Dictionary<string, IEnumerable<Location>>())));

            var request = new GetExaminationsRequest()
            {
            };

            // Act
            var response = Controller.GetExaminations(request).Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetExaminationsResponse>>().Subject;
            var okResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
        }

        [Fact]
        public void GetExaminations_PopulatesLookups()
        {
            // Arrange
            SetupAuthorize(AuthorizationResult.Success());
            var expectedLocation = new LocationLookup { LocationId = "location1", Name = "Name1" };
            var expectedUser = new UserLookup { UserId = "user1", FullName = "User 1" };

            var examination1 = new Examination();
            var examination2 = new Examination();
            IEnumerable<Examination> examinationsResult = new List<Examination> { examination1, examination2 };
            var meOfficeLocation = new Location()
            {
                LocationId = "location1",
                Name = "Name1",
                IsMeOffice = true,
            };

            var locations = new List<Location>
            {
                meOfficeLocation,
                new Location()
                {
                    LocationId = "location2",
                    Name = "Name2"
                }
            };
            var users = new List<MeUser>
            {
                new MeUser()
                {
                    UserId = "user1",
                    FirstName = "User",
                    LastName = "1",
                    Permissions = new List<MEUserPermission>()
                    {
                        new MEUserPermission()
                        {
                            LocationId = "location1",
                            UserRole = UserRoles.MedicalExaminer,
                        }
                    }
                }
            };

            var examinationOverview = new ExaminationsOverview();

            _examinationsRetrievalQueryServiceMock
                .Setup(service => service.Handle(It.IsAny<ExaminationsRetrievalQuery>()))
                .Returns(Task.FromResult(examinationsResult));

            _examinationsDashboardServiceMock
                .Setup(service => service.Handle(It.IsAny<ExaminationsRetrievalQuery>()))
                .Returns(Task.FromResult(examinationOverview));

            _locationRetrievalByQueryHandlerMock
                .Setup(service => service.Handle(It.IsAny<LocationsRetrievalByQuery>()))
                .Returns(Task.FromResult(locations.AsEnumerable()));

            _usersRetrievalServiceMock
                .Setup(service => service.Handle(It.IsAny<UsersRetrievalQuery>()))
                .Returns(Task.FromResult(users.AsEnumerable()));

            _locationsRetrievalByQueryMock.Setup(service => service.Handle(It.IsAny<LocationsParentsQuery>()))
                .Returns(Task.FromResult((IDictionary<string, IEnumerable<Location>>)(new Dictionary<string, IEnumerable<Location>>()
                {
                    {meOfficeLocation.LocationId, new [] {meOfficeLocation } }
                })));

            var request = new GetExaminationsRequest()
            {
            };

            // Act
            var response = Controller.GetExaminations(request).Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetExaminationsResponse>>().Subject;
            var okResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            var typedResponse = (GetExaminationsResponse) okResult.Value;

            // Assert
            typedResponse.Lookups.ContainsKey(ExaminationsController.LocationFilterLookupKey).Should().BeTrue();
            typedResponse.Lookups[ExaminationsController.LocationFilterLookupKey].Should()
                .ContainEquivalentOf(expectedLocation);

            typedResponse.Lookups.ContainsKey(ExaminationsController.UserFilterLookupKey).Should().BeTrue();
            typedResponse.Lookups[ExaminationsController.UserFilterLookupKey].Should()
                .ContainEquivalentOf(expectedUser);
        }

        [Fact]
        public void TestCreateCaseValidationFailure()
        {
            // Arrange
            SetupAuthorize(AuthorizationResult.Success());
            Controller.ModelState.AddModelError("test", nameof(SystemValidationErrors.Required));

            // Act
            var response = Controller.CreateExamination(CreateValidNewCaseRequest()).Result;
            
            // Assert
            response.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            var result = (BadRequestObjectResult) response.Result;
            result.Value.Should().BeAssignableTo<PutExaminationResponse>();
            var model = (PutExaminationResponse) result.Value;
            model.Errors.Count.Should().Be(1);
            model.Success.Should().BeFalse();
        }

        [Fact]
        public async void CreateExamination_ValidModelStateReturnsOkResponse()
        {
            // Arrange
            SetupAuthorize(AuthorizationResult.Success());
            var examination = CreateValidExamination();

            _createExaminationServiceMock
                .Setup(ecs => ecs.Handle(It.IsAny<CreateExaminationQuery>()))
                .Returns(Task.FromResult(examination));

            var mockMeUser = new MeUser()
            {
                UserId = "abcd"
            };

            var parentLocations = new List<Location>();

            UsersRetrievalByOktaIdServiceMock.Setup(service => service.Handle(It.IsAny<UserRetrievalByOktaIdQuery>()))
                .Returns(Task.FromResult(mockMeUser));

            _locationParentsServiceMock.Setup(lps => lps.Handle(It.IsAny<LocationParentsQuery>()))
                .Returns(Task.FromResult(parentLocations.AsEnumerable()));

            // Act
            var response = await Controller.CreateExamination(CreateValidNewCaseRequest());

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult) response.Result;
            result.Value.Should().BeAssignableTo<PutExaminationResponse>();
            var model = (PutExaminationResponse) result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();
        }

        [Fact]
        public async void CreateExamination_ReturnsForbid_WhenNotHavePermission()
        {
            // Arrange
            SetupAuthorize(AuthorizationResult.Failed());
            var examination = CreateValidExamination();

            _createExaminationServiceMock
                .Setup(ecs => ecs.Handle(It.IsAny<CreateExaminationQuery>()))
                .Returns(Task.FromResult(examination));

            var mockMeUser = new MeUser()
            {
                UserId = "abcd"
            };

            var parentLocations = new List<Location>();

            UsersRetrievalByOktaIdServiceMock.Setup(service => service.Handle(It.IsAny<UserRetrievalByOktaIdQuery>()))
                .Returns(Task.FromResult(mockMeUser));

            _locationParentsServiceMock.Setup(lps => lps.Handle(It.IsAny<LocationParentsQuery>()))
                .Returns(Task.FromResult(parentLocations.AsEnumerable()));

            // Act
            var response = await Controller.CreateExamination(CreateValidNewCaseRequest());

            // Assert
            response.Result.Should().BeAssignableTo<ForbidResult>();
        }

        private PostExaminationRequest CreateValidNewCaseRequest()
        {
            return new PostExaminationRequest
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
    }
}
