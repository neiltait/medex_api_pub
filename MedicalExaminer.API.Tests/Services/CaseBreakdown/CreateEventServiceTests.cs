using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.CaseBreakdown;
using MedicalExaminer.Common.Services.Examination;
using MedicalExaminer.Common.Settings;
using MedicalExaminer.Models;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.CaseBreakdown
{
    public class CreateEventServiceTests
    {
        private readonly Mock<IOptions<UrgencySettings>> _urgencySettingsMock;

        public CreateEventServiceTests()
        {
            _urgencySettingsMock = new Mock<IOptions<UrgencySettings>>(MockBehavior.Strict);
            _urgencySettingsMock
                .Setup(s => s.Value)
                .Returns(new UrgencySettings
                {
                    DaysToPreCalculateUrgencySort = 1
                });
        }

        /// <summary>
        /// Behavior when incoming Create Event Query is NUll.
        /// </summary>
        [Fact]
        public void CreateEventQueryIsNullThrowsException()
        {
            // Arrange
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            CreateEventQuery query = null;
            var dbAccess = new Mock<IDatabaseAccess>();

            var sut = new CreateEventService(dbAccess.Object, connectionSettings.Object, _urgencySettingsMock.Object);

            // Act
            Action act = () => sut.Handle(query).GetAwaiter().GetResult();
            act.Should().Throw<ArgumentNullException>();
        }

        /// <summary>
        /// Successful Creation of an Event.
        /// </summary>
        [Fact]
        public void CreateEventQuerySuccessReturnsEventId()
        {
            // Arrange
            var examination = new MedicalExaminer.Models.Examination();
            var theEvent = new Mock<OtherEvent>(); // only other event has been implemented so far.
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var query = new CreateEventQuery("1", theEvent.Object);
            theEvent.Object.UserId = "a";
            var dbAccess = new Mock<IDatabaseAccess>();

            dbAccess.Setup(db => db.GetItemByIdAsync<MedicalExaminer.Models.Examination>(connectionSettings.Object,
                    It.IsAny<string>()))
                .Returns(Task.FromResult(examination)).Verifiable();

            dbAccess.Setup(db => db.UpdateItemAsync(connectionSettings.Object,
                It.IsAny<MedicalExaminer.Models.Examination>())).Returns(Task.FromResult(examination)).Verifiable();

            var sut = new CreateEventService(dbAccess.Object, connectionSettings.Object, _urgencySettingsMock.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            dbAccess.Verify(db => db.UpdateItemAsync(connectionSettings.Object, 
                It.IsAny<MedicalExaminer.Models.Examination>()), Times.Once);

            Assert.NotNull(result.Result);
            Assert.Equal("a", examination.LastModifiedBy);

        }

        /// <summary>
        /// Test to make sure UpdateCaseUrgencySort method is called whenever the Examination is updated
        /// </summary>
        [Fact]
        public void CreateEventOnExaminationWithNoUrgencyIndicatorsSuccessReturnsExaminationWithUrgencyScoreZero()
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
            var theEvent = new Mock<OtherEvent>(); // only other event has been implemented so far.
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var query = new CreateEventQuery("1", theEvent.Object);
            theEvent.Object.UserId = "a";
            var dbAccess = new Mock<IDatabaseAccess>();

            dbAccess.Setup(db => db.GetItemByIdAsync<MedicalExaminer.Models.Examination>(connectionSettings.Object,
                    It.IsAny<string>()))
                .Returns(Task.FromResult(examination)).Verifiable();

            dbAccess.Setup(db => db.UpdateItemAsync(connectionSettings.Object,
                It.IsAny<MedicalExaminer.Models.Examination>())).Returns(Task.FromResult(examination)).Verifiable();

            var sut = new CreateEventService(dbAccess.Object, connectionSettings.Object, _urgencySettingsMock.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            examination.IsUrgent().Should().BeFalse();
            examination.LastModifiedBy.Should().Be("a");
        }

        /// <summary>
        /// Test to make sure UpdateCaseUrgencySort method is called whenever the Examination is updated
        /// </summary>
        [Fact]
        public void CreateEventOnExaminationWithAllUrgencyIndicatorsSuccessReturnsExaminationWithUrgencyScore500()
        {
            // Arrange
            var examination = new MedicalExaminer.Models.Examination()
            {
                ChildPriority = true,
                CoronerPriority = true,
                CulturalPriority = true,
                FaithPriority = true,
                OtherPriority = true,
                CreatedAt = DateTime.Now.AddDays(-3),
            };
            var theEvent = new Mock<OtherEvent>(); // only other event has been implemented so far.
            theEvent.Object.UserId = "a";
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var query = new CreateEventQuery("1", theEvent.Object);
            var dbAccess = new Mock<IDatabaseAccess>();

            dbAccess
                .Setup(db => db.GetItemByIdAsync<MedicalExaminer.Models.Examination>(
                    connectionSettings.Object,
                    It.IsAny<string>()))
                .Returns(Task.FromResult(examination)).Verifiable();

            dbAccess
                .Setup(db => db.UpdateItemAsync(
                    connectionSettings.Object,
                    It.IsAny<MedicalExaminer.Models.Examination>()))
                .Returns(Task.FromResult(examination)).Verifiable();

            var sut = new CreateEventService(dbAccess.Object, connectionSettings.Object, _urgencySettingsMock.Object);

            // Act
            var result = sut.Handle(query).Result;

            // Assert
            result.Should().NotBeNull();
            examination.IsUrgent().Should().BeTrue();
            examination.LastModifiedBy.Should().Be("a");
        }
    }
}
