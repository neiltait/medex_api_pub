using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services.User;
using MedicalExaminer.Models;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Users
{
    public class UserRetrievalByOktaIdServiceTests : ServiceTestsBase<
        UserRetrievalByOktaIdQuery,
        UserConnectionSettings,
        MeUser,
        MeUser,
        UserRetrievalByOktaIdService>
    {
        [Fact]
        public async Task Handle_ReturnsOnlyUser1()
        {
            // Arrange
            const string expectedOktaId = "okta1";
            var query = new UserRetrievalByOktaIdQuery(expectedOktaId);

            // Act
            var result = await Service.Handle(query);

            // Assert
            result.Should().NotBeNull();
            result.OktaId.Should().Be(expectedOktaId);
        }

        protected override MeUser[] GetExamples()
        {
            return new[]
            {
                new MeUser()
                {
                    OktaId = "okta1",
                },
                new MeUser()
                {
                    OktaId = "okta2",
                },
            };
        }
    }
}
