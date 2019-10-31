using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.CaseOutcome;
using MedicalExaminer.Common.Services.CaseOutcome;
using MedicalExaminer.Common.Settings;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.CaseOutcome
{
    public class ConfirmationOfScrutinyServiceTests
    {
        private readonly Mock<IOptions<UrgencySettings>> _urgencySettingsMock;

        public ConfirmationOfScrutinyServiceTests()
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
        /// Confirm scrutiny
        /// </summary>
        [Fact]
        public void Confirmation_Of_Scrutiny_Marks_ScrutinyConfirmed_To_True_And_Returns_ScrutinyConfirmedOn_DateTime()
        {
            // Arrange
            var examinationId = Guid.NewGuid().ToString();
            var caseOutcome = new MedicalExaminer.Models.CaseOutcome
            {
                ScrutinyConfirmedOn = null
            };
            var examination = new MedicalExaminer.Models.Examination
            {
                ExaminationId = examinationId,
                ScrutinyConfirmed = false,
                CaseOutcome = caseOutcome
            };
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var query = new ConfirmationOfScrutinyQuery(examinationId, new MeUser());
            var dbAccess = new Mock<IDatabaseAccess>();

            dbAccess.Setup(db => db.GetItemByIdAsync<MedicalExaminer.Models.Examination>(
                connectionSettings.Object,
                    It.IsAny<string>()))
                .Returns(Task.FromResult(examination)).Verifiable();

            dbAccess.Setup(db => db.UpdateItemAsync(
                connectionSettings.Object,
                It.IsAny<MedicalExaminer.Models.Examination>())).Returns(Task.FromResult(examination)).Verifiable();

            var sut = new ConfirmationOfScrutinyService(dbAccess.Object, connectionSettings.Object, _urgencySettingsMock.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            Assert.NotNull(result.Result);
            Assert.True(examination.ScrutinyConfirmed);
            Assert.NotNull(examination.CaseOutcome.ScrutinyConfirmedOn);
        }
    }
}
