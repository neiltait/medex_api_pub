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
            IEnumerable<MedicalExaminer.Models.Examination> examinations = null;
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var query = new Mock<ExaminationsRetrievalQuery>().Object;
            var dbAccess = new Mock<IDatabaseAccess>();
            dbAccess.Setup(db => db.GetItemsAsync<MedicalExaminer.Models.Examination>(connectionSettings.Object,
                    x=>true))
                .Returns(Task.FromResult(examinations)).Verifiable();
            var sut = new ExaminationsRetrievalService(dbAccess.Object, connectionSettings.Object);
            var expected = default(IEnumerable<MedicalExaminer.Models.Examination>);

            // Act
            var result = sut.Handle(query);

            // Assert
            dbAccess.Verify(db => db.GetItemsAsync<MedicalExaminer.Models.Examination>(connectionSettings.Object, 
                x=> true), Times.Once);
            Assert.Equal(expected, result.Result);
        }

        [Fact]
        public void ExaminationsQueryIsNullThrowsException()
        {
            // Arrange
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            ExaminationsRetrievalQuery query = null;
            var dbAccess = new Mock<IDatabaseAccess>();
            var sut = new ExaminationsRetrievalService(dbAccess.Object, connectionSettings.Object);

            Action act = () => sut.Handle(query);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ExaminationsFoundReturnsResult()
        {
            var examination1 = new MedicalExaminer.Models.Examination();
            var examination2 = new MedicalExaminer.Models.Examination();
            IEnumerable<MedicalExaminer.Models.Examination> examinations = new List<MedicalExaminer.Models.Examination>{examination1, examination2};
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var query = new Mock<ExaminationsRetrievalQuery>().Object;
            var dbAccess = new Mock<IDatabaseAccess>();
            dbAccess.Setup(db => db.GetItemsAsync<MedicalExaminer.Models.Examination>(connectionSettings.Object, 
                    x=> true))
                .Returns(Task.FromResult(examinations)).Verifiable();
            var sut = new ExaminationsRetrievalService(dbAccess.Object, connectionSettings.Object);
            var expected = examinations;

            // Act
            var result = sut.Handle(query);

            // Assert
            dbAccess.Verify(db => db.GetItemsAsync<MedicalExaminer.Models.Examination>(connectionSettings.Object, 
                x=> true), Times.Once);
            Assert.Equal(expected, result.Result);
        }
    }
}
