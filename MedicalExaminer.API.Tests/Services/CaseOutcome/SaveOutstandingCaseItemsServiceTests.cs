using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.CaseOutcome;
using MedicalExaminer.Common.Services.CaseOutcome;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.CaseOutcome
{
    public class SaveOutstandingCaseItemsServiceTests
    {
        /// <summary>
        /// Save outstanding case items when Scrutiny is not confirmed
        /// </summary>
        [Fact]
        public void Save_Outstanding_Case_Items_When_Scrutiny_Not_Confirmed_Returns_Null()
        {
            // Arrange
            var examinationId = Guid.NewGuid().ToString();
            var examination = new MedicalExaminer.Models.Examination
            {
                ExaminationId = examinationId,
                ScrutinyConfirmed = false
            };
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var caseOutcomeItems = new MedicalExaminer.Models.CaseOutcome
            {
                MCCDIssued = true,
                CremationFormStatus = CremationFormStatus.No,
                GPNotifiedStatus = GPNotified.GPNotified
            };
            var query = new SaveOutstandingCaseItemsQuery(examinationId, caseOutcomeItems, new MeUser());
            var dbAccess = new Mock<IDatabaseAccess>();

            dbAccess.Setup(db => db.GetItemAsync(connectionSettings.Object,
                    It.IsAny<Expression<Func<MedicalExaminer.Models.Examination, bool>>>()))
                .Returns(Task.FromResult(examination)).Verifiable();

            dbAccess.Setup(db => db.UpdateItemAsync(connectionSettings.Object,
                It.IsAny<MedicalExaminer.Models.Examination>())).Returns(Task.FromResult(examination)).Verifiable();

            var sut = new SaveOutstandingCaseItemsService(dbAccess.Object, connectionSettings.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            Assert.Null(result.Result);
        }

        /// <summary>
        /// Save outstanding case items when Scrutiny is not confirmed
        /// </summary>
        [Fact]
        public void Save_Outstanding_Case_Items_When_Scrutiny_Is_Confirmed_Returns_ExaminationID()
        {
            // Arrange
            var examinationId = Guid.NewGuid().ToString();
            var examination = new MedicalExaminer.Models.Examination
            {
                ExaminationId = examinationId,
                MedicalTeam = new MedicalExaminer.Models.MedicalTeam()
                {
                    MedicalExaminerOfficerUserId = "MedicalExaminerOfficerUserId",
                    MedicalExaminerUserId = "MedicalExaminerUserId"
                },
                ScrutinyConfirmed = true
            };
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var caseOutcomeItems = new MedicalExaminer.Models.CaseOutcome
            {
                MCCDIssued = true,
                CremationFormStatus = CremationFormStatus.No,
                GPNotifiedStatus = GPNotified.GPNotified
            };
            var query = new SaveOutstandingCaseItemsQuery(examinationId, caseOutcomeItems, new MeUser());
            var dbAccess = new Mock<IDatabaseAccess>();

            dbAccess.Setup(db => db.GetItemAsync(connectionSettings.Object,
                    It.IsAny<Expression<Func<MedicalExaminer.Models.Examination, bool>>>()))
                .Returns(Task.FromResult(examination)).Verifiable();

            dbAccess.Setup(db => db.UpdateItemAsync(connectionSettings.Object,
                It.IsAny<MedicalExaminer.Models.Examination>())).Returns(Task.FromResult(examination)).Verifiable();

            var sut = new SaveOutstandingCaseItemsService(dbAccess.Object, connectionSettings.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            Assert.NotNull(result.Result);
            Assert.True(examination.OutstandingCaseItemsCompleted);
            Assert.Equal(examinationId, result.Result);
        }
    }
}
