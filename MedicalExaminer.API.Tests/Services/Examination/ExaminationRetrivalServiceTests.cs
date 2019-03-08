using System;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services.Examination;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Examination
{
    public class ExaminationRetrivalServiceTests
    {
        [Fact]
        public void ExaminationIdNotFoundReturnsNull()
        {
            // Arrange
            var examinationId = "a";
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var query = new Mock<ExaminationRetrivalQuery>(examinationId);
            var dbAccess = new Mock<IDatabaseAccess>();
            dbAccess.Setup(db => db.QuerySingleAsync<MedicalExaminer.Models.Examination>(connectionSettings.Object, query.Object.ExaminationId))
                .Returns(Task.FromResult<MedicalExaminer.Models.Examination>(null)).Verifiable();
            var sut = new ExaminationRetrivalService(dbAccess.Object, connectionSettings.Object);
            var expected = default(MedicalExaminer.Models.Examination);

            // Act
            var result = sut.Handle(query.Object);

            // Assert
            dbAccess.Verify(db => db.QuerySingleAsync<MedicalExaminer.Models.Examination>(connectionSettings.Object, "a"), Times.Once);

            Assert.Equal(expected, result.Result);

        }

        [Fact]
        public void ExaminationIdIsNullThrowsException()
        {
            // Arrange
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            ExaminationRetrivalQuery query = null;
            var dbAccess = new Mock<IDatabaseAccess>();
            var sut = new ExaminationRetrivalService(dbAccess.Object, connectionSettings.Object);
            
            Action act = () => sut.Handle(query);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void LocationIdFoundReturnsResult()
        {
            var examinationId = "a";
            var examination = new MedicalExaminer.Models.Examination();
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var query = new Mock<ExaminationRetrivalQuery>(examinationId);
            var dbAccess = new Mock<IDatabaseAccess>();
            dbAccess.Setup(db => db.QuerySingleAsync<MedicalExaminer.Models.Examination>(connectionSettings.Object, query.Object.ExaminationId))
                .Returns(Task.FromResult(examination)).Verifiable();
            var sut = new ExaminationRetrivalService(dbAccess.Object, connectionSettings.Object);
            var expected = examination;

            // Act
            var result = sut.Handle(query.Object);

            // Assert
            dbAccess.Verify(db => db.QuerySingleAsync<MedicalExaminer.Models.Examination>(connectionSettings.Object, query.Object.ExaminationId), Times.Once);
            Assert.Equal(expected, result.Result);
        }
    }
}
