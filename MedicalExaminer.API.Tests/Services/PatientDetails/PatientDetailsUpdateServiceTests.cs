using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Queries.PatientDetails;
using MedicalExaminer.Common.Services.Location;
using MedicalExaminer.Common.Services.PatientDetails;
using MedicalExaminer.Common.Settings;
using MedicalExaminer.Models;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.PatientDetails
{
    public class PatientDetailsUpdateServiceTests : BaseServiceTest
    {
        private readonly IEnumerable<MedicalExaminer.Models.Location> _locationPath;
        private readonly Mock<IOptions<UrgencySettings>> _urgencySettingsMock;

        public PatientDetailsUpdateServiceTests()
        {
            _locationPath = new List<MedicalExaminer.Models.Location>()
            {
                new MedicalExaminer.Models.Location()
                {
                    LocationId = "NationalId"
                },
                new MedicalExaminer.Models.Location()
                {
                    LocationId = "RegionId"
                },
                new MedicalExaminer.Models.Location()
                {
                    LocationId = "TrustId"
                },
                new MedicalExaminer.Models.Location()
                {
                    LocationId = "SiteId"
                },
            };

            _urgencySettingsMock = new Mock<IOptions<UrgencySettings>>(MockBehavior.Strict);
            _urgencySettingsMock
                .Setup(s => s.Value)
                .Returns(new UrgencySettings
                {
                    DaysToPreCalculateUrgencySort = 1
                });
        }

        [Fact]
        public void PatientDetailsUpdateQueryIsNullThrowsException()
        {
            // Arrange
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            const PatientDetailsUpdateQuery query = null;

            var mapper = new Mock<IMapper>();
            var dbAccess = new Mock<IDatabaseAccess>();
            var locationConnectionSettings = new Mock<ILocationConnectionSettings>();
            var location = new MedicalExaminer.Models.Location();
            var locationService = new Mock<LocationIdService>(dbAccess.Object, locationConnectionSettings.Object);
            locationService.Setup(x => x.Handle(It.IsAny<LocationRetrievalByIdQuery>())).Returns(Task.FromResult(location));
            var sut = new PatientDetailsUpdateService(dbAccess.Object, connectionSettings.Object, mapper.Object, locationService.Object, _urgencySettingsMock.Object);

            // Act
            Action act = () => sut.Handle(query).GetAwaiter().GetResult();

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void PatientDetailsUpdateQuerySuccessReturnsCorrectPropertyValues()
        {
            // Arrange
            var examination = new MedicalExaminer.Models.Examination();
            var patientDetails = new Mock<MedicalExaminer.Models.PatientDetails>();
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var user = new Mock<MeUser>();
            user.Object.UserId = "a";
            var query = new PatientDetailsUpdateQuery("a", patientDetails.Object, user.Object, _locationPath);
            var dbAccess = new Mock<IDatabaseAccess>();
            var locationConnectionSettings = new Mock<ILocationConnectionSettings>();
            var location = new MedicalExaminer.Models.Location();
            var locationService = new Mock<LocationIdService>(dbAccess.Object, locationConnectionSettings.Object);
            locationService.Setup(x => x.Handle(It.IsAny<LocationRetrievalByIdQuery>())).Returns(Task.FromResult(location));
            dbAccess.Setup(db => db.GetItemAsync(connectionSettings.Object,
                    It.IsAny<Expression<Func<MedicalExaminer.Models.Examination, bool>>>()))
                .Returns(Task.FromResult(examination)).Verifiable();
            dbAccess.Setup(db => db.UpdateItemAsync(connectionSettings.Object,
                It.IsAny<MedicalExaminer.Models.Examination>())).Returns(Task.FromResult(examination)).Verifiable();
            var sut = new PatientDetailsUpdateService(dbAccess.Object, connectionSettings.Object, Mapper, locationService.Object, _urgencySettingsMock.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            dbAccess
                .Verify(
                    db => db.UpdateItemAsync(
                    connectionSettings.Object,
                It.IsAny<MedicalExaminer.Models.Examination>()),
                    Times.Once);
            Assert.NotNull(result.Result);
            Assert.Equal("a", result.Result.LastModifiedBy);
        }

        [Fact]
        public void PatientDetailsUpdateQuerySuccessReturnsExaminationId()
        {
            // Arrange
            var examination = new MedicalExaminer.Models.Examination();
            var patientDetails = new Mock<MedicalExaminer.Models.PatientDetails>();
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var user = new Mock<MeUser>();

            var query = new PatientDetailsUpdateQuery("a", patientDetails.Object, user.Object, _locationPath);
            var dbAccess = new Mock<IDatabaseAccess>();
            var locationConnectionSettings = new Mock<ILocationConnectionSettings>();
            var location = new MedicalExaminer.Models.Location();
            var locationService = new Mock<LocationIdService>(dbAccess.Object, locationConnectionSettings.Object);
            locationService.Setup(x => x.Handle(It.IsAny<LocationRetrievalByIdQuery>())).Returns(Task.FromResult(location));
            dbAccess.Setup(db => db.GetItemAsync(connectionSettings.Object,
                    It.IsAny<Expression<Func<MedicalExaminer.Models.Examination, bool>>>()))
                .Returns(Task.FromResult(examination)).Verifiable();
            dbAccess.Setup(db => db.UpdateItemAsync(connectionSettings.Object,
                It.IsAny<MedicalExaminer.Models.Examination>())).Returns(Task.FromResult(examination)).Verifiable();
            var sut = new PatientDetailsUpdateService(dbAccess.Object, connectionSettings.Object, Mapper, locationService.Object, _urgencySettingsMock.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            dbAccess.Verify(
                db => db.UpdateItemAsync(
                    connectionSettings.Object,
                    It.IsAny<MedicalExaminer.Models.Examination>()),
                Times.Once);
            Assert.NotNull(result.Result);
        }

        /// <summary>
        /// Test to make sure UpdateCaseUrgencySort method is called whenever the Examination is updated
        /// </summary>
        [Fact]
        public void PatientDetailsUpdateOnExaminationWithNoUrgencyIndicatorsSuccessReturnsExaminationWithIsUrgentFalse()
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

            var patientDetails = new MedicalExaminer.Models.PatientDetails()
            {
                ChildPriority = false,
                CoronerPriority = false,
                CulturalPriority = false,
                FaithPriority = false,
                OtherPriority = false,
            };

            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var userId = "userId";
            var user = new MeUser()
            {
                UserId = userId,
            };
            var query = new PatientDetailsUpdateQuery("a", patientDetails, user, _locationPath);
            var dbAccess = new Mock<IDatabaseAccess>();
            var locationConnectionSettings = new Mock<ILocationConnectionSettings>();
            var location = new MedicalExaminer.Models.Location();
            var locationService = new Mock<LocationIdService>(dbAccess.Object, locationConnectionSettings.Object);
            locationService.Setup(x => x.Handle(It.IsAny<LocationRetrievalByIdQuery>())).Returns(Task.FromResult(location));
            dbAccess.Setup(db => db.GetItemAsync(connectionSettings.Object,
                    It.IsAny<Expression<Func<MedicalExaminer.Models.Examination, bool>>>()))
                .Returns(Task.FromResult(examination)).Verifiable();
            dbAccess.Setup(db => db.UpdateItemAsync(connectionSettings.Object,
                It.IsAny<MedicalExaminer.Models.Examination>())).Returns(Task.FromResult(examination)).Verifiable();
            var sut = new PatientDetailsUpdateService(dbAccess.Object, connectionSettings.Object, Mapper, locationService.Object, _urgencySettingsMock.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            result.Result.IsUrgent().Should().BeFalse();
            result.Result.LastModifiedBy.Should().Be(userId);
        }

        /// <summary>
        /// Test to make sure UpdateCaseUrgencySort method is called whenever the Examination is updated
        /// </summary>
        [Fact]
        public void PatientDetailsUpdateOnExaminationWithAllUrgencyIndicatorsSuccessReturnsExaminationWithIsUrgentTrue()
        {
            // Arrange
            var examination = new MedicalExaminer.Models.Examination()
            {
                ChildPriority = true,
                CoronerPriority = true,
                CulturalPriority = true,
                FaithPriority = true,
                OtherPriority = true,
                CreatedAt = DateTime.Now.AddDays(-3)
            };
            var patientDetails = new MedicalExaminer.Models.PatientDetails()
            {
                ChildPriority = true,
                CoronerPriority = true,
                CulturalPriority = true,
                FaithPriority = true,
                OtherPriority = true,
            };

            const string userId = "userId";
            var user = new MeUser()
            {
                UserId = userId,
            };

            var connectionSettings = new Mock<IExaminationConnectionSettings>();

            var query = new PatientDetailsUpdateQuery("a", patientDetails, user, _locationPath);
            var dbAccess = new Mock<IDatabaseAccess>();
            var locationConnectionSettings = new Mock<ILocationConnectionSettings>();
            var location = new MedicalExaminer.Models.Location();
            var locationService = new Mock<LocationIdService>(dbAccess.Object, locationConnectionSettings.Object);
            locationService.Setup(x => x.Handle(It.IsAny<LocationRetrievalByIdQuery>())).Returns(Task.FromResult(location));
            dbAccess.Setup(db => db.GetItemAsync(connectionSettings.Object,
                    It.IsAny<Expression<Func<MedicalExaminer.Models.Examination, bool>>>()))
                .Returns(Task.FromResult(examination)).Verifiable();
            dbAccess.Setup(db => db.UpdateItemAsync(connectionSettings.Object,
                It.IsAny<MedicalExaminer.Models.Examination>())).Returns(Task.FromResult(examination)).Verifiable();
            var sut = new PatientDetailsUpdateService(dbAccess.Object, connectionSettings.Object, Mapper, locationService.Object, _urgencySettingsMock.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            result.Result.IsUrgent().Should().BeTrue();
            result.Result.LastModifiedBy.Should().Be(userId);
        }
    }
}