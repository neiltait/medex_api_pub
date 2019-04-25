using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Common.Services.MedicalTeam;
using MedicalExaminer.Common.Services.User;
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
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var dbAccess = new Mock<IDatabaseAccess>();
            dbAccess.Setup(db =>
                    db.GetItemAsync(connectionSettings.Object,
                        It.IsAny<Expression<Func<MedicalExaminer.Models.Examination, bool>>>()))
                .Returns(Task.FromResult<MedicalExaminer.Models.Examination>(null));
            var user = new MedicalExaminer.Models.MeUser();
            var userRetrievalByIdService = new Mock<IAsyncQueryHandler<UserRetrievalByIdQuery, MedicalExaminer.Models.MeUser>>();

            userRetrievalByIdService.Setup(x => x.Handle(It.IsAny<UserRetrievalByIdQuery>())).Returns(Task.FromResult(user));
            var sut = new MedicalTeamUpdateService(dbAccess.Object, connectionSettings.Object, userRetrievalByIdService.Object);

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(null, "a"));
        }

        [Fact]
        public async void ExaminationFound_ReturnsExaminationId()
        {
            // Arrange
            var examinationId = "1";
            var examination1 = new MedicalExaminer.Models.Examination
            {
                ExaminationId = examinationId
            };

            IEnumerable<MedicalExaminer.Models.Examination> examinations = new List<MedicalExaminer.Models.Examination>
                { examination1 };
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var userConnectionSettings = new Mock<IUserConnectionSettings>();

            var dbAccess = new Mock<IDatabaseAccess>();
            var user = new MedicalExaminer.Models.MeUser();
            var userRetrievalByIdService = new Mock<IAsyncQueryHandler<UserRetrievalByIdQuery, MedicalExaminer.Models.MeUser>>();

            userRetrievalByIdService.Setup(x => x.Handle(It.IsAny<UserRetrievalByIdQuery>())).Returns(Task.FromResult(user));

            dbAccess.Setup(db => db.UpdateItemAsync(connectionSettings.Object, examination1))
                .Returns(Task.FromResult(examination1));
            var sut = new MedicalTeamUpdateService(dbAccess.Object, connectionSettings.Object, userRetrievalByIdService.Object);

            // Act
            var result = await sut.Handle(examination1, "a");

            // Assert
            Assert.Equal(examinationId, result.ExaminationId);
            Assert.Equal("a", result.LastModifiedBy);
        }

        /// <summary>
        /// Test to make sure UpdateCaseUrgencyScore method is called whenever the Examination is updated
        /// </summary>
        [Fact]
        public async void UpdateMedicalTeamOfExaminationWithNoUrgencyIndicatorsThenCaseUrgencyScoreUpdatedWithZero()
        {
            // Arrange
            var examination = new MedicalExaminer.Models.Examination
            {
                ChildPriority = false,
                CoronerPriority = false,
                CulturalPriority = false,
                FaithPriority = false,
                OtherPriority = false,
                CreatedAt = DateTime.Now.AddDays(-3)
            };
            var user = new MedicalExaminer.Models.MeUser();
            var userRetrievalByIdService = new Mock<IAsyncQueryHandler<UserRetrievalByIdQuery, MedicalExaminer.Models.MeUser>>();

            userRetrievalByIdService.Setup(x => x.Handle(It.IsAny<UserRetrievalByIdQuery>())).Returns(Task.FromResult(user));
            IEnumerable<MedicalExaminer.Models.Examination> examinations = new List<MedicalExaminer.Models.Examination>
                { examination };
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var dbAccess = new Mock<IDatabaseAccess>();
            dbAccess.Setup(db => db.UpdateItemAsync(connectionSettings.Object, examination))
                .Returns(Task.FromResult(examination));
            var sut = new MedicalTeamUpdateService(dbAccess.Object, connectionSettings.Object, userRetrievalByIdService.Object);

            // Act
            var result = await sut.Handle(examination, "a");

            // Assert
            Assert.Equal(0, result.UrgencyScore);
            Assert.Equal("a", result.LastModifiedBy);
        }

        /// <summary>
        /// Test to make sure UpdateCaseUrgencyScore method is called whenever the Examination is updated
        /// </summary>
        [Fact]
        public async void UpdateMedicalTeamOfExaminationWithAllUrgencyIndicatorsThenCaseUrgencyScoreUpdatedWith500()
        {
            // Arrange
            var examination = new MedicalExaminer.Models.Examination
            {
                ChildPriority = true,
                CoronerPriority = true,
                CulturalPriority = true,
                FaithPriority = true,
                OtherPriority = true,
                CreatedAt = DateTime.Now.AddDays(-3)
            };

            var user = new MedicalExaminer.Models.MeUser();
            var userRetrievalByIdService = new Mock<IAsyncQueryHandler<UserRetrievalByIdQuery, MedicalExaminer.Models.MeUser>>();

            userRetrievalByIdService.Setup(x => x.Handle(It.IsAny<UserRetrievalByIdQuery>())).Returns(Task.FromResult(user));
            IEnumerable<MedicalExaminer.Models.Examination> examinations = new List<MedicalExaminer.Models.Examination>
                {examination};
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var dbAccess = new Mock<IDatabaseAccess>();
            dbAccess.Setup(db => db.UpdateItemAsync(connectionSettings.Object, examination))
                .Returns(Task.FromResult(examination));
            var sut = new MedicalTeamUpdateService(dbAccess.Object, connectionSettings.Object, userRetrievalByIdService.Object);

            // Act
            var result = await sut.Handle(examination, "a");

            // Assert
            Assert.Equal(500, result.UrgencyScore);
            Assert.Equal("a", result.LastModifiedBy);
        }
    }
}