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
        }
    }
}
