using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services.User;
using MedicalExaminer.Models;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Users
{
    public class CreateUserServiceTests : ServiceTestsBase<
        CreateUserQuery,
        UserConnectionSettings,
        MeUser,
        MeUser,
        CreateUserService>
    {
        [Fact]
        public async Task Handle_CreatesUser()
        {
            // Arrange
            var expectedEmail = "expectedEmail";
            var expectedUser = new MeUser()
            {
                Email = expectedEmail,
            };

            var query = new CreateUserQuery(expectedUser, new MeUser());

            // Act
            var result = await Service.Handle(query);

            // Assert
            result.Should().NotBeNull();
            result.UserId.Should().NotBeNull();
            result.Email.Should().Be(expectedEmail);
        }

        protected override MeUser[] GetExamples()
        {
            return new[]
            {
                new MeUser()
                {
                    Email = "email1",
                },
                new MeUser()
                {
                    Email = "email2",
                },
            };
        }
    }
}
