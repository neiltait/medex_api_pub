using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.CaseOutcome;
using MedicalExaminer.Common.Services.CaseOutcome;
using MedicalExaminer.Models;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.CaseOutcome
{
    public class CloseCaseServiceTests
    {
        /// <summary>
        /// Close Case When Outstanding Case Items Are Not CaseCompleted
        /// </summary>
        [Fact]
        public void Close_Case_When_Outstanding_Case_Items_Are_Not_Completed_Returns_Null()
        {
            // Arrange
            var examinationId = Guid.NewGuid().ToString();
            var examination = new MedicalExaminer.Models.Examination
            {
                ExaminationId = examinationId,
                ScrutinyConfirmed = false,
                OutstandingCaseItemsCompleted = false
            };
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var query = new CloseCaseQuery(examinationId, new MeUser());
            var dbAccess = new Mock<IDatabaseAccess>();

            dbAccess.Setup(db => db.GetItemAsync(
                connectionSettings.Object,
                    It.IsAny<Expression<Func<MedicalExaminer.Models.Examination, bool>>>()))
                .Returns(Task.FromResult(examination)).Verifiable();

            dbAccess.Setup(db => db.UpdateItemAsync(
                connectionSettings.Object,
                It.IsAny<MedicalExaminer.Models.Examination>())).Returns(Task.FromResult(examination)).Verifiable();

            var sut = new CloseCaseService(dbAccess.Object, connectionSettings.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            Assert.Null(result.Result);
        }

        /// <summary>
        /// Close Case When Outstanding Case Items Are CaseCompleted
        /// </summary>
        [Fact]
        public void Close_Case_When_Outstanding_Case_Items_Completed_Marks_Examination_Completed_And_Returns_ExaminationID()
        {
            // Arrange
            var examinationId = Guid.NewGuid().ToString();
            var examination = new MedicalExaminer.Models.Examination
            {
                ExaminationId = examinationId,
                ScrutinyConfirmed = false,
                OutstandingCaseItemsCompleted = true
            };
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var query = new CloseCaseQuery(examinationId, new MeUser());
            var dbAccess = new Mock<IDatabaseAccess>();

            dbAccess.Setup(db => db.GetItemAsync(
                connectionSettings.Object,
                    It.IsAny<Expression<Func<MedicalExaminer.Models.Examination, bool>>>()))
                .Returns(Task.FromResult(examination)).Verifiable();

            dbAccess.Setup(db => db.UpdateItemAsync(
                connectionSettings.Object,
                It.IsAny<MedicalExaminer.Models.Examination>())).Returns(Task.FromResult(examination)).Verifiable();

            var sut = new CloseCaseService(dbAccess.Object, connectionSettings.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            Assert.NotNull(result.Result);
            Assert.True(examination.CaseCompleted);
            Assert.Equal(examinationId, result.Result);
        }
    }
}
