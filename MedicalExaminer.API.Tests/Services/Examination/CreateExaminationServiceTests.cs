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
    public class CreateExaminationServiceTests
    {
        [Fact]
        public void CreateExaminationQuerySuccessReturnsExaminationId()
        {
            // Arrange
            MedicalExaminer.Models.Examination examination = new MedicalExaminer.Models.Examination();
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var query = new CreateExaminationQuery(examination);
            var dbAccess = new Mock<IDatabaseAccess>();
            dbAccess.Setup((db)  => db.CreateItemAsync(connectionSettings.Object, 
                examination, false)).Returns(Task.FromResult(examination)).Verifiable();
            var sut = new CreateExaminationService(dbAccess.Object, connectionSettings.Object);
            
            // Act
            var result = sut.Handle(query);

            // Assert
            dbAccess.Verify(db => db.CreateItemAsync(connectionSettings.Object, examination, false), Times.Once);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public void CreateExaminationQueryIsNullThrowsException()
        {
            // Arrange
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            CreateExaminationQuery query = null;
            var dbAccess = new Mock<IDatabaseAccess>();
            var sut = new CreateExaminationService(dbAccess.Object, connectionSettings.Object);

            Action act = () => sut.Handle(query).GetAwaiter().GetResult();
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
