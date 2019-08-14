using System;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Services.Examination;
using MedicalExaminer.Common.Services.Location;
using MedicalExaminer.Models;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Examination
{
    public class CreateExaminationServiceTests
    {
        [Fact]
        public void CreateExaminationQueryIsNullThrowsException()
        {
            // Arrange
            MedicalExaminer.Models.Examination examination = new MedicalExaminer.Models.Examination();
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var locationConnectionSettings = new Mock<ILocationConnectionSettings>();
            CreateExaminationQuery query = null;
            var dbAccess = new Mock<IDatabaseAccess>();
            var locationService = new Mock<LocationIdService>(dbAccess.Object, locationConnectionSettings.Object);
            var sut = new CreateExaminationService(dbAccess.Object, connectionSettings.Object, locationService.Object);

            Action act = () => sut.Handle(query).GetAwaiter().GetResult();
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
            var sut = new CreateExaminationService(dbAccess.Object, connectionSettings.Object, locationService.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            dbAccess.Verify(db => db.CreateItemAsync(connectionSettings.Object, examination, false), Times.Once);
            Assert.NotNull(result.Result);
            Assert.Equal(myUser.Object.UserId, result.Result.LastModifiedBy);
        }

        /// <summary>
        /// Test to make sure UpdateCaseUrgencyScoreAndSort method is called when the Examination is created
        /// </summary>
        [Fact]
        public void CreateExaminationQueryWithNoUrgencyIndicatorsSuccessReturnsExaminationWithUrgencyScoreZero()
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

            var myUser = new Mock<MeUser>();
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            CreateExaminationQuery query = new CreateExaminationQuery(examination, myUser.Object);
            var dbAccess = new Mock<IDatabaseAccess>();
            var locationConnectionSettings = new Mock<ILocationConnectionSettings>();
            var location = new MedicalExaminer.Models.Location();
            var locationService = new Mock<LocationIdService>(dbAccess.Object, locationConnectionSettings.Object);
            locationService.Setup(x => x.Handle(It.IsAny<LocationRetrievalByIdQuery>())).Returns(Task.FromResult(location));

            dbAccess.Setup(db => db.CreateItemAsync(
                connectionSettings.Object,
                examination,
                false)).Returns(Task.FromResult(examination)).Verifiable();
            var sut = new CreateExaminationService(dbAccess.Object, connectionSettings.Object, locationService.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            Assert.NotNull(result.Result);
            Assert.Equal(0, result.Result.GetCaseUrgencyScore());
            Assert.Equal(myUser.Object.UserId, result.Result.LastModifiedBy);
        }

        /// <summary>
        /// Test to make sure UpdateCaseUrgencyScoreAndSort method is called when the Examination is created
        /// </summary>
        [Fact]
        public void CreateExaminationQueryWithAllUrgencyIndicatorsSuccessReturnsExaminationWithUrgencyScore500()
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
            var myUser = new Mock<MeUser>();
            CreateExaminationQuery query = new CreateExaminationQuery(examination, myUser.Object);
            var dbAccess = new Mock<IDatabaseAccess>();
            var locationConnectionSettings = new Mock<ILocationConnectionSettings>();
            var location = new MedicalExaminer.Models.Location();
            var locationService = new Mock<LocationIdService>(dbAccess.Object, locationConnectionSettings.Object);
            locationService.Setup(x => x.Handle(It.IsAny<LocationRetrievalByIdQuery>())).Returns(Task.FromResult(location));
            dbAccess.Setup(db => db.CreateItemAsync(
                connectionSettings.Object,
                examination,
                false)).Returns(Task.FromResult(examination)).Verifiable();
            var sut = new CreateExaminationService(dbAccess.Object, connectionSettings.Object, locationService.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            Assert.NotNull(result.Result);
            Assert.Equal(500, result.Result.GetCaseUrgencyScore());
            Assert.Equal(myUser.Object.UserId, result.Result.LastModifiedBy);
        }
    }
}