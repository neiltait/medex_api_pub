using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Services.MedicalTeam;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.MedicalTeam
{
    public class MedicalTeamUpdateServiceTests
    {
        [Fact]
        public async void ExaminationFound_ReturnsExaminationId()
        {
            // Arrange
            var examinationId = "1";
            var examination1 = new MedicalExaminer.Models.Examination
            {
                ExaminationId = examinationId
            };
            IEnumerable<MedicalExaminer.Models.Examination> examinations = new List<MedicalExaminer.Models.Examination>
                { examination1 };
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var dbAccess = new Mock<IDatabaseAccess>();
            dbAccess.Setup(db => db.UpdateItemAsync(connectionSettings.Object, examination1))
                .Returns(Task.FromResult(examination1));
            var sut = new MedicalTeamUpdateService(dbAccess.Object, connectionSettings.Object);

            // Act
            var result = await sut.Handle(examination1);

            Assert.Equal(examinationId, result.ExaminationId);
        }

        [Fact]
        public async void UpdateMedicalTeamOfExaminationWithNoUrgencyIndicatorsThenCaseUrgencyScoreUpdatedWithZero()
        {
            // Arrange
            var examination = new MedicalExaminer.Models.Examination
            {
                ChildPriority = false,
                CoronerPriority = false,
                CulturalPriority = false,
                FaithPriority = false,
                OtherPriority = false,
                CaseCreated = DateTime.Now.AddDays(-3)
            };
            IEnumerable<MedicalExaminer.Models.Examination> examinations = new List<MedicalExaminer.Models.Examination>
                { examination };
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var dbAccess = new Mock<IDatabaseAccess>();
            dbAccess.Setup(db => db.UpdateItemAsync(connectionSettings.Object, examination))
                .Returns(Task.FromResult(examination));
            var sut = new MedicalTeamUpdateService(dbAccess.Object, connectionSettings.Object);

            // Act
            var result = await sut.Handle(examination);

            Assert.Equal(0, result.UrgencyScore);
        }

        [Fact]
        public async void UpdateMedicalTeamOfExaminationWithAllUrgencyIndicatorsThenCaseUrgencyScoreUpdatedWith500()
        {
            // Arrange
            var examination = new MedicalExaminer.Models.Examination
            {
                ChildPriority = true,
                CoronerPriority = true,
                CulturalPriority = true,
                FaithPriority = true,
                OtherPriority = true,
                CaseCreated = DateTime.Now.AddDays(-3)
            };
            IEnumerable<MedicalExaminer.Models.Examination> examinations = new List<MedicalExaminer.Models.Examination>
                { examination };
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var dbAccess = new Mock<IDatabaseAccess>();
            dbAccess.Setup(db => db.UpdateItemAsync(connectionSettings.Object, examination))
                .Returns(Task.FromResult(examination));
            var sut = new MedicalTeamUpdateService(dbAccess.Object, connectionSettings.Object);

            // Act
            var result = await sut.Handle(examination);

            Assert.Equal(500, result.UrgencyScore);
        }

        [Fact]
        public void ExaminationIdNull_ThrowsException()
        {
            // Arrange
            // IEnumerable<MedicalExaminer.Models.Examination> examinations = null;
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var dbAccess = new Mock<IDatabaseAccess>();
            dbAccess.Setup(db =>
                    db.GetItemAsync(connectionSettings.Object,
                        It.IsAny<Expression<Func<MedicalExaminer.Models.Examination, bool>>>()))
                .Returns(Task.FromResult<MedicalExaminer.Models.Examination>(null));

            var sut = new MedicalTeamUpdateService(dbAccess.Object, connectionSettings.Object);

            // Act

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(null));
        }
    }
}