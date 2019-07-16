using System.Linq;
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
        private MeUser user1 = new MeUser
        {
            UserId = "UserId1",
            Permissions = new[]
            {
                new MEUserPermission
                {
                    PermissionId = "DuplicatePermissionID",
                    LocationId = "SiteId",
                    UserRole = UserRoles.MedicalExaminer
                },
                new MEUserPermission
                {
                    PermissionId = "NotDuplicatePermissionID",
                    LocationId = "SiteId",
                    UserRole = UserRoles.MedicalExaminer
                }
            }
        };

        private MeUser duplicatePermissionUser = new MeUser
        {
            UserId = "UserId2",
            Permissions = new[]
            {
                new MEUserPermission
                {
                    PermissionId = "DuplicatePermissionID",
                    LocationId = "NationalId",
                    UserRole = UserRoles.MedicalExaminer
                },
                new MEUserPermission
                {
                    PermissionId = "DuplicatePermissionID",
                    LocationId = "NationalId",
                    UserRole = UserRoles.MedicalExaminerOfficer
                }
            }
        };

        private MeUser nullPermissionUser = new MeUser
        {
            UserId = "UserId2",
            Permissions = new[]
            {
                new MEUserPermission
                {
                    PermissionId = null,
                    LocationId = "NationalId",
                    UserRole = UserRoles.MedicalExaminer
                },
                new MEUserPermission
                {
                    PermissionId = null,
                    LocationId = "NationalId",
                    UserRole = UserRoles.MedicalExaminerOfficer
                }
            }
        };

        private MeUser whiteSpaceStringEmptyPermissionUser = new MeUser
        {
            UserId = "UserId2",
            Permissions = new[]
            {
                new MEUserPermission
                {
                    PermissionId = string.Empty,
                    LocationId = "NationalId",
                    UserRole = UserRoles.MedicalExaminer
                },
                new MEUserPermission
                {
                    PermissionId = "",
                    LocationId = "NationalId",
                    UserRole = UserRoles.MedicalExaminerOfficer
                }
            }
        };

        [Fact]
        public async void No_Update_Valid_PermissionIDs_When_No_Duplicate_PermissionIDs_Exists()
        {
            // Arrange
            var query = new InvalidUserPermissionQuery();

            var user2permissionId1 = user1.Permissions.First().PermissionId;
            var user2permissionId2 = user1.Permissions.Last().PermissionId;

            // Act
            var result = await Service.Handle(query);

            // Assert
            Assert.Equal(user1.Permissions.First().PermissionId, user2permissionId1);
            Assert.Equal(user1.Permissions.Last().PermissionId, user2permissionId2);
        }

        [Fact]
        public async void Update_Invalid_PermissionIDs_When_Duplicate_PermissionIDs_Exists()
        {
            // Arrange
            var query = new InvalidUserPermissionQuery();

            var user2permissionId1 = duplicatePermissionUser.Permissions.First().PermissionId;
            var user2permissionId2 = duplicatePermissionUser.Permissions.Last().PermissionId;

            // Act
            var result = await Service.Handle(query);

            // Assert
            Assert.NotEqual(duplicatePermissionUser.Permissions.First().PermissionId, user2permissionId1);
            Assert.NotEqual(duplicatePermissionUser.Permissions.Last().PermissionId, user2permissionId2);
        }

        [Fact]
        public async void Update_Invalid_PermissionIDs_When_Null_PermissionIDs_Exists()
        {
            // Arrange
            var query = new InvalidUserPermissionQuery();

            
            // Act
            var result = await Service.Handle(query);

            // Assert
            Assert.NotNull(nullPermissionUser.Permissions.First().PermissionId);
            Assert.NotNull(nullPermissionUser.Permissions.Last().PermissionId);
        }

        [Fact]
        public async void Update_Invalid_PermissionIDs_When_WhiteSpace_PermissionIDs_Exists()
        {
            // Arrange
            var query = new InvalidUserPermissionQuery();
            
            // Act
            var result = await Service.Handle(query);

            // Assert
            Assert.NotEqual(whiteSpaceStringEmptyPermissionUser.Permissions.First().PermissionId, string.Empty);
            Assert.NotEqual(whiteSpaceStringEmptyPermissionUser.Permissions.Last().PermissionId, string.Empty);
        }

        protected override MeUser[] GetExamples()
        {
            return new[]
            {
                user1, duplicatePermissionUser, nullPermissionUser, whiteSpaceStringEmptyPermissionUser
            };
        }
    }
}
