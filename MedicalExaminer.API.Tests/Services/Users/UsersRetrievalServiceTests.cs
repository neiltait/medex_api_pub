using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services.Examination;
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
        [Fact]
        public async Task Handle_ReturnsOnlyUser1()
        {
            // Arrange
            var query = new UsersRetrievalQuery(null);

            // Act
            var results = (await Service.Handle(query)).ToList();

            // Assert
            results.Should().NotBeNull();
            results.Count.Should().Be(2);
            results.Any(u => u.UserId == "userId1").Should().BeTrue();
            results.Any(u => u.UserId == "userId2").Should().BeTrue();
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
