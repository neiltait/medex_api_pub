using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.PatientDetails;
using MedicalExaminer.Common.Services.PatientDetails;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.PatientDetails
{
    public class PatientDetailsUpdateServiceTests
    {
        [Fact]
        public void PatientDetailsUpdateQuerySuccessReturnsExaminationId()
        {
            // Arrange
            MedicalExaminer.Models.Examination examination = new MedicalExaminer.Models.Examination();
            var patientDetails = new Mock<MedicalExaminer.Models.PatientDetails>();
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var mapper = new Mock<IMapper>();
            var query = new PatientDetailsUpdateQuery("a", patientDetails.Object);
            var dbAccess = new Mock<IDatabaseAccess>();
            dbAccess.Setup((db) => db.GetItemAsync(connectionSettings.Object,
                It.IsAny<Expression<Func<MedicalExaminer.Models.Examination, bool>>>())).Returns(Task.FromResult(examination)).Verifiable();
            dbAccess.Setup((db) => db.UpdateItemAsync(connectionSettings.Object,
                It.IsAny<MedicalExaminer.Models.Examination>())).Returns(Task.FromResult(examination)).Verifiable();
            var sut = new PatientDetailsUpdateService(dbAccess.Object, connectionSettings.Object, mapper.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            dbAccess.Verify(db => db.UpdateItemAsync(connectionSettings.Object, 
                It.IsAny<MedicalExaminer.Models.Examination>()), Times.Once);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public void PatientDetailsUpdateQuerySuccessReturnsCorrectPropertyValues()
        {
            //TODO
            // Arrange
            MedicalExaminer.Models.Examination examination = new MedicalExaminer.Models.Examination();
            var patientDetails = new Mock<MedicalExaminer.Models.PatientDetails>();
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var mapper = new Mock<IMapper>();
            var query = new PatientDetailsUpdateQuery("a", patientDetails.Object);
            var dbAccess = new Mock<IDatabaseAccess>();
            dbAccess.Setup((db) => db.GetItemAsync(connectionSettings.Object,
                It.IsAny<Expression<Func<MedicalExaminer.Models.Examination, bool>>>())).Returns(Task.FromResult(examination)).Verifiable();
            dbAccess.Setup((db) => db.UpdateItemAsync(connectionSettings.Object,
                It.IsAny<MedicalExaminer.Models.Examination>())).Returns(Task.FromResult(examination)).Verifiable();
            var sut = new PatientDetailsUpdateService(dbAccess.Object, connectionSettings.Object, mapper.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            dbAccess.Verify(db => db.UpdateItemAsync(connectionSettings.Object,
                It.IsAny<MedicalExaminer.Models.Examination>()), Times.Once);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public void PatientDetailsUpdateQueryIsNullThrowsException()
        {
            // Arrange
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            PatientDetailsUpdateQuery query = null;
            var mapper = new Mock<IMapper>();
            var dbAccess = new Mock<IDatabaseAccess>();
            var sut = new PatientDetailsUpdateService(dbAccess.Object, connectionSettings.Object, mapper.Object);

            Action act = () => sut.Handle(query).GetAwaiter().GetResult();
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
