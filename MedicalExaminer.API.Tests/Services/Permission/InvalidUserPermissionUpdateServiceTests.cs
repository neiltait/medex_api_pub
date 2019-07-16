using System.Collections.Generic;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Queries.Permissions;
using MedicalExaminer.Common.Services.Permissions;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Permission
{
    public class InvalidUserPermissionUpdateServiceTests : ServiceTestsBase<
        InvalidUserPermissionQuery,
        UserConnectionSettings,
        bool,
        MeUser,
        InvalidUserPermissionUpdateService>
    {
        [Fact]
        public async void Update_Invalid_PermissionIDs_When_Duplicate_PermissionIDs_Exists()
        {
            // Arrange
            var query = new InvalidUserPermissionQuery();
            var users = GetExamples();

            // Act
            var results = await Service.Handle(query);

            // Assert

        }

        protected override MeUser[] GetExamples()
        {
            return new[]
            {
                new MeUser
                {
                    UserId = "UserId1",
                    Permissions = new[]
                    {
                        new MEUserPermission
                        {
                            PermissionId = "DuplicatePermissionID",
                            LocationId = "SiteId",
                            UserRole = UserRoles.MedicalExaminer
                        }
                    }
                },
                new MeUser
                {
                    UserId = "UserId2",
                    Permissions = new[]
                    {
                        new MEUserPermission
                        {
                            PermissionId = "DuplicatePermissionID",
                            LocationId = "NationalId",
                            UserRole = UserRoles.MedicalExaminerOfficer
                        }
                    }
                }
            };

            List<MeUser> users = new List<MeUser>();
            users.Add(user1);
            users.Add(user2);

            var connectionSettings = new Mock<IUserConnectionSettings>();
            var query = new InvalidUserPermissionQuery();
            var dbAccess = new Mock<IDatabaseAccess>();

            //dbAccess.Setup(db => db.GetItemsAsync(
            //    connectionSettings.Object,
            //    It.IsAny<Expression<Func<IEnumerable<MeUser>, bool>>>())).Returns(Task.FromResult(users)).Verifiable();

            var sut = new InvalidUserPermissionUpdateService(dbAccess.Object, connectionSettings.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            
        }
    }
}
