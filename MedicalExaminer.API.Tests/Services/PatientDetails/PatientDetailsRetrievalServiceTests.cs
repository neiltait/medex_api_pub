using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.PatientDetails;
using MedicalExaminer.Common.Services.Examination;
using MedicalExaminer.Common.Services.PatientDetails;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.PatientDetails
{
    public class PatientDetailsRetrievalServiceTests
    {
        [Fact]
        public void ExaminationIdNotFoundReturnsNull()
        {
            // Arrange
            var examinationId = "a";
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var query = new Mock<PatientDetailsByCaseIdQuery>(examinationId);
            var dbAccess = new Mock<IDatabaseAccess>();

            dbAccess.Setup(db => db.GetItemAsync(connectionSettings.Object,
                    It.IsAny<Expression<Func<MedicalExaminer.Models.Examination, bool>>>()))
                .Returns(Task.FromResult<MedicalExaminer.Models.Examination>(null)).Verifiable();
            var sut = new PatientDetailsRetrievalService(dbAccess.Object, connectionSettings.Object);
            var expected = default(MedicalExaminer.Models.Examination);

            // Act
            var result = sut.Handle(query.Object);

            // Assert
            dbAccess.Verify(db => db.GetItemAsync<MedicalExaminer.Models.Examination>(connectionSettings.Object,
                It.IsAny<Expression<Func<MedicalExaminer.Models.Examination, bool>>>()), Times.Once);

            Assert.Equal(expected, result.Result);

        }

        [Fact]
        public void ExaminationIdIsNullThrowsException()
        {
            // Arrange
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            PatientDetailsByCaseIdQuery query = null;
            var dbAccess = new Mock<IDatabaseAccess>();
            var sut = new PatientDetailsRetrievalService(dbAccess.Object, connectionSettings.Object);

            Action act = () => sut.Handle(query).GetAwaiter().GetResult();
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ExaminationIdFoundReturnsResult()
        {
            var examinationId = "a";
            var examination = new MedicalExaminer.Models.Examination();
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var query = new Mock<PatientDetailsByCaseIdQuery>(examinationId);
            var dbAccess = new Mock<IDatabaseAccess>();
            dbAccess.Setup(db => db.GetItemAsync<MedicalExaminer.Models.Examination>(connectionSettings.Object,
                    It.IsAny<Expression<Func<MedicalExaminer.Models.Examination, bool>>>()))
                .Returns(Task.FromResult(examination)).Verifiable();
            var sut = new PatientDetailsRetrievalService(dbAccess.Object, connectionSettings.Object);
            var expected = examination;

            // Act
            var result = sut.Handle(query.Object);

            // Assert
            dbAccess.Verify(db => db.GetItemAsync(connectionSettings.Object,
                It.IsAny<Expression<Func<MedicalExaminer.Models.Examination, bool>>>()), Times.Once);
            Assert.Equal(expected, result.Result);
        }
    }
}
