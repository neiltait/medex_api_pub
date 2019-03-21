using System;
using System.Linq.Expressions;
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
    public class ExaminationRetrievalServiceTests
    {
        [Fact]
        public void ExaminationIdIsNullThrowsException()
        {
            // Arrange
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            ExaminationRetrievalQuery query = null;
            var dbAccess = new Mock<IDatabaseAccess>();
            var sut = new ExaminationRetrievalService(dbAccess.Object, connectionSettings.Object);

            Action act = () => sut.Handle(query).GetAwaiter().GetResult();
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ExaminationIdNotFoundReturnsNull()
        {
            // Arrange
            var examinationId = "a";
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var query = new Mock<ExaminationRetrievalQuery>(examinationId);
            var dbAccess = new Mock<IDatabaseAccess>();

            dbAccess.Setup(db => db.GetItemAsync(connectionSettings.Object,
                    It.IsAny<Expression<Func<MedicalExaminer.Models.Examination, bool>>>()))
                .Returns(Task.FromResult<MedicalExaminer.Models.Examination>(null)).Verifiable();
            var sut = new ExaminationRetrievalService(dbAccess.Object, connectionSettings.Object);
            var expected = default(MedicalExaminer.Models.Examination);

            // Act
            var result = sut.Handle(query.Object);

            // Assert
            dbAccess.Verify(db => db.GetItemAsync(connectionSettings.Object,
                It.IsAny<Expression<Func<MedicalExaminer.Models.Examination, bool>>>()), Times.Once);

            Assert.Equal(expected, result.Result);
        }

        [Fact]
        public void LocationIdFoundReturnsResult()
        {
            var examinationId = "a";
            var examination = new MedicalExaminer.Models.Examination();
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var query = new Mock<ExaminationRetrievalQuery>(examinationId);
            var dbAccess = new Mock<IDatabaseAccess>();
            dbAccess.Setup(db => db.GetItemAsync(connectionSettings.Object,
                    It.IsAny<Expression<Func<MedicalExaminer.Models.Examination, bool>>>()))
                .Returns(Task.FromResult(examination)).Verifiable();
            var sut = new ExaminationRetrievalService(dbAccess.Object, connectionSettings.Object);
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