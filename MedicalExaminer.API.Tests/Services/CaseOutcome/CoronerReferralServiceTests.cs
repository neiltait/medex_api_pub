using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.CaseOutcome;
using MedicalExaminer.Common.Services.CaseOutcome;
using MedicalExaminer.Common.Settings;
using MedicalExaminer.Models;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.CaseOutcome
{
    public class CoronerReferralServiceTests
    {
        private readonly Mock<IOptions<UrgencySettings>> _urgencySettingsMock;

        public CoronerReferralServiceTests()
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
        /// Send Coroner Referral Sets CoronerReferralSent To True And Returns ExaminationID
        /// </summary>
        [Fact]
        public void Send_Coroner_Referral_Sets_CoronerReferralSent_To_True_And_Returns_ExaminationID()
        {
            // Arrange
            var examinationId = Guid.NewGuid().ToString();
            var examination = new MedicalExaminer.Models.Examination
            {
                ExaminationId = examinationId,
                ScrutinyConfirmed = false
            };
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var query = new CoronerReferralQuery(examinationId, new MeUser());
            var dbAccess = new Mock<IDatabaseAccess>();

            dbAccess.Setup(db => db.GetItemByIdAsync<MedicalExaminer.Models.Examination>(
                    connectionSettings.Object,
                    It.IsAny<string>()))
                .Returns(Task.FromResult(examination)).Verifiable();

            dbAccess.Setup(db => db.UpdateItemAsync(
                connectionSettings.Object,
                It.IsAny<MedicalExaminer.Models.Examination>())).Returns(Task.FromResult(examination)).Verifiable();

            var sut = new CoronerReferralService(dbAccess.Object, connectionSettings.Object, _urgencySettingsMock.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            Assert.NotNull(result.Result);
            Assert.True(examination.CoronerReferralSent);
            Assert.Equal(examinationId, result.Result);
        }
    }
}
