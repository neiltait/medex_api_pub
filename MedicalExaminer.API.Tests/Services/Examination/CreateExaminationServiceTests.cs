using System;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Services.Examination;
using MedicalExaminer.Common.Services.Location;
using MedicalExaminer.Common.Settings;
using MedicalExaminer.Models;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Examination
{
    public class CreateExaminationServiceTests
    {
        private readonly Mock<IOptions<UrgencySettings>> _urgencySettingsMock;

        public CreateExaminationServiceTests()
        {
            _urgencySettingsMock = new Mock<IOptions<UrgencySettings>>(MockBehavior.Strict);
            _urgencySettingsMock
                .Setup(s => s.Value)
                .Returns(new UrgencySettings
                {
                    DaysToPreCalculateUrgencySort = 1
                });
        }

        [Fact]
        public void CreateExaminationQueryIsNullThrowsException()
        {
            // Arrange
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var locationConnectionSettings = new Mock<ILocationConnectionSettings>();
            const CreateExaminationQuery query = null;
            var dbAccess = new Mock<IDatabaseAccess>();
            var locationService = new Mock<LocationIdService>(dbAccess.Object, locationConnectionSettings.Object);
            var sut = new CreateExaminationService(dbAccess.Object, connectionSettings.Object, locationService.Object, _urgencySettingsMock.Object);

            // Act
            Action act = () => sut.Handle(query).GetAwaiter().GetResult();

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CreateExaminationQuerySuccessReturnsExamination()
        {
            // Arrange
            var examination = new MedicalExaminer.Models.Examination();
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var locationConnectionSettings = new Mock<ILocationConnectionSettings>();
            var myUser = new Mock<MeUser>();
            CreateExaminationQuery query = new CreateExaminationQuery(examination, myUser.Object);
            var dbAccess = new Mock<IDatabaseAccess>();
            var location = new MedicalExaminer.Models.Location();
            var locationService = new Mock<LocationIdService>(dbAccess.Object, locationConnectionSettings.Object);
            locationService.Setup(x => x.Handle(It.IsAny<LocationRetrievalByIdQuery>())).Returns(Task.FromResult(location));
            dbAccess.Setup(db => db.CreateItemAsync(
                connectionSettings.Object,
                examination,
                false)).Returns(Task.FromResult(examination)).Verifiable();
            var sut = new CreateExaminationService(dbAccess.Object, connectionSettings.Object, locationService.Object, _urgencySettingsMock.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            dbAccess.Verify(db => db.CreateItemAsync(connectionSettings.Object, examination, false), Times.Once);
            Assert.NotNull(result.Result);
            Assert.Equal(myUser.Object.UserId, result.Result.LastModifiedBy);
        }

        /// <summary>
        /// Test to make sure UpdateCaseUrgencySort method is called when the Examination is created
        /// </summary>
        [Fact]
        public void CreateExaminationQueryWithNoUrgencyIndicatorsSuccessReturnsExaminationWithIsUrgentFalse()
        {
            // Arrange
            var examination = new MedicalExaminer.Models.Examination()
            {
                ChildPriority = false,
                CoronerPriority = false,
                CulturalPriority = false,
                FaithPriority = false,
                OtherPriority = false,
                CreatedAt = DateTime.Now.AddDays(-3)
            };

            var userId = "userId";
            var user = new MeUser()
            {
                UserId = userId
            };
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            CreateExaminationQuery query = new CreateExaminationQuery(examination, user);
            var dbAccess = new Mock<IDatabaseAccess>();
            var locationConnectionSettings = new Mock<ILocationConnectionSettings>();
            var location = new MedicalExaminer.Models.Location();
            var locationService = new Mock<LocationIdService>(dbAccess.Object, locationConnectionSettings.Object);
            locationService.Setup(x => x.Handle(It.IsAny<LocationRetrievalByIdQuery>())).Returns(Task.FromResult(location));

            dbAccess.Setup(db => db.CreateItemAsync(
                connectionSettings.Object,
                examination,
                false)).Returns(Task.FromResult(examination)).Verifiable();
            var sut = new CreateExaminationService(dbAccess.Object, connectionSettings.Object, locationService.Object, _urgencySettingsMock.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            result.Result.Should().NotBeNull();
            result.Result.IsUrgent().Should().BeFalse();
            result.Result.LastModifiedBy.Should().Be(user.UserId);
        }

        /// <summary>
        /// Test to make sure UpdateCaseUrgencySort method is called when the Examination is created
        /// </summary>
        [Fact]
        public void CreateExaminationQueryWithAllUrgencyIndicatorsSuccessReturnsExaminationWithIsUrgentTrue()
        {
            // Arrange
            var examination = new MedicalExaminer.Models.Examination()
            {
                ChildPriority = true,
                CoronerPriority = true,
                CulturalPriority = true,
                FaithPriority = true,
                OtherPriority = true,
                CreatedAt = DateTime.Now
            };
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var userId = "userId";
            var user = new MeUser()
            {
                UserId = userId
            };
            CreateExaminationQuery query = new CreateExaminationQuery(examination, user);
            var dbAccess = new Mock<IDatabaseAccess>();
            var locationConnectionSettings = new Mock<ILocationConnectionSettings>();
            var location = new MedicalExaminer.Models.Location();
            var locationService = new Mock<LocationIdService>(dbAccess.Object, locationConnectionSettings.Object);
            locationService.Setup(x => x.Handle(It.IsAny<LocationRetrievalByIdQuery>())).Returns(Task.FromResult(location));
            dbAccess.Setup(db => db.CreateItemAsync(
                connectionSettings.Object,
                examination,
                false)).Returns(Task.FromResult(examination)).Verifiable();
            var sut = new CreateExaminationService(dbAccess.Object, connectionSettings.Object, locationService.Object, _urgencySettingsMock.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            result.Result.Should().NotBeNull();
            result.Result.IsUrgent().Should().BeTrue();
            result.Result.LastModifiedBy.Should().Be(user.UserId);
        }
    }
}