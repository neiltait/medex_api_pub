using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services.User;
using MedicalExaminer.Models;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Users
{
    public class UserRetrievalByEmailServiceTests : ServiceTestsBase<
        UserRetrievalByEmailQuery,
        UserConnectionSettings,
        MeUser,
        MeUser,
        UserRetrievalByEmailService>
    {
        [Fact]
        public async Task Handle_ReturnsOnlyUser1()
        {
            // Arrange
            const string expectedEmail = "email1";
            var query = new UserRetrievalByEmailQuery(expectedEmail);

            // Act
            var result = await Service.Handle(query);

            // Assert
            result.Should().NotBeNull();
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
