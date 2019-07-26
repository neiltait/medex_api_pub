using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Common.Services.MedicalTeam;
using MedicalExaminer.Models;
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
                    db.GetItemAsync(
                        connectionSettings.Object,
                        It.IsAny<Expression<Func<MedicalExaminer.Models.Examination, bool>>>()))
                .Returns(Task.FromResult<MedicalExaminer.Models.Examination>(null));
            var user = new MeUser();
            var userRetrievalByIdService = new Mock<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>>();

            userRetrievalByIdService.Setup(x => x.Handle(It.IsAny<UserRetrievalByIdQuery>())).Returns(Task.FromResult(user));
            var sut = new MedicalTeamUpdateService(dbAccess.Object, connectionSettings.Object, userRetrievalByIdService.Object);

            // Assert
            Action act = () => sut.Handle(null, "a").GetAwaiter().GetResult();
            act.Should().Throw<ArgumentNullException>();
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

            var connectionSettings = new Mock<IExaminationConnectionSettings>();

            var dbAccess = new Mock<IDatabaseAccess>();
            var user = new MeUser();
            var userRetrievalByIdService = new Mock<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>>();

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
            var user = new MeUser();
            var userRetrievalByIdService = new Mock<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>>();

            userRetrievalByIdService.Setup(x => x.Handle(It.IsAny<UserRetrievalByIdQuery>())).Returns(Task.FromResult(user));

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

            var user = new MeUser();
            var userRetrievalByIdService = new Mock<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>>();

            userRetrievalByIdService.Setup(x => x.Handle(It.IsAny<UserRetrievalByIdQuery>())).Returns(Task.FromResult(user));

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

        /// <summary>
        /// Test to make sure full names are cleared and user service is not called when null id's given.
        /// </summary>
        [Fact]
        public async void UpdateMedicalTeamOfExamination_SetsFullNameToNull_WhenMeUserIdNull()
        {
            // Arrange
            var examination = new MedicalExaminer.Models.Examination
            {
                ChildPriority = true,
                CoronerPriority = true,
                CulturalPriority = true,
                FaithPriority = true,
                OtherPriority = true,
                CreatedAt = DateTime.Now.AddDays(-3),
                MedicalTeam = new MedicalExaminer.Models.MedicalTeam
                {
                    MedicalExaminerUserId = null,
                    MedicalExaminerOfficerUserId = null,
                }
            };

            var userRetrievalByIdService = new Mock<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>>(MockBehavior.Strict);
            var connectionSettings = new Mock<IExaminationConnectionSettings>(MockBehavior.Strict);
            var dbAccess = new Mock<IDatabaseAccess>(MockBehavior.Strict);

            userRetrievalByIdService
                .Setup(x => x.Handle(It.IsAny<UserRetrievalByIdQuery>()))
                .Returns(Task.FromResult(new MeUser()))
                .Verifiable();

            dbAccess
                .Setup(db => db.UpdateItemAsync(connectionSettings.Object, examination))
                .Returns(Task.FromResult(examination));

            var sut = new MedicalTeamUpdateService(
                dbAccess.Object,
                connectionSettings.Object,
                userRetrievalByIdService.Object);

            // Act
            var result = await sut.Handle(examination, "a");

            // Assert
            result.MedicalTeam.MedicalExaminerFullName.Should().BeNull();
            result.MedicalTeam.MedicalExaminerOfficerFullName.Should().BeNull();

            userRetrievalByIdService
                .Verify(x => x.Handle(It.IsAny<UserRetrievalByIdQuery>()), Times.Never);
        }

        /// <summary>
        /// Test to make sure full name is populated if user id's are given.
        /// </summary>
        [Fact]
        public async void UpdateMedicalTeamOfExamination_SetsFullName_WhenMeUserIdNotNull()
        {
            // Arrange
            const string medicalExaminer = "medicalExaminer";
            const string medicalExaminerOfficer = "medicalExaminerOfficer";
            const string lastName = "lastName";
            var medicalExaminerFullName = $"{medicalExaminer} {lastName}";
            var medicalExaminerOfficerFullName = $"{medicalExaminerOfficer} {lastName}";
            const string medicalExaminerUserId = "medicalExaminerUserId";
            const string medicalExaminerOfficerUserId = "medicalExaminerOfficerUserId";

            var examination = new MedicalExaminer.Models.Examination
            {
                ChildPriority = true,
                CoronerPriority = true,
                CulturalPriority = true,
                FaithPriority = true,
                OtherPriority = true,
                CreatedAt = DateTime.Now.AddDays(-3),
                MedicalTeam = new MedicalExaminer.Models.MedicalTeam
                {
                    MedicalExaminerUserId = medicalExaminerUserId,
                    MedicalExaminerOfficerUserId = medicalExaminerOfficerUserId,
                }
            };

            var userRetrievalByIdService = new Mock<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>>(MockBehavior.Strict);
            var connectionSettings = new Mock<IExaminationConnectionSettings>(MockBehavior.Strict);
            var dbAccess = new Mock<IDatabaseAccess>(MockBehavior.Strict);

            userRetrievalByIdService
                .Setup(x => x.Handle(It.Is<UserRetrievalByIdQuery>(query => query.UserId == medicalExaminerUserId)))
                .Returns(Task.FromResult(new MeUser()
                {
                    FirstName = medicalExaminer,
                    LastName = lastName,
                }));
            userRetrievalByIdService
                .Setup(x => x.Handle(It.Is<UserRetrievalByIdQuery>(query => query.UserId == medicalExaminerOfficerUserId)))
                .Returns(Task.FromResult(new MeUser()
                {
                    FirstName = medicalExaminerOfficer,
                    LastName = lastName,
                }));

            dbAccess
                .Setup(db => db.UpdateItemAsync(connectionSettings.Object, examination))
                .Returns(Task.FromResult(examination));

            var sut = new MedicalTeamUpdateService(
                dbAccess.Object,
                connectionSettings.Object,
                userRetrievalByIdService.Object);

            // Act
            var result = await sut.Handle(examination, "a");

            // Assert
            result.MedicalTeam.MedicalExaminerFullName.Should().Be(medicalExaminerFullName);
            result.MedicalTeam.MedicalExaminerOfficerFullName.Should().Be(medicalExaminerOfficerFullName);
        }
    }
}