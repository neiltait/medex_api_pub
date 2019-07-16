using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.CaseOutcome;
using MedicalExaminer.Common.Queries.Permissions;
using MedicalExaminer.Common.Services.Permissions;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Permission
{
    public class InvalidUserPermissionUpdateServiceTests
    {
        [Fact]
        public void Update_Invalid_PermissionIDs_When_Duplicate_PermissionIDs_Exists()
        {
            // Arrange
            var user1 = new MeUser
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
            };

            var user2 = new MeUser
            {
                UserId = "UserId2",
                Permissions = new[]
                {
                    new MEUserPermission
                    {
                        PermissionId = "DuplicatePermissionID",
                        LocationId = "NationalId",
                        UserRole = UserRoles.MedicalExaminerOfficer
                    },
                    new MEUserPermission
                    {
                        PermissionId = "DuplicatePermissionID",
                        LocationId = "SiteId",
                        UserRole = UserRoles.MedicalExaminer
                    }
                }
            };

            List<MeUser> users = new List<MeUser>();
            users.Add(user1);
            users.Add(user2);

            var connectionSettings = new Mock<IUserConnectionSettings>();
            var query = new InvalidUserPermissionQuery();
            var dbAccess = new Mock<IDatabaseAccess>();


            var sut = new InvalidUserPermissionUpdateService(dbAccess.Object, connectionSettings.Object);

            // Act
            var result = sut.Handle(query);

            // Assert
            
        }
    }
}
