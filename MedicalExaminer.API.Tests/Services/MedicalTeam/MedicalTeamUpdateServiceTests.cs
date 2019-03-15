using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services.MedicalTeam;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.MedicalTeam
{
    public class MedicalTeamUpdateServiceTests
    {
        [Fact]
        public void ExaminationIdNull_ThrowsException()
        {
            // Arrange
           // IEnumerable<MedicalExaminer.Models.Examination> examinations = null;
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var dbAccess = new Mock<IDatabaseAccess>();
            dbAccess.Setup(db =>
                    db.GetItemAsync(connectionSettings.Object, It.IsAny<Expression<Func<MedicalExaminer.Models.Examination, bool>>>()))
                .Returns(Task.FromResult<MedicalExaminer.Models.Examination>(null));

            var sut = new MedicalTeamUpdateService(dbAccess.Object, connectionSettings.Object);

            // Act

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(null));
        }

        [Fact]
        public void ExaminationFound_ReturnsExaminationId()
        {
            // Arrange
            var examinationId = "1";
            var examination1 = new MedicalExaminer.Models.Examination
            {
                Id = examinationId
            };
            IEnumerable<MedicalExaminer.Models.Examination> examinations = new List<MedicalExaminer.Models.Examination> { examination1};
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var dbAccess = new Mock<IDatabaseAccess>();
            dbAccess.Setup(db => db.UpdateItemAsync(connectionSettings.Object, examination1)).Returns(Task.FromResult(examination1));
            var sut = new MedicalTeamUpdateService(dbAccess.Object, connectionSettings.Object);

            // Act
            var result = sut.Handle(examination1);

            Assert.Equal(examinationId,  result.Result);

        }
    }
}
