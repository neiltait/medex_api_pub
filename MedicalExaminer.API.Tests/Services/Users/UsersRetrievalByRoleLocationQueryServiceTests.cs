using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services.User;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Users
{
    public class UsersRetrievalByRoleLocationQueryServiceTests : ServiceTestsBase<
        UsersRetrievalByRoleLocationQuery,
        UserConnectionSettings,
        IEnumerable<MeUser>,
        MeUser,
        UsersRetrievalByRoleLocationQueryService>
    {
        [Fact]
        public async Task Handle_ReturnsOnlyUser1()
        {
            // Arrange
            var expectedRole = UserRoles.MedicalExaminer;
            var expectedLocations = new[] { "location1" };
            var query = new UsersRetrievalByRoleLocationQuery(expectedLocations, expectedRole);

            // Act
            var results = (await Service.Handle(query)).ToList();

            // Assert
            results.Should().NotBeNull();
            results.Count.Should().Be(1);
            results.ElementAt(0).UserId.Should().Be("user1");
        }

        [Fact]
        public async Task Handle_ReturnsOnlyUser2()
        {
            // Arrange
            var expectedRole = UserRoles.MedicalExaminerOfficer;
            var expectedLocations = new[] { "location1", "location2", "location3" };
            var query = new UsersRetrievalByRoleLocationQuery(expectedLocations, expectedRole);

            // Act
            var results = (await Service.Handle(query)).ToList();

            // Assert
            results.Should().NotBeNull();
            results.Count.Should().Be(1);
            results.ElementAt(0).UserId.Should().Be("user2");
        }

        [Fact]
        public async Task Handle_ReturnsUser1And2()
        {
            // Arrange
            var expectedRole = UserRoles.MedicalExaminer;
            var expectedLocations = new[] { "location1", "location2", "location3" };
            var query = new UsersRetrievalByRoleLocationQuery(expectedLocations, expectedRole);

            // Act
            var results = (await Service.Handle(query)).ToList();

            // Assert
            results.Should().NotBeNull();
            results.Count.Should().Be(2);
            results.Count(r => r.UserId == "user1").Should().Be(1);
            results.Count(r => r.UserId == "user2").Should().Be(1);
        }

        protected override MeUser[] GetExamples()
        {
            return new[]
            {
                new MeUser()
                {
                    UserId = "user1",
                    Permissions = new[]
                    {
                        new MEUserPermission
                        {
                            LocationId = "location1",
                            UserRole = (int)UserRoles.MedicalExaminer,
                        }
                    }
                },
                new MeUser()
                {
                    UserId = "user2",
                    Permissions = new[]
                    {
                        new MEUserPermission
                        {
                            LocationId = "location2",
                            UserRole = (int)UserRoles.MedicalExaminer,
                        },
                        new MEUserPermission
                        {
                            LocationId = "location3",
                            UserRole = (int)UserRoles.MedicalExaminerOfficer,
                        }
                    }
                },
            };
        }
    }
}