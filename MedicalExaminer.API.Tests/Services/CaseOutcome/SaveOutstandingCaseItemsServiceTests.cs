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
            var caseOutcome = new MedicalExaminer.Models.CaseOutcome
            {
                MccdIssued = true,
                CremationFormStatus = CremationFormStatus.No,
                GpNotifiedStatus = GPNotified.GPNotified
            };
            var query = new SaveOutstandingCaseItemsQuery(examinationId, caseOutcome, new MeUser());
            var dbAccess = new Mock<IDatabaseAccess>();

            dbAccess.Setup(db => db.GetItemByIdAsync<MedicalExaminer.Models.Examination>(
                connectionSettings.Object,
                    It.IsAny<string>()))
                .Returns(Task.FromResult(examination)).Verifiable();

            dbAccess.Setup(db => db.UpdateItemAsync(
                connectionSettings.Object,
                It.IsAny<MedicalExaminer.Models.Examination>())).Returns(Task.FromResult(examination)).Verifiable();

            var sut = new SaveOutstandingCaseItemsService(dbAccess.Object, connectionSettings.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            Assert.Null(result.Result);
        }

        /// <summary>
        /// correctly setup such that it meets the criteria
        /// for scrutiny complete
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
            var caseOutcome = new MedicalExaminer.Models.CaseOutcome
            {
                MccdIssued = true,
                CremationFormStatus = CremationFormStatus.Yes,
                GpNotifiedStatus = GPNotified.GPNotified
            };
            var query = new SaveOutstandingCaseItemsQuery(examinationId, caseOutcome, new MeUser());
            var dbAccess = new Mock<IDatabaseAccess>();

            dbAccess.Setup(db => db.GetItemByIdAsync<MedicalExaminer.Models.Examination>(
                    connectionSettings.Object,
                    It.IsAny<string>()))
                .Returns(Task.FromResult(examination)).Verifiable();

            dbAccess.Setup(db => db.UpdateItemAsync(
                connectionSettings.Object,
                It.IsAny<MedicalExaminer.Models.Examination>())).Returns(Task.FromResult(examination)).Verifiable();

            var sut = new SaveOutstandingCaseItemsService(dbAccess.Object, connectionSettings.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            Assert.NotNull(result.Result);
            Assert.True(examination.OutstandingCaseItemsCompleted);
            Assert.Equal(examinationId, result.Result);
        }

        /// <summary>
        /// Save Outstanding Case Items With No Cremation Form Status Does Make Outstanding CaseItems Completed
        /// </summary>
        [Fact]
        public void Save_Outstanding_Case_Items_With_No_CremationFormStatus_Does_Make_OutstandingCaseItemsCompleted()
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
            var caseOutcome = new MedicalExaminer.Models.CaseOutcome
            {
                MccdIssued = true,
                CremationFormStatus = CremationFormStatus.No,
                GpNotifiedStatus = GPNotified.GPNotified
            };
            var query = new SaveOutstandingCaseItemsQuery(examinationId, caseOutcome, new MeUser());
            var dbAccess = new Mock<IDatabaseAccess>();

            dbAccess.Setup(db => db.GetItemByIdAsync<MedicalExaminer.Models.Examination>(
                    connectionSettings.Object,
                    It.IsAny<string>()))
                .Returns(Task.FromResult(examination)).Verifiable();

            dbAccess.Setup(db => db.UpdateItemAsync(
                connectionSettings.Object,
                It.IsAny<MedicalExaminer.Models.Examination>())).Returns(Task.FromResult(examination)).Verifiable();

            var sut = new SaveOutstandingCaseItemsService(dbAccess.Object, connectionSettings.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            Assert.NotNull(result.Result);
            Assert.True(examination.OutstandingCaseItemsCompleted);
            Assert.Equal(examinationId, result.Result);
        }

        /// <summary>
        /// Save Outstanding Case Items With No Cremation Form Status Does Make Outstanding CaseItems Completed
        /// </summary>
        [Fact]
        public void Save_Outstanding_Case_Items_With_Unknown_CremationFormStatus_Does_Make_OutstandingCaseItemsCompleted()
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
            var caseOutcome = new MedicalExaminer.Models.CaseOutcome
            {
                MccdIssued = true,
                CremationFormStatus = CremationFormStatus.Unknown,
                GpNotifiedStatus = GPNotified.GPNotified
            };
            var query = new SaveOutstandingCaseItemsQuery(examinationId, caseOutcome, new MeUser());
            var dbAccess = new Mock<IDatabaseAccess>();

            dbAccess.Setup(db => db.GetItemByIdAsync<MedicalExaminer.Models.Examination>(
                    connectionSettings.Object,
                    It.IsAny<string>()))
                .Returns(Task.FromResult(examination)).Verifiable();

            dbAccess.Setup(db => db.UpdateItemAsync(
                connectionSettings.Object,
                It.IsAny<MedicalExaminer.Models.Examination>())).Returns(Task.FromResult(examination)).Verifiable();

            var sut = new SaveOutstandingCaseItemsService(dbAccess.Object, connectionSettings.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            Assert.NotNull(result.Result);
            Assert.True(examination.OutstandingCaseItemsCompleted);
            Assert.Equal(examinationId, result.Result);
        }

        /// <summary>
        /// Save Outstanding Case Items With No GP Notified Does Make Outstanding CaseItems Completed
        /// </summary>
        [Fact]
        public void Save_Outstanding_Case_Items_With_No_GPNotified_Does_Make_OutstandingCaseItemsCompleted()
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
            var caseOutcome = new MedicalExaminer.Models.CaseOutcome
            {
                MccdIssued = true,
                CremationFormStatus = CremationFormStatus.No,
                GpNotifiedStatus = GPNotified.GPUnabledToBeNotified
            };
            var query = new SaveOutstandingCaseItemsQuery(examinationId, caseOutcome, new MeUser());
            var dbAccess = new Mock<IDatabaseAccess>();

            dbAccess.Setup(db => db.GetItemByIdAsync<MedicalExaminer.Models.Examination>(
                    connectionSettings.Object,
                    It.IsAny<string>()))
                .Returns(Task.FromResult(examination)).Verifiable();

            dbAccess.Setup(db => db.UpdateItemAsync(
                connectionSettings.Object,
                It.IsAny<MedicalExaminer.Models.Examination>())).Returns(Task.FromResult(examination)).Verifiable();

            var sut = new SaveOutstandingCaseItemsService(dbAccess.Object, connectionSettings.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            Assert.NotNull(result.Result);
            Assert.True(examination.OutstandingCaseItemsCompleted);
            Assert.Equal(examinationId, result.Result);
        }

        /// <summary>
        /// Save Outstanding Case Items With NA GP Notified Does Make Outstanding CaseItems Completed
        /// </summary>
        [Fact]
        public void Save_Outstanding_Case_Items_With_GPNotified_NA_Does_Make_OutstandingCaseItemsCompleted()
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
            var caseOutcome = new MedicalExaminer.Models.CaseOutcome
            {
                MccdIssued = true,
                CremationFormStatus = CremationFormStatus.No,
                GpNotifiedStatus = GPNotified.NA
            };
            var query = new SaveOutstandingCaseItemsQuery(examinationId, caseOutcome, new MeUser());
            var dbAccess = new Mock<IDatabaseAccess>();

            dbAccess.Setup(db => db.GetItemByIdAsync<MedicalExaminer.Models.Examination>(
                    connectionSettings.Object,
                    It.IsAny<string>()))
                .Returns(Task.FromResult(examination)).Verifiable();

            dbAccess.Setup(db => db.UpdateItemAsync(
                connectionSettings.Object,
                It.IsAny<MedicalExaminer.Models.Examination>())).Returns(Task.FromResult(examination)).Verifiable();

            var sut = new SaveOutstandingCaseItemsService(dbAccess.Object, connectionSettings.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            Assert.NotNull(result.Result);
            Assert.True(examination.OutstandingCaseItemsCompleted);
            Assert.Equal(examinationId, result.Result);
        }

        /// <summary>
        /// Save Outstanding Case Items completed Makes Outstanding CaseItems Completed
        /// </summary>
        [Fact]
        public void Save_Outstanding_Case_Items_Completed_Makes_OutstandingCaseItemsCompleted()
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
            var caseOutcome = new MedicalExaminer.Models.CaseOutcome
            {
                MccdIssued = true,
                CremationFormStatus = CremationFormStatus.Yes,
                GpNotifiedStatus = GPNotified.GPNotified
            };
            var query = new SaveOutstandingCaseItemsQuery(examinationId, caseOutcome, new MeUser());
            var dbAccess = new Mock<IDatabaseAccess>();

            dbAccess.Setup(db => db.GetItemByIdAsync<MedicalExaminer.Models.Examination>(
                    connectionSettings.Object,
                    It.IsAny<string>()))
                .Returns(Task.FromResult(examination)).Verifiable();

            dbAccess.Setup(db => db.UpdateItemAsync(
                connectionSettings.Object,
                It.IsAny<MedicalExaminer.Models.Examination>())).Returns(Task.FromResult(examination)).Verifiable();

            var sut = new SaveOutstandingCaseItemsService(dbAccess.Object, connectionSettings.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            Assert.NotNull(result.Result);
            Assert.True(examination.OutstandingCaseItemsCompleted);
            Assert.Equal(examinationId, result.Result);
        }

        /// <summary>
        /// Save Outstanding Case Items completed with NA GPNotified Makes Outstanding CaseItems Completed
        /// </summary>
        [Fact]
        public void Save_Outstanding_Case_Items_Completed_With_NA_GPNotified_Makes_OutstandingCaseItemsCompleted()
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
            var caseOutcome = new MedicalExaminer.Models.CaseOutcome
            {
                MccdIssued = true,
                CremationFormStatus = CremationFormStatus.Yes,
                GpNotifiedStatus = GPNotified.NA
            };
            var query = new SaveOutstandingCaseItemsQuery(examinationId, caseOutcome, new MeUser());
            var dbAccess = new Mock<IDatabaseAccess>();

            dbAccess.Setup(db => db.GetItemByIdAsync<MedicalExaminer.Models.Examination>(
                    connectionSettings.Object,
                    It.IsAny<string>()))
                .Returns(Task.FromResult(examination)).Verifiable();

            dbAccess.Setup(db => db.UpdateItemAsync(
                connectionSettings.Object,
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
