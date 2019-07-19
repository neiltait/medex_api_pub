using System;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services.User;
using MedicalExaminer.Models;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Users
{
    public class UserUpdateServiceTests : ServiceTestsBase<
        UserUpdateQuery,
        UserConnectionSettings,
        MeUser,
        MeUser,
        UserUpdateService>
    {
        [Fact]
        public async Task Handle_UpdateUser()
        {
            // Arrange
            var expectedUserId = "expectedUserId";
            var expectedEmail = "email1";
            var expectedUser = new MeUser()
            {
                UserId = expectedUserId,
                Email = expectedEmail,
            };

            var expectedOurUserId = "userId3";
            var expectedOurEmail = "email1";
            var expectedOurUser = new MeUser()
            {
                UserId = expectedOurUserId,
                Email = expectedOurEmail,
            };

            var updateUser = new UserUpdateEmail()
            {
                UserId = expectedUserId,
                Email = expectedEmail
            };

            var query = new UserUpdateQuery(updateUser, expectedOurUser);

            // Act
            var result = await Service.Handle(query);

            // Assert
            result.Should().NotBeNull();
            result.UserId.Should().NotBeNull();
            result.Email.Should().Be(expectedEmail);
        }

        [Fact]
        public async Task Handle_UpdateUser_ThatDoesntExist_ThrowsException()
        {
            // Arrange
            var expectedUserId = "userId3";
            var expectedEmail = "email1";
            var expectedUser = new MeUser()
            {
                UserId = expectedUserId,
                Email = expectedEmail,
            };

            var expectedOurUserId = "userId3";
            var expectedOurEmail = "email1";
            var expectedOurUser = new MeUser()
            {
                UserId = expectedOurUserId,
                Email = expectedOurEmail,
            };

            var updateUser = new UserUpdateEmail()
            {
                UserId = expectedUserId,
                Email = expectedEmail
            };

            var query = new UserUpdateQuery(updateUser, expectedOurUser);

            // Act
            Func<Task> act = async () => await Service.Handle(query);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        protected override MeUser[] GetExamples()
        {
            return new[]
            {
                new MeUser()
                {
                    UserId = "expectedUserId",
                    Email = "email1",
                },
                new MeUser()
                {
                    UserId = "userId2",
                    Email = "email2",
                },
            };
        }
    }
}
