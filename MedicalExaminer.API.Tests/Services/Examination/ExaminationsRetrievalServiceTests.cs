using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services.Examination;
using MedicalExaminer.Models;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Examination
{
    public class ExaminationsRetrievalServiceTests
    {
        [Fact]
        public void NoExaminationsFoundReturnsNull()
        {
            IEnumerable<IExamination> examinations = null;
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var query = new Mock<ExaminationsRetrivalQuery>().Object;
            var dbAccess = new Mock<IDatabaseAccess>();
            dbAccess.Setup(db => db.QueryAsync<IExamination>(connectionSettings.Object, query.QueryString))
                .Returns(Task.FromResult(examinations)).Verifiable();
            var sut = new ExaminationsRetrivalService(dbAccess.Object, connectionSettings.Object);
            var expected = default(IEnumerable<IExamination>);

            // Act
            var result = sut.Handle(query);

            // Assert
            dbAccess.Verify(db => db.QueryAsync<IExamination>(connectionSettings.Object, query.QueryString), Times.Once);
            Assert.Equal(expected, result.Result);
        }

        [Fact]
        public void ExaminationsQueryIsNullThrowsException()
        {
            // Arrange
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            ExaminationsRetrivalQuery query = null;
            var dbAccess = new Mock<IDatabaseAccess>();
            var sut = new ExaminationsRetrivalService(dbAccess.Object, connectionSettings.Object);

            Action act = () => sut.Handle(query);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ExaminationsFoundReturnsResult()
        {
            var examination1 = new Mock<IExamination>().Object;
            var examination2 = new Mock<IExamination>().Object;

            IEnumerable<IExamination> examinations = new List<IExamination>{examination1, examination2};
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var query = new Mock<ExaminationsRetrivalQuery>().Object;
            var dbAccess = new Mock<IDatabaseAccess>();
            dbAccess.Setup(db => db.QueryAsync<IExamination>(connectionSettings.Object, query.QueryString))
                .Returns(Task.FromResult(examinations)).Verifiable();
            var sut = new ExaminationsRetrivalService(dbAccess.Object, connectionSettings.Object);
            var expected = examinations;

            // Act
            var result = sut.Handle(query);

            // Assert
            dbAccess.Verify(db => db.QueryAsync<IExamination>(connectionSettings.Object, query.QueryString), Times.Once);
            Assert.Equal(expected, result.Result);
        }
    }
}
