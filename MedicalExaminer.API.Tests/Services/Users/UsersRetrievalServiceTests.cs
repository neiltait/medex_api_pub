using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services.User;
using MedicalExaminer.Models;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Users
{
    public class UsersRetrievalServiceTests : ServiceTestsBase<
        UsersRetrievalQuery,
        UserConnectionSettings,
        IEnumerable<MeUser>,
        MeUser,
        UsersRetrievalService>
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Handle_ReturnsUsers(bool forLookup)
        {
            // Arrange
            var query = new UsersRetrievalQuery(forLookup, null);

            // Act
            var results = (await Service.Handle(query)).ToList();

            // Assert
            results.Should().NotBeNull();
            results.Count.Should().Be(2);
            var user1 = results.Single(user => user.UserId == "userId1");
            user1.FirstName.Should().Be("barry");
            user1.LastName.Should().Be("stow");
            var user2 = results.Single(user => user.UserId == "userId2");
            user2.FirstName.Should().Be("john");
            user2.LastName.Should().Be("battye");

            if (!forLookup)
            {
                results.First(u => u.UserId == "userId1").Email.Should().Be("email1");
            }
        }

        protected override MeUser[] GetExamples()
        {
            return new[]
            {
                new MeUser
                {
                    UserId = "userId1",
                    Email = "email1",
                    FirstName = "barry",
                    LastName = "stow"
                },
                new MeUser
                {
                    UserId = "userId2",
                    Email = "email2",
                    FirstName = "john",
                    LastName = "battye"
                },
            };
        }
    }
}
