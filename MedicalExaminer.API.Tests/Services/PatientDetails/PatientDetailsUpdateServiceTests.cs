using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Queries.PatientDetails;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Common.Services.Location;
using MedicalExaminer.Common.Services.PatientDetails;
using MedicalExaminer.Models;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.PatientDetails
{
    public class PatientDetailsUpdateServiceTests : BaseServiceTest
    {
        private IEnumerable<MedicalExaminer.Models.Location> _locationPath;

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
        }

        [Fact]
        public void PatientDetailsUpdateQueryIsNullThrowsException()
        {
            // Arrange
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            PatientDetailsUpdateQuery query = null;

            var mapper = new Mock<IMapper>();
            var dbAccess = new Mock<IDatabaseAccess>();
            var locationConnectionSettings = new Mock<ILocationConnectionSettings>();
            var location = new MedicalExaminer.Models.Location();
            var locationService = new Mock<LocationIdService>(dbAccess.Object, locationConnectionSettings.Object);
            locationService.Setup(x => x.Handle(It.IsAny<LocationRetrievalByIdQuery>())).Returns(Task.FromResult(location));
            var sut = new PatientDetailsUpdateService(dbAccess.Object, connectionSettings.Object, mapper.Object, locationService.Object);

            Action act = () => sut.Handle(query).GetAwaiter().GetResult();
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
            var sut = new PatientDetailsUpdateService(dbAccess.Object, connectionSettings.Object, Mapper, locationService.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            dbAccess.Verify(db => db.UpdateItemAsync(connectionSettings.Object,
                It.IsAny<MedicalExaminer.Models.Examination>()), Times.Once);
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
            var sut = new PatientDetailsUpdateService(dbAccess.Object, connectionSettings.Object, Mapper, locationService.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            dbAccess.Verify(db => db.UpdateItemAsync(connectionSettings.Object,
                It.IsAny<MedicalExaminer.Models.Examination>()), Times.Once);
            Assert.NotNull(result.Result);
        }

        /// <summary>
        /// Test to make sure UpdateCaseUrgencyScore method is called whenever the Examination is updated
        /// </summary>
        [Fact]
        public void PatientDetailsUpdateOnExaminationWithNoUrgencyIndicatorsSuccessReturnsExaminationWithUrgencyScoreZero()
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
            var user = new Mock<MeUser>();
            user.Object.UserId = "a";
            var query = new PatientDetailsUpdateQuery("a", patientDetails, user.Object, _locationPath);
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
            var sut = new PatientDetailsUpdateService(dbAccess.Object, connectionSettings.Object, Mapper, locationService.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            Assert.Equal(0, result.Result.UrgencyScore);
            Assert.Equal("a", result.Result.LastModifiedBy);
        }

        /// <summary>
        /// Test to make sure UpdateCaseUrgencyScore method is called whenever the Examination is updated
        /// </summary>
        [Fact]
        public void PatientDetailsUpdateOnExaminationWithAllUrgencyIndicatorsSuccessReturnsExaminationWithUrgencyScore500()
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

            var user = new Mock<MeUser>();
            user.Object.UserId = "a";
            var connectionSettings = new Mock<IExaminationConnectionSettings>();

            var query = new PatientDetailsUpdateQuery("a", patientDetails, user.Object, _locationPath);
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
            var sut = new PatientDetailsUpdateService(dbAccess.Object, connectionSettings.Object, Mapper, locationService.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            Assert.Equal(500, result.Result.UrgencyScore);
            Assert.Equal("a", result.Result.LastModifiedBy);
        }
    }
}