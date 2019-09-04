using System;
using System.Linq.Expressions;
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
    public class CloseCaseServiceTests
    {
        private readonly Mock<IOptions<UrgencySettings>> _urgencySettingsMock;

        public CloseCaseServiceTests()
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
        /// Close Case When Outstanding Case Items Are CaseCompleted
        /// </summary>
        [Fact]
        public void Close_Case_When_Outstanding_Case_Items_Completed_Marks_Examination_Completed_And_Returns_ExaminationID()
        {
            // Arrange
            var examinationId = Guid.NewGuid().ToString();
            var caseOutcome = new MedicalExaminer.Models.CaseOutcome
            {
                CaseMedicalExaminerFullName = "ME Full Name",
                ScrutinyConfirmedOn = new DateTime(2019, 6, 20),
                OutcomeQapDiscussion = QapDiscussionOutcome.MccdCauseOfDeathProvidedByQAP,
                OutcomeOfPrescrutiny = OverallOutcomeOfPreScrutiny.IssueAnMccd,
                OutcomeOfRepresentativeDiscussion = BereavedDiscussionOutcome.CauseOfDeathAccepted,
                CaseCompleted = false,
                CaseOutcomeSummary = CaseOutcomeSummary.IssueMCCD,
                MccdIssued = true,
                CremationFormStatus = CremationFormStatus.Yes,
                GpNotifiedStatus = GPNotified.GPNotified,
                CoronerReferralSent = false
            };
            var examination = new MedicalExaminer.Models.Examination
            {
                ExaminationId = examinationId,
                ScrutinyConfirmed = false,
                OutstandingCaseItemsCompleted = true,
                CaseOutcome = caseOutcome
            };
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var query = new CloseCaseQuery(examinationId, new MeUser());
            var dbAccess = new Mock<IDatabaseAccess>();

            dbAccess.Setup(db => db.GetItemByIdAsync<MedicalExaminer.Models.Examination>(
                connectionSettings.Object,
                    It.IsAny<string>()))
                .Returns(Task.FromResult(examination)).Verifiable();

            dbAccess.Setup(db => db.UpdateItemAsync(
                connectionSettings.Object,
                It.IsAny<MedicalExaminer.Models.Examination>())).Returns(Task.FromResult(examination)).Verifiable();

            var sut = new CloseCaseService(dbAccess.Object, connectionSettings.Object, _urgencySettingsMock.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            Assert.NotNull(result.Result);
            Assert.True(examination.CaseCompleted);
            Assert.Equal(examinationId, result.Result);
        }
    }
}
