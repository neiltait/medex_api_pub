using System;
using System.Collections.Generic;
using System.Text;
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
    public class VoidCaseServiceTests
    {
        private readonly Mock<IOptions<UrgencySettings>> _urgencySettingsMock;

        public VoidCaseServiceTests()
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
        /// Void Case When Outstanding Case Items Are CaseCompleted
        /// </summary>
        [Fact]
        public void Void_Case_Marks_Examination_IsVoid_And_Returns_Examination()
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
            var user = new MeUser
            {
                UserId = "UserId",
                FirstName = "FirstName",
                LastName = "LastName",
                GmcNumber = "GmcNumber"
            };
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var query = new VoidCaseQuery(examinationId, user, "Void Reason");
            var dbAccess = new Mock<IDatabaseAccess>();

            dbAccess.Setup(db => db.GetItemByIdAsync<MedicalExaminer.Models.Examination>(
                connectionSettings.Object,
                    It.IsAny<string>()))
                .Returns(Task.FromResult(examination)).Verifiable();

            dbAccess.Setup(db => db.UpdateItemAsync(
                connectionSettings.Object,
                It.IsAny<MedicalExaminer.Models.Examination>())).Returns(Task.FromResult(examination)).Verifiable();

            var sut = new VoidCaseService(dbAccess.Object, connectionSettings.Object, _urgencySettingsMock.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            Assert.NotNull(result.Result);
            Assert.True(examination.IsVoid);
            Assert.Equal(examination, result.Result);
            Assert.NotNull(examination.VoidedDate);
            Assert.Equal(DateTime.Now.Date, examination.VoidedDate.Value.Date);
            Assert.NotNull(examination.CaseBreakdown.VoidEvent);
            Assert.Equal(user.FirstName + " " + user.LastName, examination.CaseBreakdown.VoidEvent.UserFullName);
            Assert.Equal(user.UserId, examination.CaseBreakdown.VoidEvent.UserId);
            Assert.Equal(user.GmcNumber, examination.CaseBreakdown.VoidEvent.GmcNumber);
        }
    }
}
