using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services.User;
using MedicalExaminer.Models;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Users
{
    public class UserRetrievalByIdServiceTests : ServiceTestsBase<
        UserRetrievalByIdQuery,
        UserConnectionSettings,
        MeUser,
        MeUser,
        UserRetrievalByIdService>
    {
        [Fact]
        public async Task Handle_ReturnsOnlyUser1()
        {
            // Arrange
            const string expectedUserId = "userId1";
            const string expectedEmail = "email1";
            var query = new UserRetrievalByIdQuery(expectedUserId);

            // Act
            var result = await Service.Handle(query);

            // Assert
            result.Should().NotBeNull();
            result.UserId.Should().Be(expectedUserId);
            result.Email.Should().Be(expectedEmail);
        }

        protected override MeUser[] GetExamples()
        {
            return new[]
            {
                new MeUser()
                {
                    UserId = "userId1",
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
